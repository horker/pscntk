using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class CTFBuilderTest
    {
        [TestMethod]
        public void TestSingleLine()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer);

            builder.AddDenseSample("a", new float[] { 1, 2, 3 });
            builder.AddDenseSample("b", new float[] { 3.14f });

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|a 1 2 3\t|b 3.14", s);
        }

        [TestMethod]
        public void TestNextLine()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer);

            builder.AddDenseSample("a", new float[] { 1, 2, 3 });
            builder.NextLine();
            builder.AddDenseSample("b", new float[] { 3.14f });

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|a 1 2 3\r\n1\t|b 3.14", s);
        }

        [TestMethod]
        public void TestSequence()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer, 0, false);

            builder.AddDenseSample("a", new float[] { 1, 2, 3 });
            builder.NextLine();
            builder.AddDenseSample("b", new float[] { 3.14f });
            builder.NextLine();
            builder.NextSequence();
            builder.AddDenseSample("c", new float[] { 5 });
            builder.AddDenseSample("d", new float[] { 10 });
            builder.NextLine();

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|a 1 2 3\r\n0\t|b 3.14\r\n1\t|c 5\t|d 10", s);
        }

        [TestMethod]
        public void TestSparse()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer, 0, false);

            builder.AddSparseSample("a");
            builder.AddSparseValue(20, 1);
            builder.AddSparseValue(999, 10);

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|a 20:1 999:10", s);
        }

        [TestMethod]
        public void TestComment()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer, 0, false);

            builder.AddComment("hello world");
            builder.AddDenseSample("a", new float[] { 1, 2 });

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|# hello world\t|a 1 2", s);
        }

        [TestMethod]
        public void TestOneHot()
        {
            var writer = new StringWriter();
            var builder = new CTFBuilder(writer, 0, false);

            builder.AddOneHotSample("one", 5, 4);

            builder.Finish();

            var s = writer.ToString();
            Assert.AreEqual("0\t|one 0 0 0 0 1", s);
        }
    }
}
