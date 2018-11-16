using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using MsgPack.Serialization;

namespace Horker.PSCNTK
{
    public class MsgPackSerializer
    {
        private static Dictionary<string, Tuple<float[], int[]>> ConvertToSerializableObject(DataSourceSet dss)
        {
            var obj = new Dictionary<string, Tuple<float[], int[]>>();

            foreach (var entry in dss.Features)
            {
                var ds = entry.Value;
                obj[entry.Key] = new Tuple<float[], int[]>(ds.Data.ToArray(), ds.Shape.Dimensions);
            }

            return obj;
        }

        private static DataSourceSet ConvertFromSerializableObject(Dictionary<string, Tuple<float[], int[]>> obj)
        {
            var dss = new DataSourceSet();

            foreach (var entry in obj)
                dss.Add(entry.Key, DataSourceFactory.Create(entry.Value.Item1, entry.Value.Item2));

            return dss;
        }

        public static void Serialize(DataSourceSet dss, Stream stream, bool compress = false)
        {
            var serializer = MessagePackSerializer.Get<Dictionary<string, Tuple<float[], int[]>>>();
            var obj = ConvertToSerializableObject(dss);

            if (compress)
            {
                using (var zstream = new DeflateStream(stream, CompressionLevel.Optimal, true))
                {
                    serializer.Pack(zstream, obj);
                }
            }
            else
                serializer.Pack(stream, obj);
        }

        public static void Serialize(DataSourceSet dss, string path, bool compress = false)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Serialize(dss, stream, compress);
            }
        }

        public static DataSourceSet Deserialize(Stream stream, bool decompress = false)
        {
            var serializer = MessagePackSerializer.Get<Dictionary<string, Tuple<float[], int[]>>>();

            if (decompress)
            {
                using (var zstream = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    return ConvertFromSerializableObject(serializer.Unpack(zstream));
                }
            }
            else
                return ConvertFromSerializableObject(serializer.Unpack(stream));
        }

        public static List<DataSourceSet> Deserialize(string path, bool decompress = false)
        {
            var serializer = MessagePackSerializer.Get<DataSourceSet>();

            var result = new List<DataSourceSet>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (decompress)
                {
                    using (var zstream = new DeflateStream(stream, CompressionMode.Decompress))
                    {
                        while (true)
                        {
                            var dss = serializer.Unpack(zstream);
                            if (dss == null)
                                break;
                            result.Add(dss);
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        var dss = serializer.Unpack(stream);
                        if (dss == null)
                            break;
                        result.Add(dss);
                    }
                }
                return result;
            }
        }
    }
}
