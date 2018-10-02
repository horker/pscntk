using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
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
            Assert.AreEqual(n.Inputs[0].Uid, nodes[0].Uid);
            Assert.AreEqual(n.RootFunction.Uid, nodes[1].Uid);

            var f = NodeGroup.FindGroup(n.RootFunction.Uid);
            Assert.AreEqual("test", f.Name);
        }

        public static Function model;

        [TestMethod]
        public void TestNodeGroupsLifecycle()
        {
            var input = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float);
            var lstm = Horker.PSCNTK.Microsoft.LSTMSequenceClassifierNet.Create(input, 3, 4, false, true, DeviceDescriptor.UseDefaultDevice(), "LSTM");

            // Keep reference to the model
            model = lstm;

            for (var i = 0; i < 10; ++i)
            {
                GC.Collect();

                var g = NodeGroup.Groups.Where(x => x.Name == "LSTM_it").First();
                Assert.IsTrue(g.Nodes.Count() > 5);

                g = NodeGroup.Groups.Where(x => x.Name == "LSTM_ft").First();
                Assert.IsTrue(g.Nodes.Count() > 5);

                g = NodeGroup.Groups.Where(x => x.Name == "LSTM_ot").First();
                Assert.IsTrue(g.Nodes.Count() > 5);
            }
        }
    }
}
