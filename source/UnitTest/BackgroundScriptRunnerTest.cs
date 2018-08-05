using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;

namespace UnitTest
{
    [TestClass]
    public class BackgroundScriptRunnerTest
    {
        [TestMethod]
        public void TestException()
        {
            var r = new BackgroundScriptRunner();
            r.Start(ScriptBlock.Create("param($x) $xxx"), 10);
            r.Finish();
        }
    }
}