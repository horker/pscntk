using System;
using System.IO;
using System.Management.Automation;
using MsgPack;
using MsgPack.Serialization;

namespace Horker.PSCNTK
{
    [Cmdlet("Export", "CNTKMsgPack")]
    public class ExportCNTKMsgPack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public DataSourceSet DataSourceSet;

        [Parameter(Position = 1, Mandatory = true)]
        public string Path;

        [Parameter(Position = 2, Mandatory = false)]
        public int SplitSize = int.MaxValue;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter OmitFraction = false;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter Append = false;

        protected override void BeginProcessing()
        {
            var path = IO.GetAbsolutePath(this, Path);
            var fileMode = Append ? FileMode.Append : FileMode.Create;

            using (var stream = new FileStream(path, fileMode, FileAccess.Write))
            {
                var count = DataSourceSet.SampleCount;
                for (var i = 0; i < count; i += SplitSize)
                {
                    var size = Math.Min(SplitSize, count - i);
                    if (OmitFraction && size < SplitSize)
                        break;

                    var chunk = DataSourceSet.Slice(i, size);
                    MsgPackSerializer.Serialize(chunk, stream);
                }
            }
        }
    }

    [Cmdlet("Import", "CNTKMsgPack")]
    public class ImportCNTKMsgPack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = false)]
        public int TotalCount = int.MaxValue;

        protected override void BeginProcessing()
        {
            var path = IO.GetAbsolutePath(this, Path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                for (var i = 0; i < TotalCount; ++i)
                {
                    var dss = MsgPackSerializer.Deserialize(stream);

                    WriteObject(dss);

                    if (stream.Length == stream.Position)
                        break;
                }
            }
        }
    }
}
