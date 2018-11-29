using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Invoke", "CNTKFunction")]
    [Alias("cntk.invoke")]
    [CmdletBinding(DefaultParameterSetName = "Hashtable")]
    [OutputType(typeof(Value))]
    public class InvokeCNTKFunction : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Hashtable")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "DataSourceSet")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Sampler")]
        public WrappedFunction Function;

        [Parameter(Position = 0, Mandatory = false, ParameterSetName = "Hashtable")]
        public Hashtable Arguments;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "DataSourceSet")]
        public DataSourceSet DataSourceSet;

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Sampler")]
        public ISampler Sampler;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "Sampler")]
        public Hashtable DataNameToInputMap;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "Hashtable")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "DataSourceSet")]
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "Sampler")]
        public DeviceDescriptor Device;

        protected override void BeginProcessing()
        {
            if (Device == null)
                Device = DeviceDescriptor.UseDefaultDevice();
        }

        protected override void ProcessRecord()
        {
            IList<Value> results;
            
            if (ParameterSetName == "Hashtable")
                results = FunctionInvoke.Invoke(Function, Arguments, Device, false);
            else if (ParameterSetName == "DataSourceSet")
                results = FunctionInvoke.Invoke(Function, DataSourceSet, Device, false);
            else
            {
                DataNameToInputMap map = new DataNameToInputMap(new Function[] { Function }, DataNameToInputMap);
                Minibatch batch = null;
                var values = new List<Value>();
                do
                {
                    batch = Sampler.GetNextMinibatch(Device);
                    map.InitializeByMinibatch(batch);
                    values.AddRange(FunctionInvoke.Invoke(Function, batch, map, Device));
                }
                while (!batch.SweepEnd);
                results = values;
            }

            foreach (var r in results)
                WriteObject(r);
        }
    }
}

