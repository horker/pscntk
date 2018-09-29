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
        public static void Write(TextWriter writer, DataSourceSet dataSourceSet, bool withSequenceAxis)
        {
            var builder = new CTFBuilder(writer, 0, false);

            // Argument check

            var sampleCount = dataSourceSet.Features.First().Value.Shape[-1];
            var maxSeqLength = 1;
            foreach (var entry in dataSourceSet)
            {
                var name = entry.Key;
                var ds = entry.Value;

                if (withSequenceAxis && ds.Shape.Rank < 3)
                    throw new ArgumentException("DataSource shape should have sequence and batch axes as the last two");

                if (!withSequenceAxis && ds.Shape.Rank < 2)
                    throw new ArgumentException("DataSource shape should have a batch axis");

                var count = ds.Shape[-1];
                if (count != sampleCount)
                    throw new ArgumentException("Sample counts of data sources should be equal");

                if (withSequenceAxis)
                {
                    var seqLength = ds.Shape[-2];
                    if (seqLength > maxSeqLength)
                        maxSeqLength = seqLength;
                }
            }

            for (var sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
            {
                for (var seq = 0; seq < maxSeqLength; ++seq)
                {
                    foreach (var entry in dataSourceSet)
                    {
                        var name = entry.Key;
                        var ds = entry.Value;

                        int seqLength;
                        int dim;
                        if (withSequenceAxis)
                        {
                            seqLength = ds.Shape[-2];
                            if (seq >= seqLength)
                                continue;

                            dim = ds.Shape.GetSize(ds.Shape.Rank - 3);
                        }
                        else
                        {
                            seqLength = 1;
                            dim = ds.Shape.GetSize(ds.Shape.Rank - 2);
                        }

                        int index = sampleIndex * dim * seqLength + seq * dim;
                        builder.AddDenseSample(name, new ListSlice<float>(ds.Data, index, dim));
                    }
                    builder.NextLine();
                }
                builder.NextSequence();
            }

            builder.Finish();
        }

        public static void Write(string path, DataSourceSet dataSourceSet, bool hasSequenceAxis)
        {
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false)))
                Write(writer, dataSourceSet, hasSequenceAxis);
        }
    }
}
