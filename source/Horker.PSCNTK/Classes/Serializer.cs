using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class Serializer
    {
        public static byte[] Serialize(object obj, bool compress)
        {
            using (var stream = new MemoryStream())
            {
                if (compress)
                    using (var zstream = new DeflateStream(stream, CompressionLevel.Optimal, true))
                        (new BinaryFormatter()).Serialize(zstream, obj);
                else
                    (new BinaryFormatter()).Serialize(stream, obj);

                return stream.ToArray();
            }
        }

        public static void Serialize(object obj, string path, bool compress)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                if (compress)
                    using (var zstream = new DeflateStream(stream, CompressionLevel.Optimal, true))
                        (new BinaryFormatter()).Serialize(zstream, obj);
                else
                    (new BinaryFormatter()).Serialize(stream, obj);
            }
        }

        public static T Deserialize<T>(byte[] data, bool decompress)
        {
            using (var stream = new MemoryStream(data))
            {
                if (decompress)
                    using (var zstream = new DeflateStream(stream, CompressionMode.Decompress))
                        return (T)(new BinaryFormatter()).Deserialize(zstream);
                else
                    return (T)(new BinaryFormatter()).Deserialize(stream);
            }
        }

        public static T Deserialize<T>(string path, bool decompress)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (decompress)
                    using (var zstream = new DeflateStream(stream, CompressionMode.Decompress))
                        return (T)(new BinaryFormatter()).Deserialize(zstream);
                else
                    return (T)(new BinaryFormatter()).Deserialize(stream);
            }
        }
    }
}
