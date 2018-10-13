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
        public void TestLineInfoReader()
        {
            var reader = new StringReader(_ctf);
            var e = CTFTools.GetLineInfoReader(reader).GetEnumerator();

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

        [TestMethod]
        public void TestSampleReader()
        {
            var reader = new StringReader(_ctf);

            var e = CTFTools.GetSampleReader(reader).GetEnumerator();

            var c = e.MoveNext();
            Assert.IsTrue(c);
            var s = e.Current;

            Assert.AreEqual(1, s.LineCount);
            Assert.AreEqual(1, s.SequenceCount);
            Assert.AreEqual("0", s.SequenceId);

            CollectionAssert.AreEqual(new string[] { "data", "label" }, s.DataSet.Features.Keys);
            CollectionAssert.AreEqual(new int[] { 2, 2, 1 }, s.DataSet["data"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, s.DataSet["data"].Data.ToArray());
            CollectionAssert.AreEqual(new int[] { 2, 1, 1 }, s.DataSet["label"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 0, 1 }, s.DataSet["label"].Data.ToArray());

            c = e.MoveNext();
            Assert.IsTrue(c);
            s = e.Current;

            Assert.AreEqual(3, s.LineCount);
            Assert.AreEqual(2, s.SequenceCount);
            Assert.AreEqual("1", s.SequenceId);

            CollectionAssert.AreEqual(new string[] { "data", "label" }, s.DataSet.Features.Keys);
            CollectionAssert.AreEqual(new int[] { 2, 3, 1 }, s.DataSet["data"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 6, 7, 8, 9, 10, 11 }, s.DataSet["data"].Data.ToArray());
            CollectionAssert.AreEqual(new int[] { 2, 1, 1 }, s.DataSet["label"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 2, 3 }, s.DataSet["label"].Data.ToArray());

            c = e.MoveNext();
            Assert.IsTrue(c);
            s = e.Current;

            Assert.AreEqual(6, s.LineCount);
            Assert.AreEqual(3, s.SequenceCount);
            Assert.AreEqual("2", s.SequenceId);

            CollectionAssert.AreEqual(new string[] { "data", "label" }, s.DataSet.Features.Keys);
            CollectionAssert.AreEqual(new int[] { 2, 3, 1 }, s.DataSet["data"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 12, 13, 14, 15, 16, 17 }, s.DataSet["data"].Data.ToArray());
            CollectionAssert.AreEqual(new int[] { 2, 1, 1 }, s.DataSet["label"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 4, 5 }, s.DataSet["label"].Data.ToArray());

            c = e.MoveNext();
            Assert.IsFalse(c);
        }

        [TestMethod]
        public void TestSampleReaderComment()
        {
            var ctf =
                "99\t|data 9 8 7|  # comment1 \t\r\n" +
                "99\t|#comment2\t|data 6 5 4\r\n";

            var reader = new StringReader(ctf);
            var e = CTFTools.GetSampleReader(reader).GetEnumerator();

            var c = e.MoveNext();
            Assert.IsTrue(c);
            var s = e.Current;

            Assert.AreEqual(1, s.LineCount);
            Assert.AreEqual(1, s.SequenceCount);
            Assert.AreEqual("99", s.SequenceId);

            CollectionAssert.AreEqual(new string[] { "data" }, s.DataSet.Features.Keys);
            CollectionAssert.AreEqual(new int[] { 3, 2, 1 }, s.DataSet["data"].Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 9, 8, 7, 6, 5, 4 }, s.DataSet["data"].Data.ToArray());
            CollectionAssert.AreEqual(new string[] { "comment1", "comment2" }, s.Comments.ToArray());

            c = e.MoveNext();
            Assert.IsFalse(c);
        }
    }
}
