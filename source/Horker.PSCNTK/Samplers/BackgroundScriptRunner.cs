using System;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace Horker.PSCNTK
{
    public class BackgroundScriptRunner : IDisposable
    {
        private Runspace _runspace;
        private PowerShell _powerShell;

        private IAsyncResult _result;

        public bool HadErrors { get => _powerShell.HadErrors; }
        public PSDataStreams Streams { get => _powerShell.Streams; }

        public BackgroundScriptRunner()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _powerShell = PowerShell.Create();
            _powerShell.Runspace = _runspace;
            _runspace.Open();
        }

        public void Dispose()
        {
            _powerShell.Stop();
            _runspace.Close();
            _powerShell.Dispose();
        }

        public void Start(ScriptBlock script, params object[] arguments)
        {
            var scriptString = script.ToString();

            int paramIndex = 0;
            var ast = script.Ast as ScriptBlockAst;
            var paramBlock = ast.ParamBlock;
            if (paramBlock != null)
            {
                var paramString = paramBlock.ToString();
                paramIndex = scriptString.IndexOf(paramString);
                paramIndex += paramString.Length;
            }

            var s = scriptString.Substring(0, paramIndex) +
                " try { " +
                scriptString.Substring(paramIndex) +
                "\r\n}\r\ncatch {\r\n" +
                "[Console]::WriteLine('Error:')\r\n" +
                "[Console]::WriteLine($_.Exception.Message)\r\n" +
                "[Console]::WriteLine('--- StackTrace ---')\r\n" +
                "[Console]::WriteLine($_.Exception.StackTrace)\r\n" +
                "[Console]::WriteLine('--- ScriptStackTrace ---')\r\n" +
                "[Console]::WriteLine($Error[0].ScriptStackTrace)\r\n" +
                "}";

            _powerShell.AddScript(s);

            foreach (var arg in arguments)
                _powerShell.AddArgument(arg);

            _result = _powerShell.BeginInvoke();
        }

        public PSDataCollection<PSObject> Finish()
        {
            return _powerShell.EndInvoke(_result);
        }

        public void Stop()
        {
            _powerShell.Stop();
        }
    }
}
