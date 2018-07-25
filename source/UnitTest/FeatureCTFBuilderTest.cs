using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class FeatureCTFBuilderTest
    {
        [TestMethod]
        public void TestDenseFeature()
        {
            var builder = new FeatureCTFBuilder();

            builder.AddDenseFeature("by_sequence", new double[] { 1, 2, 3, 4, 5, 6, 7 }, 3);

            builder.AddDenseFeature("by_array_of_array", new double[][] {
                new double[] { 1, 2 }, new double[] { 3, 4, 5 }, new double[] { 6, 7, 8, 9 }
            });

            builder.AddDenseFeature("step_by_step");
            builder.AddDenseSample(new double[] { 10, 20, 30 });
            builder.AddDenseSample(new double[] { 40, 50, 60 });

            var writer = new StringWriter();
            builder.Write(writer);
            var s = writer.ToString();

            var expected =
                "0\t|by_sequence 1 2 3\t|by_array_of_array 1 2\t|step_by_step 10 20 30\r\n" +
                "1\t|by_sequence 4 5 6\t|by_array_of_array 3 4 5\t|step_by_step 40 50 60\r\n" +
                "2\t|by_sequence 7\t|by_array_of_array 6 7 8 9";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestSparseFeature()
        {
            var builder = new FeatureCTFBuilder();

            builder.AddSparseFeature("sparse");

            builder.StartNewSparseSample();
            builder.AddSparseValue(123, 9);
            builder.AddSparseValue(124, 99);
            builder.AddSparseValue(125, 999);

            builder.StartNewSparseSample();
            builder.AddSparseValue(0, 1);
            builder.AddSparseValue(1, 2);

            var writer = new StringWriter();
            builder.Write(writer);
            var s = writer.ToString();

            var expected =
                "0\t|sparse 123:9 124:99 125:999\r\n" +
                "1\t|sparse 0:1 1:2";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestOneHotFeature()
        {
            var builder = new FeatureCTFBuilder();

            builder.AddOneHotFeature("onehot", new int[] { 0, 1, 2, 3 }, 5);

            var writer = new StringWriter();
            builder.Write(writer);
            var s = writer.ToString();

            var expected =
                "0\t|onehot 1 0 0 0 0\r\n" +
                "1\t|onehot 0 1 0 0 0\r\n" +
                "2\t|onehot 0 0 1 0 0\r\n" +
                "3\t|onehot 0 0 0 1 0";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestComment()
        {
            var builder = new FeatureCTFBuilder();

            builder.AddComment(new string[] { "hello", "world", "!" });
            builder.AddDenseFeature("dense", new double[] { 1, 2, 3 }, 1);

            var writer = new StringWriter();
            builder.Write(writer);
            var s = writer.ToString();

            var expected =
                "0\t|# hello\t|dense 1\r\n" +
                "1\t|# world\t|dense 2\r\n" +
                "2\t|# !\t|dense 3";

            Assert.AreEqual(expected, s);
        }

    }
}
