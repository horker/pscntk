using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace UnitTest
{
    [TestClass]
    public class LoggerTest
    {
        public class TestClass
        {
            public string Text;
            public int Number;
            public TestClass Inner;

            public override string ToString()
            {
                return "test class";
            }
        }

        private string OutputLog(object data)
        {
            using (var writer = new StringWriter())
            {
                var logger = new Logger(writer);
                logger.Info(data);

                var s = writer.ToString();
                return Regex.Replace(s, "\"Timestamp\":\"[^\"]+\"", "\"Timestamp\":\"\"");
            }
        }

        [TestMethod]
        public void TestLoggingVariousTypes()
        {
            string actual;
            string expected;

            actual = OutputLog(11);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Int32\",Data:11}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog(22.2);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Double\",Data:22.2}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog(33.3f);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Single\",Data:33.3}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog("hello");
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.String\",Data:\"hello\"}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog(true);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Boolean\",Data:true}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog(null);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Object\",Data:null}\r\n";
            Assert.AreEqual(expected, actual);

            var testObject = new TestClass()
            {
                Text = "text",
                Number = 123,
                Inner = new TestClass() { Text = "text2", Number = 456, Inner = null }
            };

            actual = OutputLog(testObject);
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"UnitTest.LoggerTest+TestClass\",Data:{\"Text\":\"text\",\"Number\":123,\"Inner\":\"test class\"}}\r\n";
            Assert.AreEqual(expected, actual);

            actual = OutputLog(new PSObject(testObject));
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.Management.Automation.PSObject\",Data:{\"Text\":\"text\",\"Number\":123,\"Inner\":\"test class\"}}\r\n";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLoggingSeverityAndCategory()
        {
            string actual;
            string expected;

            using (var writer = new StringWriter())
            {
                var logger = new Logger(writer);
                logger.Fatal(1234, "cat");

                var s = writer.ToString();
                actual = Regex.Replace(s, "\"Timestamp\":\"[^\"]+\"", "\"Timestamp\":\"\"");
            }

            expected = "{\"Timestamp\":\"\",\"Severity\":\"FATAL\",\"Source\":\"cat\",DataType:\"System.Int32\",Data:1234}\r\n";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLoggingEscape()
        {
            string actual;
            string expected;

            actual = OutputLog("abc\"def\"\r\nxyz");
            expected = "{\"Timestamp\":\"\",\"Severity\":\"INFO\",\"Source\":\"\",DataType:\"System.String\",Data:\"abc\\\"def\\\"\\r\\nxyz\"}\r\n";
            Assert.AreEqual(expected, actual);
        }
    }
}
