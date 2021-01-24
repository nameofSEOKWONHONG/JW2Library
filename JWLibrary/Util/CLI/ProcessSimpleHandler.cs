using JWLibrary.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace JWLibrary.Utils {

    /// <summary>
    ///     execute command line base
    ///     (this method execute cmd.exe base)
    /// </summary>
    public class ProcessSimpleHandler : IDisposable {
        private readonly Process _process;

        public ProcessSimpleHandler(string[] killProcessNames = null) {
            if (this._process.jIsNull()) this._process = new Process();
        }

        public void Run(string fileName, string args, string workingDir) {
            this._process.StartInfo = new ProcessStartInfo();
            this._process.StartInfo.FileName = "cmd";
            this._process.StartInfo.Arguments = string.Format("/{0} {1}", "k", fileName, args);
            this._process.StartInfo.WorkingDirectory = workingDir;
            this._process.Start();
        }

        public void Stop() {
            if (this._process.jIsNotNull())
            {
                this._process.CloseMainWindow();
                this._process.Close();
            }
        }

        public void Dispose() {
            Stop();
        }
    }
}