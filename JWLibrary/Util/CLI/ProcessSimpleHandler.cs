using System;
using System.Diagnostics;
using eXtensionSharp;

namespace JWLibrary.Utils {
    /// <summary>
    ///     execute command line base
    ///     (this method execute cmd.exe base)
    /// </summary>
    public class ProcessSimpleHandler : IDisposable {
        private readonly Process _process;

        public ProcessSimpleHandler(string[] killProcessNames = null) {
            if (_process.xIsNull()) _process = new Process();
        }

        public void Dispose() {
            Stop();
        }

        public void Run(string fileName, string args, string workingDir, bool isCreateNoWindow = true, bool isUseShellExecute = false) {
            _process.StartInfo = new ProcessStartInfo();
            _process.StartInfo.FileName = "cmd";
            _process.StartInfo.Arguments = string.Format("/{0} {1}", "k", fileName, args);
            _process.StartInfo.WorkingDirectory = workingDir;
            _process.StartInfo.CreateNoWindow = isCreateNoWindow;
            _process.StartInfo.UseShellExecute = isUseShellExecute;
            _process.Start();
        }

        public void Stop() {
            if (_process.xIsNotNull()) {
                _process.CloseMainWindow();
                _process.Close();
            }
        }
    }
}