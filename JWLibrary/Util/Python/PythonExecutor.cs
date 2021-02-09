using System;
using System.IO;
using System.Threading.Tasks;
using IronPython.Hosting;
using JWLibrary.Core;
using JWLibrary.Utils.Files;
using Microsoft.Scripting.Hosting;

namespace JWLibrary.Util
{
    public static class PythonExecutor
    {
        public static async Task Execute_Python_Script_Async(this string python_script, Action<ScriptScope> preaction, Action<ScriptScope> endaction)
        {
            if (python_script.jIsNullOrEmpty()) throw new Exception("script is empty.");

            var engine = Python.CreateEngine();
            var src = engine.CreateScriptSourceFromString(python_script);
            var scope = engine.CreateScope();
            var paths = engine.GetSearchPaths();
            engine.SetSearchPaths(paths);
            
            preaction(scope);
            src.Execute(scope);
            endaction(scope);
        }
        
        public static async Task Execute_Python_File_Async(this string python_file_path, Action<ScriptScope> preaction, Action<ScriptScope> endaction)
        {
            if (python_file_path.jIsNull()) throw new FileNotFoundException("pytho_file_path is empty");
            if (!python_file_path.jFileExists()) throw new FileNotFoundException();
            if (!python_file_path.Contains(".py")) throw new FileLoadException("file is not support python");

            var lines = await python_file_path.jReadLinesAsync();

            var engine = Python.CreateEngine();
            var src = engine.CreateScriptSourceFromFile(python_file_path);
            var scope = engine.CreateScope();
            var paths = engine.GetSearchPaths();
            engine.SetSearchPaths(paths);
            
            preaction(scope);
            src.Execute(scope);
            endaction(scope);
        }
    }
}