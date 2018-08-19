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
    public class DataSourceSetCTFBuilder
    {
        public static void Write(TextWriter writer, DataSourceSet dataSourceSet)
        {
            var builder = new CTFBuilder(writer, 0, false);

            // Argument check

            var sampleCount = dataSourceSet.Features.First().Value.Shape[-1];
            var maxSeqLength = 1;
            foreach (var entry in dataSourceSet)
            {
                var name = entry.Key;
                var ds = entry.Value;

                if (ds.Shape.Rank < 3)
                    throw new ArgumentException("DataSource shape should contain sequence and batch axes as the last two");

                var count = ds.Shape[-1];
                if (count != sampleCount)
                    throw new ArgumentException("Sample counts of data sources should be equal");

                var seqLength = ds.Shape[-2];
                if (seqLength > maxSeqLength)
                    maxSeqLength = seqLength;
            }

            for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
            {
                for (var seq = 0; seq < maxSeqLength; ++seq)
                {
                    foreach (var entry in dataSourceSet)
                    {
                        var name = entry.Key;
                        var ds = entry.Value;
                        var seqLength = ds.Shape[-2];
                        if (seq >= seqLength)
                            continue;

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

        public static void Write(TextWriter writer, Hashtable sourceSpec)
        {
            var set = new DataSourceSet();

            foreach (DictionaryEntry entry in sourceSpec)
            {
                var name = entry.Key.ToString();

                DataSource<float> ds;
                if (entry.Value is PSObject)
                    ds = (DataSource<float>)(entry.Value as PSObject).BaseObject;
                else
                    ds = (DataSource<float>)entry.Value;

                set.Features.Add(name, ds);
            }

            Write(writer, set);
        }

        public static void Write(string path, DataSourceSet dataSourceSet)
        {
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false)))
                DataSourceSetCTFBuilder.Write(writer, dataSourceSet);
        }

        public static void Write(string path, Hashtable sourceSpec)
        {
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false)))
                DataSourceSetCTFBuilder.Write(writer, sourceSpec);
        }
    }
}
