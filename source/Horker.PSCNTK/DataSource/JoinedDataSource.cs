using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.PSCNTK
{
    public class JoinedDataSource<T> : DataSourceBase<T, JoinedList<T>>
    {
        private int _stride;

        public JoinedDataSource(JoinedList<T> lists, int[] dimensions)
            : base(lists, dimensions)
        {
            if (Shape.Rank == 1)
                _stride = 1;
            else
                _stride = Shape.GetSize(-2);
        }

        public void AddSamples(IList<T> list)
        {
            TypedData.AddList(list);
            Shape.Dimensions[Shape.Dimensions.Length - 1] += list.Count;
        }

        public void SkipSamples(int n)
        {
            TypedData.SkipElements(n * _stride);
            Shape.Dimensions[Shape.Dimensions.Length - 1] -= n;
        }
    }
}
