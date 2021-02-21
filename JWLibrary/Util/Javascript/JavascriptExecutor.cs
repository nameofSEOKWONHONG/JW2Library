using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JWLibrary.Core;
using JWLibrary.Utils.Files;
using NiL.JS;
using NiL.JS.Core;

namespace JWLibrary.Util
{
    public static class JavascriptExecutor
    {
        public static void Execute(this string source) {
            ExecuteJavascriptSource(source, null, null);
        }
        
        public static void Execute(this string source, Action<Context> endaction) {
            ExecuteJavascriptSource(source, null, endaction);
        }

        public static void Execute(this string source, Action<Context> preaction, Action<Context> endaction) {
            ExecuteJavascriptSource(source, preaction, endaction);
        }
        
        private static void ExecuteJavascriptSource(string source, Action<Context> preaction, Action<Context> endaction)
        {
            Context context = new Context();
            if(preaction.jIsNotNull()) preaction(context);
            context.Eval(source);
            if(endaction.jIsNotNull()) endaction(context);
        }

        public static void ExecuteFile(this string fileName) {
            ExecuteJavascriptFile(fileName, null, null);
        }

        public static void ExecuteFile(this string fileName, Action<Context> endaction) {
            ExecuteJavascriptFile(fileName, null, endaction);
        }

        public static void ExecuteFile(this string fileName, Action<Context> preaction, Action<Context> endaction) {
            ExecuteJavascriptFile(fileName, preaction, endaction);
        }

        private static void ExecuteJavascriptFile(string fileName, Action<Context> preaction, Action<Context> endaction)
        {
            if (fileName.jIsNull()) throw new FileNotFoundException("jsPath is empty");
            if (!fileName.jFileExists()) throw new FileNotFoundException();
            if (!fileName.Contains(".js")) throw new FileLoadException("file is not support javascript");

            Context context = new Context();
            if(preaction.jIsNotNull()) preaction(context);
            context.Eval(string.Join("\n", fileName.jReadLines()));
            if(endaction.jIsNotNull()) endaction(context);
        }
    }
}