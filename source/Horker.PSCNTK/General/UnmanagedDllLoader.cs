using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class UnmanagedDllLoader
    {
        // The order of the files is significant
        private static readonly string[] FILES = new string[] {
          "cublas64_90.dll",
          "cudart64_90.dll",
          "cudnn64_7.dll",
          "curand64_90.dll",
          "cusparse64_90.dll",
          "libiomp5md.dll",
          "nvml.dll",
          "opencv_world310.dll",
//          "zip.dll",
          "zlib.dll",

          "mklml.dll",
          "mkldnn.dll",

          "Cntk.Math-2.5.1.dll",
          "Cntk.PerformanceProfiler-2.5.1.dll",

          "Cntk.Core-2.5.1.dll",

          "Cntk.Deserializers.Binary-2.5.1.dll",
          "Cntk.Deserializers.HTK-2.5.1.dll",
//          "Cntk.Deserializers.Image-2.5.1.dll",
          "Cntk.Deserializers.TextFormat-2.5.1.dll",
          "Cntk.Composite-2.5.1.dll",
          "Cntk.Core.CSBinding-2.5.1.dll"
        };

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        private static bool loaded = false;

        public static void Load(string dllPath)
        {
            if (loaded)
                return;

            foreach (var file in FILES)
            {
                var path = Path.Combine(new string[] { dllPath, file });
                var result = LoadLibrary(path);
                if (result == IntPtr.Zero)
                    throw new InvalidOperationException(string.Format("Failed to load: {0}", path));
            }

            loaded = true;

            // For debugging purpose
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }
    }
}
