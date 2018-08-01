using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class DataSourceTest
    {
        [TestMethod]
        public void TestShape()
        {
            var dims = new int[] { 3, 4, 5 };

            var s = new Shape(dims);

            // Dimensions
            CollectionAssert.AreEqual(dims, s.Dimensions);

            // Rank
            Assert.AreEqual(s.Rank, 3);

            // TotalSize
            Assert.AreEqual(s.TotalSize, 3 * 4 * 5);

            // GetSize()
            Assert.AreEqual(s.TotalSize, s.GetSize(2));
            Assert.AreEqual(3 * 4, s.GetSize(1));
            Assert.AreEqual(3, s.GetSize(0));
        }

        [TestMethod]
        public void TestCreate()
        {
            var a = new float[] { 10, 20 };
            var b = new float[] { 30, 40, 50, 60 };

            var result = DataSource<float>.Create(new float[][] { a, b }, new int[] { 2, 1, 3 });

            Assert.AreEqual(3, result.Shape.Rank);
            CollectionAssert.AreEqual(new int[] { 2, 1, 3 }, result.Shape.Dimensions);

            Assert.AreEqual(20.0, result[1, 0, 0]);
            Assert.AreEqual(50.0, result[0, 0, 2]);
        }

        [TestMethod]
        public void TestFromRows()
        {
            var a = new float[] { 10, 20, 30 };
            var b = new float[] { 40, 50, 60, 70 };

            var result = DataSource<float>.FromRows(new float[][] { a, b });

            Assert.AreEqual(2, result.Shape.Rank);
            Assert.AreEqual(2, result.Shape.Dimensions[0]);
            Assert.AreEqual(4, result.Shape.Dimensions[1]);

            Assert.AreEqual(0.0, result[0, 3]);
            Assert.AreEqual(60.0, result[1, 2]);
        }

        [TestMethod]
        public void TestFromColumns()
        {
            var a = new float[] { 10, 20, 30 };
            var b = new float[] { 40, 50, 60, 70 };

            var result = DataSource<float>.FromColumns(new float[][] { a, b });

            Assert.AreEqual(2, result.Shape.Rank);
            Assert.AreEqual(4, result.Shape.Dimensions[0]);
            Assert.AreEqual(2, result.Shape.Dimensions[1]);

            Assert.AreEqual(0.0, result[3, 0]);
            Assert.AreEqual(60.0, result[2, 1]);
        }

        [TestMethod]
        public void TestDataJoinAxis0()
        {
            var a = new DataSource<float>(new float[] { 11, 12, 13, 14 }, new int[] { 2, 1, 2 });
            var b = new DataSource<float>(new float[] { 21, 22, 23, 24, 25, 26 }, new int[] { 3, 1, 2 });

            var result = DataSource<float>.Combine(new DataSource<float>[] { a, b }, 0);

            var newShape = new int[] { 5, 1, 2 };
            var newData = new float[] {
                11, 12, 21, 22, 23,
                13, 14, 24, 25, 26
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestDataJoinAxis1()
        {
            var a = new DataSource<float>(new float[] { 11, 12, 13, 14 }, new int[] { 2, 2, 1 });
            var b = new DataSource<float>(new float[] { 21, 22, 23, 24, 25, 26 }, new int[] { 2, 3, 1 });

            var result = DataSource<float>.Combine(new DataSource<float>[] { a, b }, 1);

            var newShape = new int[] { 2, 5, 1 };
            var newData = new float[] {
                11, 12, 13, 14,
                21, 22, 23, 24, 25, 26
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestDataJoinAxis2()
        {
            var a = new DataSource<float>(new float[] { 11, 12, 13, 14, 15, 16, 17, 18 }, new int[] { 2, 2, 2 });
            var b = new DataSource<float>(new float[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 20, 21, 22 }, new int[] { 2, 2, 3 });

            var result = DataSource<float>.Combine(new DataSource<float>[] { a, b }, 2);

            var newShape = new int[] { 2, 2, 5 };
            var newData = new float[] {
                11, 12, 13, 14, 15, 16, 17, 18,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 20, 21, 22
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestSubsequence1()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4 }, new int[] { 1, 4, 1 });

            var result = a.GetSubsequences(3);

            var newShape = new int[] { 1, 3, 2 };
            var newData = new float[] {
                1, 2, 3,
                2, 3, 4
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestSubsequence2()
        {
            var a = new DataSource<float>(new float[] { 11, 12, 21, 22, 31, 32, 41, 42 }, new int[] { 2, 4, 1 });

            var result = a.GetSubsequences(2);

            var newShape = new int[] { 2, 2, 3 };
            var newData = new float[] {
                11, 12, 21, 22,
                21, 22, 31, 32,
                31, 32, 41, 42
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestSubsequence3()
        {
            var a = new DataSource<float>(new float[] { 11, 21, 31, 41, 12, 22, 32, 42 }, new int[] { 1, 4, 2 });

            var result = a.GetSubsequences(3);

            var newShape = new int[] { 1, 3, 4 };
            var newData = new float[] {
                11, 21, 31,
                21, 31, 41,
                12, 22, 32,
                22, 32, 42
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestSubsequence5()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new int[] { 1, 2, 6, 1 });

            var result = a.GetSubsequences(2, 2);

            var newShape = new int[] { 1, 2, 2, 3 };
            var newData = new float[] {
                1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12
            };

            CollectionAssert.AreEqual(newShape, result.Shape.Dimensions);
            CollectionAssert.AreEqual(newData, (float[])result.Data);
        }

        [TestMethod]
        public void TestSplit()
        {
            var a = new DataSource<float>(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, new int[] { 2, 6 });

            var results = a.Split(.34, 1);

            Assert.AreEqual(3, results.Length);

            CollectionAssert.AreEqual(new int[] { 2, 2 }, results[0].Shape.Dimensions);
            CollectionAssert.AreEqual(new int[] { 2, 1 }, results[1].Shape.Dimensions);
            CollectionAssert.AreEqual(new int[] { 2, 3 }, results[2].Shape.Dimensions);

            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, results[0].ToArray());
            CollectionAssert.AreEqual(new float[] { 4, 5 }, results[1].ToArray());
            CollectionAssert.AreEqual(new float[] { 6, 7, 8, 9, 10, 11 }, results[2].ToArray());
        }

        [TestMethod]
        public void TestToString1()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3 });

            var s = a.ToString();

            var expected = "DataSource [2 x 3] [ [1 2] [3 4] [5 6] ]";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestToString2()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new int[] { 2, 2, 3 });

            var s = a.ToString();

            var expected = "DataSource [2 x 2 x 3] [ [ [1 2] [3 4] ] [ [5 6] [7 8] ] [ [9 10] [11 12] ] ]";

            Assert.AreEqual(expected, s);
        }

        [TestMethod]
        public void TestSerialization()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new int[] { 2, 2, 3 });

            var bytes = a.Serialize(false);

            var b = DataSource<float>.Load(bytes, false);

            Assert.AreEqual(a.Shape, b.Shape);
            CollectionAssert.AreEqual(a.Data, b.Data);
        }

        [TestMethod]
        public void TestCompression()
        {
            var a = new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new int[] { 2, 2, 3 });

            var bytes = a.Serialize(true);

            var b = DataSource<float>.Load(bytes, true);

            Assert.AreEqual(a.Shape, b.Shape);
            CollectionAssert.AreEqual(a.Data, b.Data);
        }
    }
}
