using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class DataSourceCTFBuilder
    {
        public static void Write(TextWriter writer, DataSource<float>[] dataSources, string[] names)
        {
            var builder = new CTFBuilder(writer, false);

            // Argument check

            if (dataSources.Length != names.Length)
                throw new ArgumentException("Number of dataSources and names should be equal");

            var sampleCount = dataSources[0].Shape[dataSources[0].Shape.Rank - 1];
            var maxSeqLength = 1;
            foreach (var ds in dataSources)
            {
                if (ds.Shape.Rank < 3)
                    throw new ArgumentException("DataSource shape should contain sequence and batch axes as the last two");

                var count = ds.Shape[ds.Shape.Rank - 1];
                if (count != sampleCount)
                    throw new ArgumentException("sample counts of data sources should be equal");

                var seqLength = ds.Shape[ds.Shape.Rank - 2];
                if (seqLength > maxSeqLength)
                    maxSeqLength = seqLength;
            }

            for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
            {
                for (var seq = 0; seq < maxSeqLength; ++seq)
                {
                    for (var dataIndex = 0; dataIndex < dataSources.Length; ++dataIndex)
                    {
                        var ds = dataSources[dataIndex];
                        var seqLength = ds.Shape[ds.Shape.Rank - 2];
                        if (seq >= seqLength)
                            continue;

                        var name = names[dataIndex];
                        var dim = ds.Shape.GetSize(ds.Shape.Rank - 3);

                        int index = sampleIndex * dim * seqLength + seq * dim;
                        builder.AddDenseSample(name, new ArraySegment<float>(ds.Data, index, dim));
                    }
                    builder.NextLine();
                }
                builder.NextSequence();
            }

            builder.Finish();
        }

        public static void Write(string file, Hashtable sourceSpec)
        {
            var names = new List<string>();
            var dataSources = new List<DataSource<float>>();

            foreach (DictionaryEntry entry in sourceSpec)
            {
                names.Add(entry.Key.ToString());

                DataSource<float> ds;
                if (entry.Value is PSObject)
                    ds = (DataSource<float>)(entry.Value as PSObject).BaseObject;
                else
                    ds = (DataSource<float>)entry.Value;

                dataSources.Add(ds);
            }

            using (var writer = new StreamWriter(file, false, new UTF8Encoding(false)))
            {
                DataSourceCTFBuilder.Write(writer, dataSources.ToArray(), names.ToArray());
            }
        }
    }
}
