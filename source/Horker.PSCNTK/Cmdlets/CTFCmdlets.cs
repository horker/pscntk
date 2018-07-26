using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace Horker.PSCNTK
{
    [Cmdlet("Export", "CNTKCTF")]
    public class ExportCNTKCTF: PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = true)]
        public Hashtable DataSources;

        protected override void EndProcessing()
        {
            var current = SessionState.Path.CurrentFileSystemLocation;
            var file = SessionState.Path.Combine(current.ToString(), Path);

            var names = new List<string>();
            var dataSources = new List<DataSource<float>>();

            foreach (DictionaryEntry entry in DataSources)
            {
                names.Add(entry.Key.ToString());

                DataSource<float> ds;
                if (entry.Value is PSObject)
                    ds = (DataSource<float>)(entry.Value as PSObject).BaseObject;
                else
                    ds = (DataSource<float>)entry.Value;

                dataSources.Add(ds);
            }

            using (var writer = new StreamWriter(file, false, new UTF8Encoding(false)))
            {
                DataSourceCTFBuilder.Write(writer, dataSources.ToArray(), names.ToArray());
            }
        }
    }
}