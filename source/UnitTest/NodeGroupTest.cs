using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class NodeGroupText
    {
        public NodeGroupText()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
        }

        [TestMethod]
        public void TestNodeGroup()
        {
            NodeGroup group = null;
            Function n = null;
            try
            {
                group = NodeGroup.EnterNewGroup("test");

                n = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float);
                NodeGroup.Current.Add(n);

                n = CNTKLib.ReLU(n);
                NodeGroup.Current.Add(n);
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }

            var g = NodeGroup.Groups.ToArray();
            Assert.AreEqual(1, g.Length);
            Assert.AreEqual(group, g[0]);

            var nodes = g[0].Nodes.ToArray();
            Assert.AreEqual(2, nodes.Length);
            Assert.AreEqual(n.Inputs[0], nodes[0]);
            Assert.AreEqual((Variable)n, nodes[1]);
        }
    }
}
