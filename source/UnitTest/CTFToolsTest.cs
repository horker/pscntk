using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class CTFToolsTest
    {
        private string _ctf;

        public CTFToolsTest()
        {
            _ctf =
                "0\t|data 0 1\t|label 0 1\r\n" +
                "0\t|data 2 3\r\n" +
                "1\t|data 6 7\t|label 2 3\r\n" +
                "1\t|data 8 9\r\n" +
                "1\t|data 10 11\r\n" +
                "2\t|data 12 13\t|label 4 5\r\n" +
                "2\t|data 14 15\r\n" +
                "2\t|data 16 17";
        }

        [TestMethod]
        public void TestIterate()
        {
            var reader = new StringReader(_ctf);
            var e = CTFTools.Iterate(reader).GetEnumerator();

            {
                var state = e.MoveNext();
                Assert.IsTrue(state);

                var l = e.Current;
                Assert.AreEqual(1, l.LineCount);
                Assert.AreEqual(1, l.SequenceCount);
                Assert.AreEqual("0", l.SequenceId);
                Assert.AreEqual("0\t|data 0 1\t|label 0 1", l.Line);
            }

            {
                var state = e.MoveNext();
                Assert.IsTrue(state);

                var l = e.Current;
                Assert.AreEqual(2, l.LineCount);
                Assert.AreEqual(1, l.SequenceCount);
                Assert.AreEqual("0", l.SequenceId);
                Assert.AreEqual("0\t|data 2 3", l.Line);
            }

            {
                var state = e.MoveNext();
                Assert.IsTrue(state);

                var l = e.Current;
                Assert.AreEqual(3, l.LineCount);
                Assert.AreEqual(2, l.SequenceCount);
                Assert.AreEqual("1", l.SequenceId);
                Assert.AreEqual("1\t|data 6 7\t|label 2 3", l.Line);
            }
        }

        [TestMethod]
        public void TestCountLines()
        {
            var reader = new StringReader(_ctf);
            var l = CTFTools.CountLines(reader);

            Assert.AreEqual(8, l.Item1);
            Assert.AreEqual(3, l.Item2);
        }

        [TestMethod]
        public void TestWritePartial1()
        {
            var reader = new StringReader(_ctf);
            var writer = new StringWriter();

            CTFTools.WritePartial(reader, writer, 1, null);

            var s = writer.ToString();

            var expected =
                "0\t|data 0 1\t|label 0 1\r\n" +
                "0\t|data 2 3\r\n";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestWritePartial2()
        {
            var reader = new StringReader(_ctf);
            var writer = new StringWriter();

            CTFTools.WritePartial(reader, writer, 2, null);

            var s = writer.ToString();

            var expected =
                "0\t|data 0 1\t|label 0 1\r\n" +
                "0\t|data 2 3\r\n" +
                "1\t|data 6 7\t|label 2 3\r\n" +
                "1\t|data 8 9\r\n" +
                "1\t|data 10 11\r\n";

            Assert.AreEqual(expected, s);
        }
    }
}
