using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class Helpers
    {
        private static Dictionary<string, MethodInfo> _libMethods;

        public static MethodInfo GetCNTKLibMethod(string name)
        {
            if (_libMethods == null)
            {
                _libMethods = new Dictionary<string, MethodInfo>();
                var methods = typeof(CNTKLib).GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (var m in methods)
                    _libMethods[m.Name.ToLower()] = m;
            }

            MethodInfo methodInfo;
            if (!_libMethods.TryGetValue(name.ToLower(), out methodInfo))
                throw new ArgumentException(string.Format("Function not found: {0}", name));

            return methodInfo;
        }
    }
}
