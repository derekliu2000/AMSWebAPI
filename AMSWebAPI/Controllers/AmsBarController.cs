using AMSWebAPI.Common;
using API.Common.AMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmsBarController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AmsBarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST api/AmsBar
        [HttpPost]
        public IActionResult PostAmsBar([FromBody] string value)
        {
            Buffer_Bar barBuffer;

            try
            {
                barBuffer = new Buffer_Bar(value, Request.Headers["Data-Hash"]);
                if (barBuffer.DBName == "")
                {
                    return Ok("Hash Error");
                }
            }
            catch (Exception e)
            {
                return Ok("new Buffer_Bar exception: " + e.Message);
            }
            
            try
            {
                UpdateBar_SQL(barBuffer);
            }
            catch (Exception e)
            {
                return Ok("UpdateBar_SQL exception: " + e.Message);
            }

            return Ok("Success");
        }

        private void UpdateBar_SQL(Buffer_Bar barBuffer)
        {
            SQLParts sqlParts = SQLParts.ConstructSQLParts(barBuffer.chEnList, barBuffer.auxEnList, "BAR", 0);
            
            string connectionString = string.Format(_configuration.GetValue<string>("ConnectionStrings:SiteConnection"), barBuffer.DBName);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = sqlConn.CreateCommand();
                SqlTransaction transaction = sqlConn.BeginTransaction("AddBarTrans");

                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = transaction;

                try
                {
                    // Add values to AMS_DataBar table
                    sqlCmd.Parameters.Clear();
                    sqlCmd.CommandText = $"IF NOT EXISTS (SELECT * FROM AMS_DataBar WHERE UTC=@UTC) INSERT INTO AMS_DataBar ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                    sqlCmd.Parameters.AddWithValue("@UTC", barBuffer.UTC);
                    for (int idx = 0; idx < barBuffer.chEnList.Count; idx++)
                        sqlCmd.Parameters.AddWithValue($"C{barBuffer.chEnList[idx] + 1}", barBuffer.chValList[idx]);
                    for (int idx = 0; idx < barBuffer.auxEnList.Count; idx++)
                        sqlCmd.Parameters.AddWithValue($"A{barBuffer.auxEnList[idx] + 1}", barBuffer.auxValList[idx]);
                    sqlCmd.ExecuteNonQuery();

                    // Add values to AMS_DataBarFlags_n tables
                    for (int tblIndex = 0; tblIndex < AMSOperator.TABLE_BAR_FLAGS.Length; tblIndex++)
                    {
                        int minChId = AMSSysInfo.CH_COUNT / AMSOperator.TABLE_BAR_FLAGS.Length * tblIndex;
                        int maxChId = AMSSysInfo.CH_COUNT / AMSOperator.TABLE_BAR_FLAGS.Length * (tblIndex + 1) - 1;

                        sqlParts = SQLParts.ConstructSQLParts(barBuffer.chEnList, null, "BAR_FLAGS", tblIndex);
                        if (sqlParts.ContainValues)
                        {
                            sqlCmd.Parameters.Clear();
                            sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM {AMSOperator.TABLE_BAR_FLAGS[tblIndex]} WHERE UTC=@UTC) " +
                                                 $"INSERT INTO {AMSOperator.TABLE_BAR_FLAGS[tblIndex]} ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                            for (int idx = 0; idx < barBuffer.chEnList.Count; idx++)
                            {
                                if (barBuffer.chEnList[idx] >= minChId && barBuffer.chEnList[idx] <= maxChId)
                                {
                                    sqlCmd.Parameters.AddWithValue($"@fC{barBuffer.chEnList[idx] + 1}", barBuffer.chMiscAlmPackList[idx]);
                                    sqlCmd.Parameters.AddWithValue($"@gC{barBuffer.chEnList[idx] + 1}", barBuffer.chGainList[idx]);
                                }
                            }
                            if (sqlCmd.Parameters.Count > 0)
                            {
                                sqlCmd.Parameters.AddWithValue("@UTC", barBuffer.UTC);
                                sqlCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("Failed to rollback.", ex2);
                    }
                }
            }
        }
    }
}
