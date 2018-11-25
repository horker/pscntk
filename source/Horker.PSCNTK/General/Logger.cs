using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Horker.PSCNTK
{
    public class Logger : IDisposable
    {
        private string _logFile;
        private TextWriter _writer;
        private string _defaultSource;

        public string LogFile => _logFile;
        public TextWriter Writer => _writer;
        public string DefaultSource => _defaultSource;

        public Logger(string logFile, bool append, string defaultSource = "")
        {
            _logFile = logFile;
            _writer = new StreamWriter(logFile, append, new UTF8Encoding(false));
            _defaultSource = defaultSource;
        }

        public Logger(TextWriter writer, string defaultSource = "")
        {
            _logFile = null;
            _writer = writer;
            _defaultSource = defaultSource;
        }

        public void Dispose()
        {
            if (_logFile != null)
                ((IDisposable)_writer).Dispose();
        }

        public void Close()
        {
            Dispose();
        }

        public void Write(int value)
        {
            _writer.Write(value.ToString());
        }

        public void Write(float value)
        {
            _writer.Write(value.ToString());
        }

        public void Write(double value)
        {
            _writer.Write(value.ToString());
        }

        public void Write(string value)
        {
            _writer.Write('"');
            _writer.Write(EscapeString(value));
            _writer.Write('"');
        }

        public string EscapeString(string s)
        {
            if (s.IndexOf('"') < 0)
                return s;

            return s.Replace("\"", "\\\"");
        }

        public void Write(bool value)
        {
            if (value)
                _writer.Write("true");
            else
                _writer.Write("false");
        }

        public void Write(IDictionary value)
        {
            bool first = true;
            _writer.Write('{');
            foreach (DictionaryEntry entry in value)
            {
                if (!first)
                    _writer.Write(',');
                first = false;

                Write(entry.Key, false);
                _writer.Write(':');
                Write(entry.Value, false);
            }
            _writer.Write(']');
        }

        public void Write(ICollection value)
        {
            bool first = true;
            _writer.Write('[');
            foreach (var v in value)
            {
                if (!first)
                    _writer.Write(',');
                first = false;

                Write(v, false);
            }
            _writer.Write(']');
        }

        public void Write(object value, bool inner)
        {
            if (value == null)
            {
                _writer.Write("null");
                return;
            }

            if (value is int intValue) { Write(intValue); return; }
            if (value is float floatValue) { Write(floatValue); return; }
            if (value is double doubleValue) { Write(doubleValue); return; }
            if (value is string stringValue) { Write(stringValue); return; }
            if (value is bool boolValue) { Write(boolValue); return; }
            if (value is IDictionary dictionaryValue) { Write(dictionaryValue); return; }
            if (value is ICollection collectionValue) { Write(collectionValue); return; }
            if (value is PSObject psObjectValue) { Write(psObjectValue); return; }

            // generic objects

            if (inner)
            {
                Write(value.ToString());
                return;
            }

            bool first = true;
            _writer.Write('{');
            var members = value.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField);
            foreach (var member in members)
            {
                PropertyInfo prop = null;
                FieldInfo field = null;
                var skip = true;

                if (member is PropertyInfo p)
                {
                    prop = p;
                    skip = false;
                }
                else if (member is FieldInfo f)
                {
                    field = f;
                    skip = false;
                }

                if (skip)
                    continue;

                if (!first)
                    _writer.Write(',');
                first = false;

                Write(member.Name);
                _writer.Write(':');

                if (prop != null)
                    Write(prop.GetValue(value), true);
                else
                    Write(field.GetValue(value), true);
            }
            _writer.Write('}');
        }

        public void Write(PSObject obj)
        {
            bool first = true;
            _writer.Write('{');
            foreach (var prop in obj.Properties)
            {
                if (!first)
                    _writer.Write(',');
                first = false;

                Write(prop.Name);
                _writer.Write(':');
                Write(prop.Value, true);
            }
            _writer.Write('}');
        }

        public void WriteLine()
        {
            _writer.WriteLine();
            _writer.Flush();
        }

        public void Log(string sevirity, object data, string source)
        {
            var date = DateTimeOffset.Now.ToString("O");

            if (string.IsNullOrEmpty(source))
                source = _defaultSource;

            _writer.Write(
                string.Format("{{\"Timestamp\":\"{0}\",\"Severity\":\"{1}\",\"Source\":\"{2}\",DataType:\"{3}\",Data:",
                date, sevirity, source, data == null ? "System.Object" : data.GetType().FullName));
            Write(data, false);
            _writer.Write('}');
            WriteLine();
        }

        public void Info(object data, string source = "")
        {
            Log("INFO", data, source);
        }

        public void Warn(object data, string source = "")
        {
            Log("WARN", data, source);
        }

        public void Error(object data, string source = "")
        {
            Log("ERROR", data, source);
        }

        public void Fatal(object data, string source = "")
        {
            Log("FATAL", data, source);
        }

        public void Debug(object data, string source = "")
        {
            Log("DEBUG", data, source);
        }
    }
}
