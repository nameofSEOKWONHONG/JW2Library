using System;
using System.IO;
using JWLibrary.Core;

namespace JWLibrary.Utils.Files {
    public class FileSystemWatcherProvider : IDisposable {
        private FileSystemWatcher _watcher;

        public FileSystemWatcherProvider(string path,
            bool watchSubDir = true,
            int internalBufferSize = 32768, //32KB
            string fileExtensionFilters = null,
            NotifyFilters? notifyFilters = null) {
            _watcher = new FileSystemWatcher(path);

            _watcher.EnableRaisingEvents = false;
            _watcher.IncludeSubdirectories = watchSubDir;
            _watcher.InternalBufferSize = internalBufferSize;

            _watcher.Path = path;

            Console.WriteLine($"Watching Folder: {_watcher.Path}");

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.

            if (notifyFilters.isNotNull())
                _watcher.NotifyFilter = notifyFilters.Value;

            _watcher.Filter = fileExtensionFilters.ifNullOrEmpty(_ => "*.*");
        }

        public void Dispose() {
            if (_watcher.isNotNull()) {
                _watcher.Dispose();
                _watcher = null;
            }
        }

        public FileSystemWatcherProvider Changed(Action<object, FileSystemEventArgs, FileInfo> action) {
            if (_watcher.isNotNull())
                _watcher.Changed += (s, e) => { action(s, e, new FileInfo(e.FullPath)); };

            return this;
        }

        public FileSystemWatcherProvider Created(Action<object, FileSystemEventArgs, FileInfo> action) {
            if (_watcher.isNotNull())
                _watcher.Created += (s, e) => { action(s, e, new FileInfo(e.FullPath)); };

            return this;
        }

        public void Start() {
            if (_watcher.isNotNull()) {
                // Begin watching.
                _watcher.EnableRaisingEvents = true;
                Console.WriteLine("FileSystemWatcher Ready.");
            }
        }

        public void Stop() {
            if (_watcher.isNotNull()) {
                _watcher.EnableRaisingEvents = false;
                Console.WriteLine("FileSystemWatcher End.");
            }
        }
    }
}