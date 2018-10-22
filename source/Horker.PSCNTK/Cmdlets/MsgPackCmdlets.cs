using System.IO;
using System.Management.Automation;
using MsgPack;
using MsgPack.Serialization;

namespace Horker.PSCNTK
{
    [Cmdlet("Set", "CNTKMsgPack")]
    public class SetCNTKMsgPack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public DataSourceSet DataSourceSet;

        [Parameter(Position = 1, Mandatory = true)]
        public string Path;

        protected override void BeginProcessing()
        {
            var path = IO.GetAbsolutePath(this, Path);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                MsgPackSerializer.Serialize(DataSourceSet, stream);
            }
        }
    }

    [Cmdlet("Add", "CNTKMsgPack")]
    public class AddCNTKMsgPack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public DataSourceSet DataSourceSet;

        [Parameter(Position = 1, Mandatory = true)]
        public string Path;

        protected override void BeginProcessing()
        {
            var path = IO.GetAbsolutePath(this, Path);

            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                MsgPackSerializer.Serialize(DataSourceSet, stream);
            }
        }
    }

    [Cmdlet("Get", "CNTKMsgPack")]
    public class GetCNTKMsgPack : PSCmdlet
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
