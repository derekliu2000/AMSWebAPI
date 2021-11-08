using API.Common.AMS;
using API.Common.IO;
using API.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmsSpecRefController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AmsSpecRefController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetSpecRefBinary")]
        public IActionResult GetSpecRefBinary(string p)
        {
            try
            {
                byte[] byteDBName = Convert.FromBase64String(CommonUtil.Base64UrlDecode(p));
                if (ArrayOperate.GetArrayHashString(byteDBName) == Request.Headers["Data-Hash"])
                {
                    string DBName = Encoding.ASCII.GetString(byteDBName);
                    return Ok(GetSpecRefBinary_SQL(DBName));
                }
                else
                {
                    return Ok("Exception: Hash Error");
                }
            }
            catch (Exception ex)
            {
                return Ok("Exception: GetSpecRefBinary()" + ex.Message);
            }
        }

        private byte[] GetSpecRefBinary_SQL(string DBName)
        {
            string connectionString = string.Format(_configuration.GetValue<string>("ConnectionStrings:SiteConnection"), DBName);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;

                DataSet ds = new DataSet();
                sqlCmd.CommandText = "SELECT TOP 1 SpecRef FROM AMS_SpecRefBinary ORDER BY UTC DESC";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmd);
                dataAdapter.Fill(ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0][0];
                    return data;
                }

                return null;
            }
        }

        // POST api/<AmsSpecRefController>
        [HttpPost]
        public IActionResult AmsSpecRef([FromBody] string value)
        {
            Buffer_SpecRef specRefBuffer;

            try
            {
                specRefBuffer = new Buffer_SpecRef(value, Request.Headers["Data-Hash"]);
                if (specRefBuffer.DBName == "")
                {
                    return Ok("Hash Error");
                }
            }
            catch (Exception e)
            {
                return Ok("new Buffer_SpecRef exception: " + e.Message);
            }

            try
            {
                AddSpecRef_SQL(specRefBuffer);
            }
            catch (Exception e)
            {
                return Ok("AddSpecRef_SQL exception: " + e.Message);
            }

            return Ok("Success");
        }

        private void AddSpecRef_SQL(Buffer_SpecRef specRefBuffer)
        {
            string connectionString = string.Format(_configuration.GetValue<string>("ConnectionStrings:SiteConnection"), specRefBuffer.DBName);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;

                sqlCmd.Parameters.Clear();
                sqlCmd.CommandText = "IF NOT EXISTS (SELECT * FROM AMS_SpecRefBinary WHERE UTC=@UTC) INSERT INTO AMS_SpecRefBinary (UTC, SpecRef) VALUES (@UTC, @SpecRef)";
                sqlCmd.Parameters.AddWithValue("@UTC", specRefBuffer.specRefLastWriteUTC);
                sqlCmd.Parameters.Add("@SpecRef", SqlDbType.VarBinary, specRefBuffer.binaryCompressedFile.Length).Value = specRefBuffer.binaryCompressedFile;

                sqlCmd.ExecuteNonQuery();

                for (int i = 0; i < specRefBuffer.chEnList.Count; i++)
                {
                    byte[] curBlock = new byte[512];
                    byte[] compressedBlock = null;
                    if (Convert.ToInt16(specRefBuffer.binaryFile[specRefBuffer.chEnList[i]]) != 0)
                    {
                        Buffer.BlockCopy(specRefBuffer.binaryFile, (specRefBuffer.chEnList[i] + 1) * 512, curBlock, 0, 512);
                        compressedBlock = ArrayOperate.ZipData(curBlock);
                    }

                    sqlCmd.Parameters.Clear();
                    sqlCmd.CommandText = "IF NOT EXISTS (SELECT * FROM AMS_SpecRef_Channel_Binary WHERE UTC=@UTC AND C_ID=@C_ID) " +
                                         "INSERT INTO AMS_SpecRef_Channel_Binary (UTC, C_ID, Header, SpecRef) " +
                                         string.Format("VALUES (@UTC, @C_ID, @Header, {0})", compressedBlock == null ? "NULL" : "@SpecRef");
                    sqlCmd.Parameters.AddWithValue("@UTC", specRefBuffer.specRefLastWriteUTC);
                    sqlCmd.Parameters.AddWithValue("@C_ID", specRefBuffer.chEnList[i] + 1);
                    sqlCmd.Parameters.AddWithValue("@Header", Convert.ToInt16(specRefBuffer.binaryFile[specRefBuffer.chEnList[i]]));
                    if (compressedBlock != null)
                    {
                        sqlCmd.Parameters.Add("@SpecRef", SqlDbType.VarBinary, compressedBlock.Length).Value = compressedBlock;
                    }

                    sqlCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
