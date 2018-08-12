using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class ShapeTest
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
        public void TestGetDimentionalIndexes()
        {
            var dims = new int[] { 3, 4, 5 };

            var s = new Shape(dims);

            CollectionAssert.AreEqual(new int[] { 0, 0, 0 }, s.GetDimensionalIndexes(0));
            CollectionAssert.AreEqual(new int[] { 1, 0, 0 }, s.GetDimensionalIndexes(1));
            CollectionAssert.AreEqual(new int[] { 1, 1, 0 }, s.GetDimensionalIndexes(1 + 3 * 1));
            CollectionAssert.AreEqual(new int[] { 0, 0, 1 }, s.GetDimensionalIndexes(0 + 3 * 0 + 3 * 4 * 1));
            CollectionAssert.AreEqual(new int[] { 1, 0, 1 }, s.GetDimensionalIndexes(1 + 3 * 0 + 3 * 4 * 1));
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, s.GetDimensionalIndexes(1 + 3 * 2 + 3 * 4 * 3));
        }
    }
}
