using AMSWebAPI.Common;
using API.Common.AMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebAPI.Controllers
{
    public class SQLParts
    {
        public string ColumnNames { get; set; }
        public string Values { get; set; }
        public Boolean ContainValues { get; set; }

        public SQLParts()
        {
            this.ColumnNames = "";
            this.Values = "";
            this.ContainValues = false;
        }

        static public SQLParts ConstructSQLParts(List<short> list1, List<short> list2, string type, int tblIndex)
        {
            SQLParts sqlParts = new SQLParts();
            sqlParts.ColumnNames = "UTC";
            sqlParts.Values = "@UTC";

            if (type == "RMS")
            {
                int minChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_RMS.Length * tblIndex;
                int maxChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_RMS.Length * (tblIndex + 1) - 1;

                for (int idx = 0; idx < list1.Count; idx++)
                {
                    if (list1[idx] >= minChId && list1[idx] <= maxChId)
                    {
                        sqlParts.ColumnNames += $",C{list1[idx] + 1}";
                        sqlParts.Values += $",@C{list1[idx] + 1}";
                        sqlParts.ContainValues = true;
                    }
                }
            }
            else if (type == "AUX")
            {
                for (int idx = 0; idx < list1.Count; idx++)
                {
                    sqlParts.ColumnNames += $",A{list1[idx] + 1}";
                    sqlParts.Values += $",@A{list1[idx] + 1}";
                    sqlParts.ContainValues = true;
                }
            }
            else if (type == "BAR")
            {
                for (int idx = 0; idx < list1.Count; idx++)
                {
                    sqlParts.ColumnNames += $",C{list1[idx] + 1}";
                    sqlParts.Values += $",@C{list1[idx] + 1}";
                    sqlParts.ContainValues = true;
                }
                for (int idx = 0; idx < list2.Count; idx++)
                {
                    sqlParts.ColumnNames += $",A{list2[idx] + 1}";
                    sqlParts.Values += $",@A{list2[idx] + 1}";
                    sqlParts.ContainValues = true;
                }
            }
            else if (type == "BAR_FLAGS")
            {
                int minChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_BAR_FLAGS.Length * tblIndex;
                int maxChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_BAR_FLAGS.Length * (tblIndex + 1) - 1;

                for (int idx = 0; idx < list1.Count; idx++)
                {
                    if (list1[idx] >= minChId && list1[idx] <= maxChId)
                    {
                        sqlParts.ColumnNames += $",fC{list1[idx] + 1},gC{list1[idx] + 1}";
                        sqlParts.Values += $",@fC{list1[idx] + 1},@gC{list1[idx] + 1}";
                        sqlParts.ContainValues = true;
                    }
                }
            }
            else if (type == "JRN")
            {
                int minChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_JRN.Length * tblIndex;
                int maxChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_JRN.Length * (tblIndex + 1) - 1;

                for (int idx = 0; idx < list1.Count; idx++)
                {
                    if (list1[idx] >= minChId && list1[idx] <= maxChId)
                    {
                        sqlParts.ColumnNames += $",C{list1[idx] + 1}";
                        sqlParts.Values += $",@C{list1[idx] + 1}";
                        sqlParts.ContainValues = true;
                    }
                }
            }

            return sqlParts;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AmsRMSController : ControllerBase
    {
        private readonly ILogger<AmsRMSController> _logger;
        private readonly IConfiguration _configuration;

        public AmsRMSController(ILogger<AmsRMSController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("RMSMaxUTC")]
        public IActionResult GetRMSMaxUTC(string p)
        {
            MaxUTCBuffer maxUTCBuffer;

            try
            {
                maxUTCBuffer = new MaxUTCBuffer(p, Request.Headers["Data-Hash"]);
                if (maxUTCBuffer.DBName == "")
                {
                    _logger.LogWarning($"new MaxUTCBuffer Hash Error({Request.Headers["DBName"]})");
                    return Ok("Hash Error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"new MaxUTCBuffer exception in GetRMSMaxUTC({Request.Headers["DBName"]}): " + e.Message);
                return BadRequest("new MaxUTCBuffer exception in GetRMSMaxUTC: " + e.Message);
            }

            try
            {
                DateTime maxUTC = GetRMSMaxUTC_SQL(maxUTCBuffer);
                return Ok(maxUTC);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get RMS Max UTC from DB exception({Request.Headers["DBName"]}): " + ex.Message);
                return BadRequest("Get RMS Max UTC from DB exception: " + ex.Message);
            }
        }

        private DateTime GetRMSMaxUTC_SQL(MaxUTCBuffer maxUTCBuffer)
        {
            DateTime maxUTC = DateTime.MinValue;

            string connectionString = string.Format(_configuration.GetValue<string>("ConnectionStrings:SiteConnection"), maxUTCBuffer.DBName);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = new SqlCommand($"SELECT MAX(UTC) FROM {AMSOperator.TABLE_RMS[maxUTCBuffer.tblIdx]}", sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr[0] != DBNull.Value)
                            maxUTC = dr.GetDateTime(0);
                    }
                }
                dr.Close();
            }

            return maxUTC;
        }

        [HttpPost("AddRMSBlocks")]
        public IActionResult AddRMSBlocks([FromBody] string value)
        {
            Buffer_RMS rmsBuffer;

            try
            {
                rmsBuffer = new Buffer_RMS(value, Request.Headers["Data-Hash"]);
                if (rmsBuffer.DBName == "")
                {
                    _logger.LogWarning($"new Buffer_RMS Hash Error({Request.Headers["DBName"]})");
                    return Ok("Hash Error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"new Buffer_RMS exception({Request.Headers["DBName"]}): " + e.Message);
                return Ok("new Buffer_RMS exception: " + e.Message);
            }

            try
            {
                for (int i = 0; i < rmsBuffer.blockCount; i++)
                {
                    DateTime UTC = DateTime.MinValue;
                    List<short> chValList = null;
                    List<short> auxValList = null;

                    Buffer_RMS.GetRMSRecordByIndex(rmsBuffer.rmsBuffer, i, rmsBuffer.chEnList, rmsBuffer.auxEnList, ref UTC, ref chValList, ref auxValList);
                    if (UTC != DateTime.MinValue)
                    {
                        AddOneRMSBlock(rmsBuffer, UTC, chValList, auxValList);
                    }
                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddOneRMSBlock exception({Request.Headers["DBName"]}): " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private void AddOneRMSBlock(Buffer_RMS rmsBuffer, DateTime UTC, List<short> chValList, List<short> auxValList)
        {
            SQLParts sqlParts;
            string connectionString = string.Format(_configuration.GetValue<string>("ConnectionStrings:SiteConnection"), rmsBuffer.DBName);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = sqlConn.CreateCommand();
                SqlTransaction transaction = sqlConn.BeginTransaction("AddRMSTrans");
                Boolean timestamp = false;

                sqlCmd.Connection = sqlConn;
                sqlCmd.Transaction = transaction;

                try
                {
                    for (int tblIndex = 0; tblIndex < AMSOperator.TABLE_RMS.Length; tblIndex++)
                    {
                        int minChId = AMSSysInfo.CH_COUNT / AMSOperator.TABLE_RMS.Length * tblIndex;
                        int maxChId = AMSSysInfo.CH_COUNT / AMSOperator.TABLE_RMS.Length * (tblIndex + 1) - 1;

                        if (rmsBuffer.chEnList.Max() < minChId)
                            break;

                        sqlParts = SQLParts.ConstructSQLParts(rmsBuffer.chEnList, null, "RMS", tblIndex);
                        if (sqlParts.ContainValues)
                        {
                            // Add records to RMS tables
                            sqlCmd.Parameters.Clear();
                            sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM {AMSOperator.TABLE_RMS[tblIndex]} WHERE UTC=@UTC) " +
                                                 $"INSERT INTO {AMSOperator.TABLE_RMS[tblIndex]} ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                            for (int idx = 0; idx < rmsBuffer.chEnList.Count; idx++)
                            {
                                if (rmsBuffer.chEnList[idx] >= minChId && rmsBuffer.chEnList[idx] <= maxChId)
                                {
                                    sqlCmd.Parameters.AddWithValue($"@C{rmsBuffer.chEnList[idx] + 1}", chValList[idx] & Convert.ToInt32("11111111111111", 2));
                                }
                            }
                            if (sqlCmd.Parameters.Count > 0)
                            {
                                sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                                sqlCmd.ExecuteNonQuery();
                            }

                            // Add records to RMSFlags tables
                            sqlCmd.Parameters.Clear();
                            sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM {AMSOperator.TABLE_RMS_FLAGS[tblIndex]} WHERE UTC=@UTC) " +
                                                 $"INSERT INTO {AMSOperator.TABLE_RMS_FLAGS[tblIndex]} ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                            for (int idx = 0; idx < rmsBuffer.chEnList.Count; idx++)
                            {
                                if (rmsBuffer.chEnList[idx] >= minChId && rmsBuffer.chEnList[idx] <= maxChId)
                                {
                                    sqlCmd.Parameters.AddWithValue($"@C{rmsBuffer.chEnList[idx] + 1}", (chValList[idx] >> 14) & 3);
                                }
                            }
                            if (sqlCmd.Parameters.Count > 0)
                            {
                                sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                                sqlCmd.ExecuteNonQuery();

                                // Add timestamp to AMS_DataRMSTimestamp if needed
                                if (timestamp == false)
                                {
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM AMS_DataRMSTimestamp WHERE UTC=@UTC) " +
                                                         $"INSERT INTO AMS_DataRMSTimestamp (UTC) VALUES (@UTC)";
                                    sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                                    sqlCmd.ExecuteNonQuery();

                                    timestamp = true;
                                }
                            }
                        }
                    }

                    // Add aux record to AMS_DataAux table
                    sqlParts = SQLParts.ConstructSQLParts(rmsBuffer.auxEnList, null, "AUX", 0);
                    sqlCmd.Parameters.Clear();
                    sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM AMS_DataAux WHERE UTC=@UTC) " +
                                         $"INSERT INTO AMS_DataAux ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                    sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                    for (int idx = 0; idx < auxValList.Count; idx++)
                    {
                        sqlCmd.Parameters.AddWithValue($"@A{rmsBuffer.auxEnList[idx] + 1}", auxValList[idx]);
                    }
                    sqlCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
