using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class Constants
    {
        public static readonly UInt32 SentinelValueForAutoSelectRandomSeed = UInt32.MaxValue - 2; // An arbitrary choice of sentinel value

        public static readonly int SentinelValueForInferParamInitRank = int.MaxValue;
        public static readonly int DefaultParamInitScale = 1;
        public static readonly int DefaultParamInitOutputRank = 1;
        public static readonly int DefaultParamInitFilterRank = 0;
    }
}
