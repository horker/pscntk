using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horker.PSCNTK;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class SlidingListTest
    {
        [TestMethod]
        public void TestGetItem()
        {
            var l = new SlidingList<int>(new IList<int>[] { new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 } });

            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(20, l[4]);
        }

        [TestMethod]
        public void TestToArray()
        {
            var l = new SlidingList<int>(new IList<int>[] { new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 } });

            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 10, 20, 30 }, l.ToArray());
        }

        [TestMethod]
        public void TestCopyTo()
        {
            var l = new SlidingList<int>(new IList<int>[] { new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 } });
            var a = new int[10];

            l.CopyTo(a, 3);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 1, 2, 3, 10, 20, 30, 0 }, a);
        }

        [TestMethod]
        public void TestCopyZeroLengthList()
        {
            var l = new SlidingList<int>(new IList<int>[] { });
            var a = new int[5];

            l.CopyTo(a, 3);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 0, 0 }, a);
        }

        [TestMethod]
        public void TestCombinationWithSlice()
        {
            var l = new SlidingList<int>(new IList<int>[] { new int[] { 1, 2, 3 }, new int[] { 10, 20, 30 }, new int[] { 111, 222, 333, 444 } });
            var l2 = new ListSlice<int>(l, 2, 7);
            var a = new int[8];

            l2.CopyTo(a, 1);
            CollectionAssert.AreEqual(new int[] { 0, 3, 10, 20, 30, 111, 222, 333 }, a);
        }
    }
}
