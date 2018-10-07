using System;
using CNTK;

namespace ConsoleApp1
{
    class Program
    {
        static Value GetValue()
        {
            var a = new NDArrayView(DataType.Float, new int[] { 1, 2, 3 }, DeviceDescriptor.CPUDevice);
            return new Value(a);
        }

        static MinibatchData GetMinibatchData()
        {
            var value = GetValue();
            return new MinibatchData(value.DeepClone());
        }

        static void Main(string[] args)
        {
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);

            var value = GetMinibatchData().data;
//            var value = GetValue();

            GC.Collect();
            Console.WriteLine(value.IsValid); // => true
            Console.WriteLine(string.Join(", ", value.Shape.Dimensions)); // => exception occurs
        }
    }
}
