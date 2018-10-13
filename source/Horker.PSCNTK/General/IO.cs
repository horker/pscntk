using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Horker.PSCNTK
{
    public class IO
    {
        public static string GetAbsolutePath(PSCmdlet cmdlet, string path)
        {
            var current = cmdlet.SessionState.Path.CurrentFileSystemLocation;

            if (!Path.IsPathRooted(path))
                path = cmdlet.SessionState.Path.Combine(current.ToString(), path);

            return path;
        }
    }
}
