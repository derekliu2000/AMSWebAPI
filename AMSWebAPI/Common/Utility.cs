using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

namespace AMSWebAPI.Common
{
    //public class ArrayOperate
    //{
    //    public static byte[] ZipData(byte[] data)
    //    {
    //        using (var outStream = new MemoryStream())
    //        {
    //            using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
    //            {
    //                using (var mStream = new MemoryStream(data))
    //                {
    //                    mStream.CopyTo(tinyStream);
    //                }
    //            }
    //            return outStream.ToArray();
    //        }
    //    }

    //    public static byte[] UnZipData(byte[] compressed)
    //    {
    //        using (var inStream = new MemoryStream(compressed))
    //        {
    //            using (var bigStream = new GZipStream(inStream, CompressionMode.Decompress))
    //            {
    //                using (var bigStreamOut = new MemoryStream())
    //                {
    //                    bigStream.CopyTo(bigStreamOut);
    //                    return bigStreamOut.ToArray();
    //                }
    //            }
    //        }
    //    }

    //    public static List<T> ToListOf<T>(byte[] array, Func<byte[], int, T> bitConverter)
    //    {
    //        var size = Marshal.SizeOf(typeof(T));
    //        return Enumerable.Range(0, array.Length / size)
    //                         .Select(i => bitConverter(array, i * size))
    //                         .ToList();
    //    }

    //    public static List<short> ConvertArrayToEnableList(byte[] byteEnList)
    //    {
    //        byte[] pos = { 128, 64, 32, 16, 8, 4, 2, 1 };
    //        List<short> enableList = new List<short>();

    //        for (int i = 0; i < byteEnList.Length; i++)
    //        {
    //            for (int b = 0; b < 8; b++)
    //            {
    //                if ((byteEnList[i] & pos[b]) == pos[b])
    //                    enableList.Add((short)(i * 8 + b));
    //            }
    //        }

    //        return enableList;
    //    }

    //    public static byte[] ConvertEnableListToArray(List<int> enableList, int size)
    //    {
    //        byte[] pos = { 128, 64, 32, 16, 8, 4, 2, 1 };
    //        byte[] byteEnList = new byte[size];
    //        Array.Clear(byteEnList, 0, byteEnList.Length);

    //        for (int i = 0; i < enableList.Count; i++)
    //        {
    //            int bitPos = enableList[i] % 8;
    //            int bufferIdx = enableList[i] / 8;
    //            byteEnList[bufferIdx] |= pos[bitPos];
    //        }

    //        return byteEnList;
    //    }
    //}
}
