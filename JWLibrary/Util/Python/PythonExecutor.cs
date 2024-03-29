﻿using System;
using System.IO;
using System.Threading.Tasks;
using eXtensionSharp;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace JWLibrary.Util
{
    public static class PythonExecutor
    {
        public static Task Execute_Python_Script_Async(this string script, Action<ScriptScope> preaction,
            Action<ScriptScope> endaction)
        {
            if (script.xIsNullOrEmpty()) throw new Exception("script is empty.");

            var engine = Python.CreateEngine();
            var src = engine.CreateScriptSourceFromString(script);
            var scope = engine.CreateScope();
            var paths = engine.GetSearchPaths();
            engine.SetSearchPaths(paths);

            preaction(scope);
            src.Execute(scope);
            endaction(scope);

            return Task.CompletedTask;
        }

        public static async Task Execute_Python_File_Async(this string fileName, Action<ScriptScope> preaction,
            Action<ScriptScope> endaction)
        {
            if (fileName.xIsNull()) throw new FileNotFoundException("pytho_file_path is empty");
            if (!fileName.xFileExists()) throw new FileNotFoundException();
            if (!fileName.Contains(".py")) throw new FileLoadException("file is not support python");

            var lines = await fileName.xFileReadAllLinesAsync();

            var engine = Python.CreateEngine();
            var src = engine.CreateScriptSourceFromFile(fileName);
            var scope = engine.CreateScope();
            var paths = engine.GetSearchPaths();
            engine.SetSearchPaths(paths);

            preaction(scope);
            src.Execute(scope);
            endaction(scope);
        }
    }
}