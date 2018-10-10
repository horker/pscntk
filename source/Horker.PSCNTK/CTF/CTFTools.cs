﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class CTFTools
    {
        public class CTFLineInfo
        {
            public int LineCount;
            public int SequenceCount;
            public string SequenceId;
            public string Line;
        }

        public class CTFSample
        {
            public int LineCount;
            public int SequenceCount;
            public string SequenceId;
            public DataSourceSet DataSet;
            public IList<string> Comments;
        }

        public static IEnumerable<CTFLineInfo> GetLineInfoReader(TextReader reader)
        {
            int lineCount = 0;
            int sequenceCount = 0;

            string line;
            string seqId = null;

            while ((line = reader.ReadLine()) != null)
            {
                ++lineCount;

                var s = line.Split(new char[] { '|' });
                var n = s[0].Trim();
                if (n != seqId)
                    ++sequenceCount;

                seqId = n;

                yield return new CTFLineInfo()
                {
                    LineCount = lineCount,
                    SequenceCount = sequenceCount,
                    SequenceId = seqId,
                    Line = line
                };
            }
        }

        public static IEnumerable<CTFSample> GetSampleReader(TextReader reader)
        {
            int lineCount = 0;
            int sequenceCount = 0;
            int seqStartLineCount = 1;

            string line;
            string seqId = null;

            var splitLines = new List<string[]>();

            while ((line = reader.ReadLine()) != null)
            {
                ++lineCount;

                var splitLine = line.Split(new char[] { '|' });
                var n = splitLine[0].Trim();

                if (seqId == null || n == seqId)
                {
                    splitLines.Add(splitLine);
                    seqId = n;

                    if (reader.Peek() != -1)
                        continue;
                }

                ++sequenceCount;

                var seqDim = splitLines.Count;

                var dss = new DataSourceSet();
                var comments = new List<string>();

                for (var i = 0; i < splitLines.Count; ++i)
                {
                    var columns = splitLines[i];
                    for (var j = 1; j < columns.Length; ++j)
                    {
                        var feature = columns[j].Trim();
                        if (feature[0] == '#')
                        {
                            int skip;
                            for (skip = 1; skip < feature.Length && Char.IsWhiteSpace(feature[skip]); ++skip)
                                ;

                            comments.Add(feature.Substring(skip));
                            continue;
                        }

                        var items = feature.Split();
                        if (items.Length < 2)
                            throw new InvalidDataException(string.Format("line {0}: Invalid feature", lineCount));

                        var featureDim = items.Length - 1;
                        var name = items[0];

                        DataSourceBase<float, float[]> ds;

                        float[] data;
                        if (dss.Features.ContainsKey(name))
                        {
                            ds = (DataSourceBase<float, float[]>)dss.Features[name];
                            data = ds.TypedData;
                        }
                        else
                        {
                            data = new float[featureDim * seqDim];
                            ds = DataSourceFactory.Create(data, new int[] { featureDim, seqDim, 1 });
                            dss.Add(name, ds);
                        }

                        var baseIndex = ds.Shape.GetSequentialIndex(new int[] { 0, i, 0 });
                        for (var k = 0; k < featureDim; ++k)
                            data[baseIndex + k] = Converter.ToFloat(items[k + 1]);
                    }
                }

                yield return new CTFSample()
                {
                    LineCount = seqStartLineCount,
                    SequenceCount = sequenceCount,
                    SequenceId = seqId,
                    DataSet = dss,
                    Comments = comments
                };

                splitLines.Clear();
                splitLines.Add(splitLine);

                seqStartLineCount = lineCount;
                seqId = n;
            }
        }

        public static Tuple<int, int> CountLines(TextReader reader)
        {
            var last = GetLineInfoReader(reader).Last();
            return new Tuple<int, int>(last.LineCount, last.SequenceCount);
        }

        public static string WritePartial(TextReader reader, TextWriter writer, int sequenceCount, string lastLine)
        {
            if (lastLine != null)
                writer.WriteLine(lastLine);

            foreach (var line in GetLineInfoReader(reader))
            {
                if (line.SequenceCount > sequenceCount)
                    return line.Line;

                writer.WriteLine(line.Line);
            }

            return null;
        }
    }
}
