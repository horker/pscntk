using System.Threading;
using CNTK;

namespace Horker.PSCNTK
{
    public class Random
    {
        static private int _seed = 123456789;
        static private long _globalGeneration = 0;
        static private System.Random _seedGenerator = new System.Random();

        static private ThreadLocal<System.Random> _instance = new ThreadLocal<System.Random>();
        static private ThreadLocal<long> _generation = new ThreadLocal<long>();

        static public void SetRandomSeed(int seed)
        {
            lock (_seedGenerator)
            {
                _seed = seed;
                _seedGenerator = new System.Random(_seed);
                ++_globalGeneration;
            }
        }

        static public void ResetRandomSeed()
        {
            SetRandomSeed(new System.Random().Next());
        }

        static public System.Random GetInstance()
        {
            if (!_instance.IsValueCreated || _globalGeneration > _generation.Value)
            {
                _instance.Value = new System.Random(_seedGenerator.Next());
                _generation.Value = _globalGeneration;
            }

            return _instance.Value;
        }

        static public System.Random GetInstance(int? seed)
        {
            if (seed.HasValue)
                return new System.Random(seed.Value);

            return GetInstance();
        }
    }
}
