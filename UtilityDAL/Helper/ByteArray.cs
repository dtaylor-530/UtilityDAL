using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace UtilityDAL
{
    public class ByteArrayHelper
    {
        // Convert an object to a byte array
        public static byte[] ToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public static Object ToObject(byte[] arrBytes)
        {
            using (var memStream = new System.IO.MemoryStream(arrBytes))
            {
                var binForm = new BinaryFormatter();
                //memStream.Write(arrBytes, 0, arrBytes.Length);
                //memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        //public static byte[] SerializeToByteArray(this object obj)
        //{
        //    if (obj == null)
        //    {
        //        return null;
        //    }
        //    var bf = new BinaryFormatter();
        //    using (var ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        //public static T Deserialize<T>(this byte[] byteArray) where T : class
        //{
        //    if (byteArray == null)
        //    {
        //        return null;
        //    }
        //    using (var memStream = new MemoryStream())
        //    {
        //        var binForm = new BinaryFormatter();
        //        memStream.Write(byteArray, 0, byteArray.Length);
        //        memStream.Seek(0, SeekOrigin.Begin);
        //        var obj = (T)binForm.Deserialize(memStream);
        //        return obj;
        //    }
        //}
    }
}