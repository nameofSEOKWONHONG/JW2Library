using System;
using System.Diagnostics;
using JWLibrary.Core;

namespace JWLibrary.Utils {
    /// <summary>
    ///     execute command line base
    ///     (this method execute cmd.exe base)
    /// </summary>
    public class ProcessSimpleHandler : IDisposable {
        private readonly Process _process;

        public ProcessSimpleHandler(string[] killProcessNames = null) {
            if (_process.isNull()) _process = new Process();
        }

        public void Dispose() {
            Stop();
        }

        public void Run(string fileName, string args, string workingDir) {
            _process.StartInfo = new ProcessStartInfo();
            _process.StartInfo.FileName = "cmd";
            _process.StartInfo.Arguments = string.Format("/{0} {1}", "k", fileName, args);
            _process.StartInfo.WorkingDirectory = workingDir;
            _process.Start();
        }

        public void Stop() {
            if (_process.isNotNull()) {
                _process.CloseMainWindow();
                _process.Close();
            }
        }
    }
}