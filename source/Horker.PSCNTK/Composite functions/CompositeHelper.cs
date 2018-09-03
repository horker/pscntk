using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static void Register(Variable v)
        {
            if (NodeGroup.Current == null)
                return;

            NodeGroup.Current.Add(v);
        }

        public static Function GetAffine(Variable n, Parameter weight, Parameter bias)
        {
            n = CNTKLib.Times(weight, n);
            Register(n);

            if (bias != null)
            {
                n = CNTKLib.Plus(n, bias);
                Register(n);
            }

            return n;
        }

        public static Function ApplyActivation(Function input, string activation)
        {
            if (activation != null)
            {
                var m = Helpers.GetCNTKLibMethod(activation, 1);
                input = (Function)m.Invoke(null, new object[] { (Variable)input });
                Register(input);
            }

            return input;
        }
    }
}
