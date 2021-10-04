using AMSWebAPI.Common;
using AMSWebAPI.Models;
using API.Common.AMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebAPI.Controllers
{
    class SQLParts
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
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AmsRMSController : ControllerBase
    {
        private readonly AMS_SiteContext _context;

        public AmsRMSController(AMS_SiteContext dbContext)
        {
            _context = dbContext;
        }

        // GET: api/<AmsRMSController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("RMSMaxUTC")]
        //public DateTime GetRMSMaxUTC(string p)
        public DateTime GetRMSMaxUTC(string db, int tblIdx, long t)
        {
            //MaxUTCBuffer maxUTCBuffer = new MaxUTCBuffer(p);
            //string db = maxUTCBuffer.DBName;
            string connectionString = $"Server=10.32.66.230\\sqlexpress2019;Database={db};UID=sa;Password=Mistras1;MultipleActiveResultSets=true";
            try
            {
                DateTime UTC = DateTime.MinValue;

                //string connectionString = $"Server=10.32.66.230\\sqlexpress2019;Database={db};UID=sa;Password=Mistras1;MultipleActiveResultSets=true";
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();

                    //SqlCommand sqlCmd = new SqlCommand($"SELECT MAX(UTC) FROM {AMSOperator.TABLE_RMS[maxUTCBuffer.tblIdx]}", sqlConn);
                    SqlCommand sqlCmd = new SqlCommand($"SELECT MAX(UTC) FROM {AMSOperator.TABLE_RMS[tblIdx]}", sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr[0] != DBNull.Value)
                                UTC = dr.GetDateTime(0);
                        }
                    }
                    dr.Close();
                }

                return UTC;
            }
            catch (Exception ex)
            {
                throw new Exception("Get RMS Max UTC failed", ex);
            }
        }

        // GET api/<AmsRMSController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AmsRMSController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("AddRMSBlocks")]
        public async Task<IActionResult> AddRMSBlocks([FromBody] string value)
        {
            byte[] zippedData = System.Convert.FromBase64String(value);
            byte[] OrgData = ArrayOperate.UnZipData(zippedData);

            List<short> chIdList = null;
            List<short> auxIdList = null;
            Int32 blockCount = 0;
            string DBName = "";
            RMSBuffer.GetBufferInfo(OrgData, ref DBName, ref chIdList, ref auxIdList, ref blockCount);

            string connectionString = $"Server=10.32.66.230\\sqlexpress2019;Database={DBName};UID=sa;Password=Mistras1;MultipleActiveResultSets=true";
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                SqlCommand sqlCmd = sqlConn.CreateCommand();

                //sqlCmd.CommandText = $"USE [{Request.Headers["DBName"]}]";
                //sqlCmd.ExecuteNonQuery();

                //SqlTransaction transaction = sqlConn.BeginTransaction("AddRMSTrans");
                ////Boolean timestamp = false;
                //sqlCmd.Connection = sqlConn;
                //sqlCmd.Transaction = transaction;

                try
                {
                    

                    for (int i = 0; i < blockCount; i++)
                    {
                        DateTime UTC = DateTime.MinValue;
                        List<short> chValList = null;
                        List<short> auxValList = null;
                        RMSBuffer.GetRMSRecordByIndex(OrgData, i, chIdList, auxIdList, ref UTC, ref chValList, ref auxValList);
                        if (UTC != DateTime.MinValue)
                        {
                            for (int tblIndex = 0; tblIndex < AMSOperator.TABLE_RMS.Length; tblIndex++)
                            {
                                int minChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_RMS.Length * tblIndex;
                                int maxChId = AMSOperator.CH_COUNT / AMSOperator.TABLE_RMS.Length * (tblIndex + 1) - 1;

                                if (chIdList.Max() < minChId)
                                    break;

                                SQLParts sqlParts = ConstructSQLParts(chIdList, null, "RMS", tblIndex);
                                if (sqlParts.ContainValues)
                                {
                                    SqlTransaction transaction = sqlConn.BeginTransaction("AddRMSTrans");
                                    //Boolean timestamp = false;
                                    sqlCmd.Connection = sqlConn;
                                    sqlCmd.Transaction = transaction;

                                    // Add records to RMS tables
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM {AMSOperator.TABLE_RMS[tblIndex]} WHERE UTC=@UTC) " +
                                                         $"INSERT INTO {AMSOperator.TABLE_RMS[tblIndex]} ({sqlParts.ColumnNames}) VALUES ({sqlParts.Values})";
                                    for (int idx = 0; idx < chIdList.Count; idx++)
                                    {
                                        if (chIdList[idx] >= minChId && chIdList[idx] <= maxChId)
                                        {
                                            sqlCmd.Parameters.AddWithValue($"@C{chIdList[idx] + 1}", chValList[idx] & Convert.ToInt32("11111111111111", 2));
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
                                    for (int idx = 0; idx < chIdList.Count; idx++)
                                    {
                                        if (chIdList[idx] >= minChId && chIdList[idx] <= maxChId)
                                        {
                                            sqlCmd.Parameters.AddWithValue($"@C{chIdList[idx] + 1}", Math.Abs(chValList[idx] >> 14));
                                        }
                                    }
                                    if (sqlCmd.Parameters.Count > 0)
                                    {
                                        sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                                        sqlCmd.ExecuteNonQuery();

                                        // Add timestamp to AMS_DataRMSTimestamp if needed
                                        //if (timestamp == false)
                                        //{
                                        //    sqlCmd.Parameters.Clear();
                                        //    sqlCmd.CommandText = $"IF NOT EXISTS (SELECT UTC FROM AMS_DataRMSTimestamp WHERE UTC=@UTC) " +
                                        //                         $"INSERT INTO AMS_DataRMSTimestamp (UTC) VALUES (@UTC)";
                                        //    sqlCmd.Parameters.AddWithValue("@UTC", UTC);
                                        //    sqlCmd.ExecuteNonQuery();

                                        //    timestamp = true;
                                        //}
                                    }

                                    transaction.Commit();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    try
                    {
                       // transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("Failed to rollback.", ex2);
                    }
                }
            }

            return NoContent();
        }

        private SQLParts ConstructSQLParts(List<short> list1, List<int> list2, string type, int tblIndex)
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

        /*[HttpPost("AddRMSWholeFile")]
        public async Task<IActionResult> AddRMSWholeFile([FromBody] string value)
        {
            _context.SetDatabase(Request);
            List<int> bufferSizeList = _context.GetBufferSizes(Request);

            byte[] zippedData = System.Convert.FromBase64String(value);
            byte[] OrgData = Utility.UnZipData(zippedData);

            List<RMSRecord> rmsRecordList = AMSOperator.GetRMSRecordFromArray(OrgData);

            for (int i = 0; i < rmsRecordList.Count; i++)
            {
                RMSRecord rmsRecord = rmsRecordList[i];
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    if (rmsRecord.rms1.Utc != DateTime.MinValue && !_context.AmsDataRms1s.Any(i => i.Utc == rmsRecord.rms1.Utc))
                    {
                        _context.AmsDataRms1s.Add(rmsRecord.rms1);
                    }

                    if (rmsRecord.rms2.Utc != DateTime.MinValue && !_context.AmsDataRms2s.Any(i => i.Utc == rmsRecord.rms2.Utc))
                    {
                        _context.AmsDataRms2s.Add(rmsRecord.rms2);
                    }

                    if (rmsRecord.rms3.Utc != DateTime.MinValue && !_context.AmsDataRms3s.Any(i => i.Utc == rmsRecord.rms3.Utc))
                    {
                        _context.AmsDataRms3s.Add(rmsRecord.rms3);
                    }

                    if (rmsRecord.rms4.Utc != DateTime.MinValue && !_context.AmsDataRms4s.Any(i => i.Utc == rmsRecord.rms4.Utc))
                    {
                        _context.AmsDataRms4s.Add(rmsRecord.rms4);
                    }

                    if (rmsRecord.rms5.Utc != DateTime.MinValue && !_context.AmsDataRms5s.Any(i => i.Utc == rmsRecord.rms5.Utc))
                    {
                        _context.AmsDataRms5s.Add(rmsRecord.rms5);
                    }

                    if (rmsRecord.rms6.Utc != DateTime.MinValue && !_context.AmsDataRms6s.Any(i => i.Utc == rmsRecord.rms6.Utc))
                    {
                        _context.AmsDataRms6s.Add(rmsRecord.rms6);
                    }

                    if (rmsRecord.rms7.Utc != DateTime.MinValue && !_context.AmsDataRms7s.Any(i => i.Utc == rmsRecord.rms7.Utc))
                    {
                        _context.AmsDataRms7s.Add(rmsRecord.rms7);
                    }

                    if (rmsRecord.rms8.Utc != DateTime.MinValue && !_context.AmsDataRms8s.Any(i => i.Utc == rmsRecord.rms8.Utc))
                    {
                        _context.AmsDataRms8s.Add(rmsRecord.rms8);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {

                }
            }

            return NoContent();
        }*/

        [HttpPost("AddRMS")]
        public async Task<IActionResult> AddRMS([FromBody] string value)
        {
            _context.SetDatabase(Request);
            List<int> bufferSizeList = _context.GetBufferSizes(Request);

            byte[] zippedData = System.Convert.FromBase64String(value);
            byte[] OrgData = ArrayOperate.UnZipData(zippedData);
            if (bufferSizeList.Count == 3 && OrgData.Length == bufferSizeList.Sum())
            {
                byte[] byteUTC = new byte[bufferSizeList[0]];
                byte[] byteChEnList = new byte[bufferSizeList[1]];
                byte[] byteChValList = new byte[bufferSizeList[2]];

                System.Array.Copy(OrgData, 0, byteUTC, 0, bufferSizeList[0]);
                System.Array.Copy(OrgData, bufferSizeList[0], byteChEnList, 0, bufferSizeList[1]);
                System.Array.Copy(OrgData, bufferSizeList[0] + bufferSizeList[1], byteChValList, 0, bufferSizeList[2]);

                DateTime UTC = DateTime.FromBinary(BitConverter.ToInt64(byteUTC));
                List<short> chEnList = ArrayOperate.ConvertArrayToEnableList(byteChEnList);
                List<short> chValList = ArrayOperate.ToListOf<short>(byteChValList, BitConverter.ToInt16);
                RMSRecord rmsRecord = AMSOperator.FillRMSObjects(UTC, chEnList, chValList);

                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    if (rmsRecord.rms1.Utc != DateTime.MinValue && !_context.AmsDataRms1s.Any(i => i.Utc == rmsRecord.rms1.Utc))
                    {
                        _context.AmsDataRms1s.Add(rmsRecord.rms1);
                    }

                    if (rmsRecord.rms2.Utc != DateTime.MinValue && !_context.AmsDataRms2s.Any(i => i.Utc == rmsRecord.rms2.Utc))
                    {
                        _context.AmsDataRms2s.Add(rmsRecord.rms2);
                    }

                    if (rmsRecord.rms3.Utc != DateTime.MinValue && !_context.AmsDataRms3s.Any(i => i.Utc == rmsRecord.rms3.Utc))
                    {
                        _context.AmsDataRms3s.Add(rmsRecord.rms3);
                    }

                    if (rmsRecord.rms4.Utc != DateTime.MinValue && !_context.AmsDataRms4s.Any(i => i.Utc == rmsRecord.rms4.Utc))
                    {
                        _context.AmsDataRms4s.Add(rmsRecord.rms4);
                    }

                    if (rmsRecord.rms5.Utc != DateTime.MinValue && !_context.AmsDataRms5s.Any(i => i.Utc == rmsRecord.rms5.Utc))
                    {
                        _context.AmsDataRms5s.Add(rmsRecord.rms5);
                    }

                    if (rmsRecord.rms6.Utc != DateTime.MinValue && !_context.AmsDataRms6s.Any(i => i.Utc == rmsRecord.rms6.Utc))
                    {
                        _context.AmsDataRms6s.Add(rmsRecord.rms6);
                    }

                    if (rmsRecord.rms7.Utc != DateTime.MinValue && !_context.AmsDataRms7s.Any(i => i.Utc == rmsRecord.rms7.Utc))
                    {
                        _context.AmsDataRms7s.Add(rmsRecord.rms7);
                    }

                    if (rmsRecord.rms8.Utc != DateTime.MinValue && !_context.AmsDataRms8s.Any(i => i.Utc == rmsRecord.rms8.Utc))
                    {
                        _context.AmsDataRms8s.Add(rmsRecord.rms8);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {

                }
            }

            return NoContent();
        }

        [HttpPost("AddAux")]
        public async Task<IActionResult> AddAux([FromBody] string value)
        {
            _context.SetDatabase(Request);
            List<int> bufferSizeList = _context.GetBufferSizes(Request);

            byte[] zippedData = System.Convert.FromBase64String(value);
            byte[] OrgData = ArrayOperate.UnZipData(zippedData);
            if (bufferSizeList.Count == 3 && OrgData.Length == bufferSizeList.Sum())
            {
                byte[] byteUTC = new byte[bufferSizeList[0]];
                byte[] byteAuxEnList = new byte[bufferSizeList[1]];
                byte[] byteAuxValList = new byte[bufferSizeList[2]];

                System.Array.Copy(OrgData, 0, byteUTC, 0, bufferSizeList[0]);
                System.Array.Copy(OrgData, bufferSizeList[0], byteAuxEnList, 0, bufferSizeList[1]);
                System.Array.Copy(OrgData, bufferSizeList[0] + bufferSizeList[1], byteAuxValList, 0, bufferSizeList[2]);

                DateTime UTC = DateTime.FromBinary(BitConverter.ToInt64(byteUTC));
                List<short> auxEnList = ArrayOperate.ConvertArrayToEnableList(byteAuxEnList);
                List<short> auxValList = ArrayOperate.ToListOf<short>(byteAuxValList, BitConverter.ToInt16);
                AmsDataAux auxRecord = AMSOperator.FillAuxObjects(UTC, auxEnList, auxValList);
                if (!_context.AmsDataAuxes.Any(i => i.Utc == auxRecord.Utc))
                {
                    _context.AmsDataAuxes.Add(auxRecord);
                    await _context.SaveChangesAsync();
                }
            }

            return NoContent();
        }

        // PUT api/<AmsRMSController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
