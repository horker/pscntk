using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class Serializer
    {
        public static byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static void Serialize(object obj, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                (new BinaryFormatter()).Serialize(stream, obj);
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return (T)(new BinaryFormatter()).Deserialize(stream);
            }
        }

        public static T Deserialize<T>(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (T)(new BinaryFormatter()).Deserialize(stream);
            }
        }
    }
}
