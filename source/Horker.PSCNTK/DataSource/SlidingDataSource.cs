using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class SlidingDataSource<T> : DataSourceBase<T, SlidingList<T>>
    {
        private int _stride;

        public SlidingDataSource(IList<IList<T>> lists, int[] dimensions)
            : this(new SlidingList<T>(lists), dimensions)
        {
        }

        public SlidingDataSource(SlidingList<T> lists, int[] dimensions)
            : base(lists, dimensions)
        {
            if (Shape.Rank == 1)
                _stride = 1;
            else
                _stride = Shape.GetSize(-2);
        }

        public void AddSamples(IList<T> list)
        {
            if (list.Count % _stride != 0)
                throw new ArgumentException("Length of list should be a multiplication of the dimension of features");

            TypedData.AddList(list);
            Shape[-1] += list.Count / _stride;
        }

        public void SkipSamples(int n)
        {
            if (n < 0 || Shape[-1] < n)
                throw new ArgumentOutOfRangeException("n");

            TypedData.SkipElements(n * _stride);
            Shape[-1] -= n;
        }
    }
}
