using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class CTFTools
    {
        public class LineInfo
        {
            public int LineCount;
            public int SequenceCount;
            public string SequenceId;
            public string Line;
        }

        public static IEnumerable<LineInfo> Iterate(TextReader reader)
        {
            int lineCount = 0;
            int sequenceCount = 0;

            string line;
            string number = null;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineCount;

                var s = line.Split(new char[] { '|' });
                var n = s[0].Trim();
                if (n != number)
                    ++sequenceCount;

                number = n;

                yield return new LineInfo()
                {
                    LineCount = lineCount,
                    SequenceCount = sequenceCount,
                    SequenceId = number,
                    Line = line
                };
            }
        }

        public static Tuple<int, int> CountLines(TextReader reader)
        {
            var last = Iterate(reader).Last();
            return new Tuple<int, int>(last.LineCount, last.SequenceCount);
        }

        public static string WritePartial(TextReader reader, TextWriter writer, int sequenceCount, string lastLine)
        {
            if (lastLine != null)
                writer.WriteLine(lastLine);

            foreach (var line in Iterate(reader))
            {
                if (line.SequenceCount > sequenceCount)
                    return line.Line;

                writer.WriteLine(line.Line);
            }

            return null;
        }
    }
}
