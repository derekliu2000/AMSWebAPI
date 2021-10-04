using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AMSWebAPI.Models;

namespace AMSWebAPI.Common
{
    public class RMSRecord
    {
        DateTime UTC { get; set; }
        public AmsDataRms1 rms1 { get; set; }
        public AmsDataRms2 rms2 { get; set; }
        public AmsDataRms3 rms3 { get; set; }
        public AmsDataRms4 rms4 { get; set; }
        public AmsDataRms5 rms5 { get; set; }
        public AmsDataRms6 rms6 { get; set; }
        public AmsDataRms7 rms7 { get; set; }
        public AmsDataRms8 rms8 { get; set; }
        public RMSRecord(DateTime Utc)
        {
            UTC = Utc;
            rms1 = new AmsDataRms1() { Utc = DateTime.MinValue };
            rms2 = new AmsDataRms2() { Utc = DateTime.MinValue };
            rms3 = new AmsDataRms3() { Utc = DateTime.MinValue };
            rms4 = new AmsDataRms4() { Utc = DateTime.MinValue };
            rms5 = new AmsDataRms5() { Utc = DateTime.MinValue };
            rms6 = new AmsDataRms6() { Utc = DateTime.MinValue };
            rms7 = new AmsDataRms7() { Utc = DateTime.MinValue };
            rms8 = new AmsDataRms8() { Utc = DateTime.MinValue };
        }
    }
    public class AMSOperator
    {
        public static int CH_COUNT = 192;
        public static int AUX_COUNT = 32;
        public static string[] TABLE_BAR_FLAGS = { "AMS_DataBarFlags_1", "AMS_DataBarFlags_2", "AMS_DataBarFlags_3", "AMS_DataBarFlags_4", "AMS_DataBarFlags_5", "AMS_DataBarFlags_6", "AMS_DataBarFlags_7", "AMS_DataBarFlags_8" };
        public static string[] TABLE_RMS = { "AMS_DataRMS_1", "AMS_DataRMS_2", "AMS_DataRMS_3", "AMS_DataRMS_4", "AMS_DataRMS_5", "AMS_DataRMS_6", "AMS_DataRMS_7", "AMS_DataRMS_8" };
        public static string[] TABLE_RMS_FLAGS = { "AMS_DataRMSFlags_1", "AMS_DataRMSFlags_2", "AMS_DataRMSFlags_3", "AMS_DataRMSFlags_4", "AMS_DataRMSFlags_5", "AMS_DataRMSFlags_6", "AMS_DataRMSFlags_7", "AMS_DataRMSFlags_8" };
        public static string[] TABLE_DTA = { "AMS_DataDTA_1", "AMS_DataDTA_2", "AMS_DataDTA_3", "AMS_DataDTA_4", "AMS_DataDTA_5", "AMS_DataDTA_6", "AMS_DataDTA_7", "AMS_DataDTA_8" };
        public static string[] TABLE_JRN = { "AMS_Journal_1", "AMS_Journal_2", "AMS_Journal_3", "AMS_Journal_4", "AMS_Journal_5", "AMS_Journal_6", "AMS_Journal_7", "AMS_Journal_8" };

        public static RMSRecord FillRMSObjects(DateTime UTC, List<short> chEnList, List<short> chValList)
        {
            RMSRecord rms = new RMSRecord(UTC);

            for (int i = 0; i < chEnList.Count; i++)
            {
                FillMember(rms, UTC, chEnList[i], chValList[i]);
            }

            return rms;
        }

        private static void FillMember(RMSRecord rms, DateTime UTC, int chIdx, short chVal)
        {
            try
            {
                object[] rmsTbl = { rms.rms1, rms.rms2, rms.rms3, rms.rms4, rms.rms5, rms.rms6, rms.rms7, rms.rms8 };

                Type type = rmsTbl[chIdx / 24].GetType();
                type.GetProperty($"C{chIdx + 1}").SetValue(rmsTbl[chIdx / 24], chVal);
                DateTime recordUTC = Convert.ToDateTime(type.GetProperty("Utc").GetValue(rmsTbl[chIdx / 24]));
                if (recordUTC == DateTime.MinValue)
                {
                    type.GetProperty("Utc").SetValue(rmsTbl[chIdx / 24], UTC);
                }
            }
            catch (Exception e)
            {
                string strErr = e.Message;
            }
        }

        public static AmsDataAux FillAuxObjects(DateTime UTC, List<short> auxEnList, List<short> auxValList)
        {
            AmsDataAux aux = new AmsDataAux() { Utc = UTC };

            for (int i = 0; i < auxEnList.Count; i++)
            {
                FillMember(aux, auxEnList[i], auxValList[i]);
            }

            return aux;
        }

        private static void FillMember(AmsDataAux aux, short auxIdx, short auxVal)
        {
            try
            {
                Type type = aux.GetType();
                type.GetProperty($"A{auxIdx + 1}").SetValue(aux, auxVal);
            }
            catch (Exception e)
            {
                string strErr = e.Message;
            }
        }

        public static List<RMSRecord> GetRMSRecordFromArray(byte[] byteData)
        {
            List<RMSRecord> rmsRecordList = new List<RMSRecord>();

            byte[] byteChEnList = new byte[192 / 8];
            System.Array.Copy(byteData, 0, byteChEnList, 0, byteChEnList.Length);
            List<short> chEnList = ArrayOperate.ConvertArrayToEnableList(byteChEnList);
            Int32 chEnCount = chEnList.Count();

            // one block size = 8 (UTC) + chEnListCount * 2
            // block count = 
            Int32 blockCount = (byteData.Length - byteChEnList.Length) / (8 + chEnCount * 2);
            for (int i = 0; i < blockCount; i++)
            {
                byte[] byteUTC = new byte[8];
                byte[] byteChValList = new byte[chEnCount * 2];

                System.Array.Copy(byteData, byteChEnList.Length + (8 + chEnCount * 2) * i, byteUTC, 0, byteUTC.Length);
                System.Array.Copy(byteData, byteChEnList.Length + (8 + chEnCount * 2) * i + 8, byteChValList, 0, byteChValList.Length);

                DateTime UTC = DateTime.FromBinary(BitConverter.ToInt64(byteUTC));
                List<short> chValList = ArrayOperate.ToListOf<short>(byteChValList, BitConverter.ToInt16);

                RMSRecord rmsRecord = AMSOperator.FillRMSObjects(UTC, chEnList, chValList);

                rmsRecordList.Add(rmsRecord);
            }

            return rmsRecordList;
        }

        public static int GetChInfo(byte[] byteData, ref List<short> chEnList, ref List<short> auxEnList)
        {
            byte[] byteChEnList = new byte[192 / 8];
            byte[] byteAuxEnList = new byte[32 / 8];
            int curPos = 0;
            System.Array.Copy(byteData, curPos, byteChEnList, 0, byteChEnList.Length);
            curPos += byteChEnList.Length;
            System.Array.Copy(byteData, curPos, byteAuxEnList, 0, byteAuxEnList.Length);

            chEnList = ArrayOperate.ConvertArrayToEnableList(byteChEnList);
            auxEnList = ArrayOperate.ConvertArrayToEnableList(byteAuxEnList);

            // one block size = 8 (UTC) + chEnListCount * 2 + auxEnListCount * 2            
            Int32 blockCount = (byteData.Length - byteChEnList.Length) / (8 + chEnList.Count * 2);

            return blockCount;
        }
    }
}
