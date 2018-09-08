using CNTK;

namespace Horker.PSCNTK
{
    public class Random
    {
        static public System.Random GetInstance(int? seed = null)
        {
            if (seed.HasValue)
                return new System.Random(seed.Value);

            if (CNTKLib.IsRandomSeedFixed())
                return new System.Random((int)CNTKLib.GetRandomSeed());

            return new System.Random();
        }
    }
}
