using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKMinibatchSource")]
    [Alias("cntk.minibatchsource")]
    public class NewCNTKMinibatchSource : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Hashtable DataSources;

        [Parameter(Position = 1, Mandatory = false)]
        public UInt64 EpochSize = MinibatchSource.InfinitelyRepeat;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter NoRandomize = false;

        [Parameter(Position = 3, Mandatory = false)]
        public string CTFFile;

        protected override void EndProcessing()
        {
            string file;

            if (string.IsNullOrEmpty(CTFFile))
            {
                file = Path.GetTempFileName();
            }
            else
            {
                var current = SessionState.Path.CurrentFileSystemLocation;
                file = SessionState.Path.Combine(current.ToString(), CTFFile);
            }

            // Write CTF

            DataSourceCTFBuilder.Write(file, DataSources);

            // Bulid configuration

            var configs = new List<StreamConfiguration>();
            var sampleCount = -1;

            foreach (DictionaryEntry entry in DataSources)
            {
                var name = entry.Key.ToString();

                DataSource<float> ds;
                if (entry.Value is PSObject)
                    ds = (DataSource<float>)(entry.Value as PSObject).BaseObject;
                else
                    ds = (DataSource<float>)entry.Value;

                var count = ds.Shape[ds.Shape.Rank - 1];
                if (sampleCount == -1)
                    sampleCount = count;
                else
                    if (count != sampleCount)
                        throw new ArgumentException("Batch counts are different");

                var config = ds.GetStreamConfiguration(name);
                configs.Add(config);
            }

            var source = MinibatchSource.TextFormatMinibatchSource(file, configs, EpochSize, !NoRandomize);
            WriteObject(source);
        }
    }

    [Cmdlet("Close", "CNTKMinibatchSource")]
    public class CloseCNTKMinibatchSource : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public MinibatchSource MinibatchSource;

        protected override void EndProcessing()
        {
            MinibatchSource.Dispose();
        }
    }

    [Cmdlet("Get", "CNTKNextMinibatch")]
    [Alias("cntk.nextbatch")]
    public class GetCNTKMinibatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public MinibatchSource MinibatchSource;

        [Parameter(Position = 1, Mandatory = false)]
        public int SizeInSamples = 1;

        [Parameter(Position = 2, Mandatory = false)]
        public DeviceDescriptor Device = null;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter AsIs = false;

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = DeviceDescriptor.UseDefaultDevice();

            var batch = MinibatchSource.GetNextMinibatch((uint)SizeInSamples, Device);

            if (AsIs)
            {
                WriteObject(batch);
                return;
            }

            var result = new Dictionary<string, object>();
            result.Add("_source", batch);

            foreach (var info in batch.Keys)
            {
                var name = info.m_name;
                result.Add(name, batch[info]);
            }

            WriteObject(result);
        }
    }
}