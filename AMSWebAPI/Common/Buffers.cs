using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSWebAPI.Common
{
    public class AMSSysInfo
    {
        public static int CH_COUNT = 192;
        public static int AUX_COUNT = 32;
    }

    public class RMSBuffer1
    {
        // 24: chEnList; 4: auxEnList; 32: reserved
        const int HEADER_SIZE = 24 + 4 + 32;
        int _chEnCount = 0;
        int _auxEnCount = 0;
        int _blockCountPerCall = 0;
        int _totalBlock = 0;
        int _unsavedBlocks = 0;

        public int BlockSize
        {
            get
            {
                return 8 + _chEnCount * 2 + _auxEnCount * 2;
            }
        }
        public int BlockCountPerCall
        {
            get { return _blockCountPerCall; }
        }

        public int UnsavedBlockCount
        {
            get { return _unsavedBlocks; }
        }

        public byte[] CreateRMSBuffer(int chEnCount, int auxEnCount, int blockCountPerCall)
        {
            _chEnCount = chEnCount;
            _auxEnCount = auxEnCount;
            _blockCountPerCall = blockCountPerCall;

            byte[] buffer = new byte[HEADER_SIZE + BlockSize * blockCountPerCall];
            return buffer;
        }

        public void SetRMSHeader(byte[] buffer, string DBName, List<int> chEnList, List<int> auxEnList)
        {
            byte[] byteDBName = Encoding.ASCII.GetBytes(DBName);
            byte[] byteChEnList = ArrayOperate.ConvertEnableListToArray(chEnList, AMSSysInfo.CH_COUNT / 8);
            byte[] byteAuxEnList = ArrayOperate.ConvertEnableListToArray(auxEnList, AMSSysInfo.AUX_COUNT / 8);

            int curPos = 0;
            Array.Copy(byteDBName, 0, buffer, curPos, byteDBName.Length);

            curPos = 16;
            Array.Copy(byteChEnList, 0, buffer, curPos, byteChEnList.Length);

            curPos += byteChEnList.Length;
            Array.Copy(byteAuxEnList, 0, buffer, curPos, byteAuxEnList.Length);
        }

        public void SetRMSBlock(byte[] buffer, DateTime UTC, List<short> chValList, List<short> auxValList, int blockIndex)
        {
            byte[] byteUTC = BitConverter.GetBytes(UTC.ToBinary());
            byte[] byteChValList = chValList.SelectMany(BitConverter.GetBytes).ToArray();
            byte[] byteAuxValList = auxValList.SelectMany(BitConverter.GetBytes).ToArray();

            int curPos = HEADER_SIZE + BlockSize * blockIndex;
            Array.Copy(byteUTC, 0, buffer, curPos, 8);

            curPos += 8;
            Array.Copy(byteChValList, 0, buffer, curPos, byteChValList.Length);

            curPos += byteChValList.Length;
            Array.Copy(byteAuxValList, 0, buffer, curPos, byteAuxValList.Length);

            _unsavedBlocks++;
            _totalBlock++;
        }

        public void ResetUnsavedBlocks()
        {
            _unsavedBlocks = 0;
        }

        static public void GetBufferInfo(byte[] buffer, ref string DBName, ref List<short> chEnList, ref List<short> auxEnList, ref int blockCount)
        {
            byte[] byteDBName = new byte[16]; Encoding.ASCII.GetBytes(DBName);
            byte[] byteChEnList = new byte[192 / 8];
            byte[] byteAuxEnList = new byte[32 / 8];

            int curPos = 0;
            System.Array.Copy(buffer, curPos, byteDBName, 0, byteDBName.Length);
            curPos = byteDBName.Length;
            System.Array.Copy(buffer, curPos, byteChEnList, 0, byteChEnList.Length);
            curPos += byteChEnList.Length;
            System.Array.Copy(buffer, curPos, byteAuxEnList, 0, byteAuxEnList.Length);

            DBName = BitConverter.ToString(byteDBName);
            chEnList = ArrayOperate.ConvertArrayToEnableList(byteChEnList);
            auxEnList = ArrayOperate.ConvertArrayToEnableList(byteAuxEnList);
            blockCount = (buffer.Length - HEADER_SIZE) / (8 + chEnList.Count * 2 + auxEnList.Count * 2);
        }

        static public void GetRMSRecordByIndex(byte[] buffer, Int32 blockIndex, List<short> chEnList, List<short> auxEnList, ref DateTime UTC, ref List<short> chValList, ref List<short> auxValList)
        {
            byte[] byteUTC = new byte[8];
            byte[] byteChValList = new byte[chEnList.Count * 2];
            byte[] byteAuxValList = new byte[auxEnList.Count * 2];

            int curPos = HEADER_SIZE + (byteUTC.Length + byteChValList.Length + byteAuxValList.Length) * blockIndex;
            System.Array.Copy(buffer, curPos, byteUTC, 0, byteUTC.Length);

            curPos += byteUTC.Length;
            System.Array.Copy(buffer, curPos, byteChValList, 0, byteChValList.Length);

            curPos += byteChValList.Length;
            System.Array.Copy(buffer, curPos, byteAuxValList, 0, byteAuxValList.Length);

            UTC = DateTime.FromBinary(BitConverter.ToInt64(byteUTC));
            chValList = ArrayOperate.ToListOf<short>(byteChValList, BitConverter.ToInt16);
            auxValList = ArrayOperate.ToListOf<short>(byteAuxValList, BitConverter.ToInt16);
        }
    }
}
