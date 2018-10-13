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
    public class Logger
    {
        private TextWriter _writer;

        public TextWriter Writer => _writer;

        public Logger(TextWriter writer)
        {
            _writer = writer;
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
            // TODO: escape
            _writer.Write('"');
            _writer.Write(value);
            _writer.Write('"');
        }

        public void Write(bool value)
        {
            if (value)
                _writer.Write("true");
            else
                _writer.Write("false");
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

        public void Log(string sevirity, object data, string category)
        {
            var date = DateTimeOffset.Now.ToString("O");

            _writer.Write(
                string.Format("{{\"Timestamp\":\"{0}\",\"Severity\":\"{1}\",\"Category\":\"{2}\",DataType:\"{3}\",Data:",
                date, sevirity, category, data == null ? "System.Object" : data.GetType().FullName));
            Write(data, false);
            _writer.Write('}');
            WriteLine();
        }

        public void Info(object data, string category = "")
        {
            Log("INFO", data, category);
        }

        public void Warn(object data, string category = "")
        {
            Log("WARN", data, category);
        }

        public void Error(object data, string category = "")
        {
            Log("ERROR", data, category);
        }

        public void Fatal(object data, string category = "")
        {
            Log("FATAL", data, category);
        }

        public void Debug(object data, string category = "")
        {
            Log("DEBUG", data, category);
        }
    }
}
