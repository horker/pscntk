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

        void ApplyInPlace(Func<int, T, T> func);
        string AsString();
        StreamConfiguration GetStreamConfiguration(string name, string alias = "");
        IDataSource<T> GetSubsequences(int subseqLength, int step = 1, int sequenceAxis = -1);
        void Reshape(params int[] dimensions);
        void Save(string path, bool compress = true);
        byte[] Serialize(bool compress = true);
        void Shuffle(int? seed = null);
        IDataSource<T> Slice(int from, int to);
        IDataSource<T>[] Split(params double[] rates);
        T[] ToArray();
        Bitmap ToBitmap(ImageFormat imageFormat, bool scale);
        IDataSource<float> ToDataSourceFloat();
        MinibatchData ToMinibatchData(bool sweepEnd = false);
        NDArrayView ToNDArrayView(DeviceDescriptor device = null);
        string ToString();
        Value ToValue(DeviceDescriptor device = null);
        Variable ToVariable(DeviceDescriptor device = null);
        IDataSource<T> Transpose(params int[] order);
    }
}