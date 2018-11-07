using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horker.PSCNTK;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class RandomizedListTest
    {
        [TestMethod]
        public void TestRandomizedListGetItem()
        {
            var a = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var ordinals = RandomizedList<float>.GetRandomizedIndexes(5);
            var l = new RandomizedList<int>(a, ordinals, 4);

            Assert.AreEqual(a.Length, l.Count);
            CollectionAssert.AreNotEqual(a, l.ToArray());

            Assert.AreEqual(l[0] + 1, l[1]);
            Assert.AreEqual(l[1] + 1, l[2]);
            Assert.AreEqual(l[2] + 1, l[3]);

            Assert.AreEqual(l[16] + 1, l[17]);
            Assert.AreEqual(l[17] + 1, l[18]);
            Assert.AreEqual(l[18] + 1, l[19]);

            var sorted = l.ToList();
            sorted.Sort();
            CollectionAssert.AreEqual(a, sorted);
        }
    }
}
