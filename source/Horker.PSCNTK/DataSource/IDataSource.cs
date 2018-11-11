using System;
using System.Collections.Generic;
using System.Drawing;
using CNTK;

namespace Horker.PSCNTK
{
    public interface IDataSource<T>
    {
        Shape Shape { get; }
        IList<T> Data { get; }

        T this[params int[] indexes] { get; set; }

        IDataSource<T> Apply(Func<T, int, T> func);
        void ApplyInPlace(Func<T, int, T> func);
        string AsString();
        StreamConfiguration GetStreamConfiguration(string name, string alias = "");
        IDataSource<T> GetSubsequences(int subseqLength, int step = 1, int sequenceAxis = -1);
        void Reshape(params int[] dimensions);

        void Save(string path, bool compress = true);
        byte[] Serialize(bool compress = true);

        IDataSource<T> Shuffle();
        void ShuffleInPlace();
        IDataSource<T> Subset(int offset, int count, int axis = -1);
        IDataSource<T>[] Split(params double[] rates);
        IDataSource<T> Transpose(params int[] order);

        T[] ToArray();
        Bitmap ToBitmap(ImageFormat imageFormat, bool scale);
        IDataSource<float> ToDataSourceFloat();
        MinibatchData ToMinibatchData(bool sweepEnd = false);
        NDArrayView ToNDArrayView(DeviceDescriptor device = null);
        string ToString();
        Value ToValue(DeviceDescriptor device = null);
        Variable ToVariable(DeviceDescriptor device = null);
    }
}