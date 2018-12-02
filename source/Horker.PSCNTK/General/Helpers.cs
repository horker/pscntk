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

        public static MethodInfo GetCNTKLibMethod(string name, int parameterCount)
        {
            var mi = GetCNTKLibMethod(name);
            var p = mi.GetParameters();
            if (p.Length == parameterCount)
                return mi;

            throw new ArgumentException(string.Format(
                "Function found ({0}), but the number of parameters doesn't match ({1} required, {2} actual)", name, parameterCount, p.Length));
        }

        public static int[] GetShuffledSequencse(int count)
        {
            var random = Random.GetInstance();

            var result = new int[count];
            for (var i = 0; i < count; ++i)
            {
                var j = random.Next(i + 1);
                if (j != i)
                    result[i] = result[j];
                result[j] = i;
            }

            return result;
        }
    }
}
