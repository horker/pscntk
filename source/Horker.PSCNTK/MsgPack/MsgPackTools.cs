using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class MsgPackTools
    {
        public static int GetTotalSampleCount(Stream stream)
        {
            int count = 0;
            while (stream.Position < stream.Length)
            {
                var dss = MsgPackSerializer.Deserialize(stream);
                count += dss.SampleCount;
            }

            return count;
        }

        public static IEnumerable<DataSourceSet> ReadDataSourceSet(Stream stream, int totalSampleCount, int splitCount)
        {
            var device = DeviceDescriptor.CPUDevice;

            using (var sampler = new MsgPackSampler(stream, splitCount, false, totalSampleCount, 1000, false))
            {
                for (var count = 0; count < totalSampleCount;)
                {
                    var dss = sampler.Deque();
                    count += dss.SampleCount;

                    if (count > totalSampleCount)
                        dss = dss.Subset(0, splitCount - (count - totalSampleCount));

                    yield return dss;
                }
            }
        }
    }
}
