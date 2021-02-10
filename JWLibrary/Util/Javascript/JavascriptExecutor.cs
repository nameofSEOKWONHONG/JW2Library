using System;
using System.IO;
using System.Linq;
using JWLibrary.Core;
using JWLibrary.Utils.Files;
using NiL.JS.Core;

namespace JWLibrary.Util
{
    public static class JavascriptExecutor
    {
        public static void ExecuteJavascriptSource(this string source, Action<Context> preaction, Action<Context> endaction)
        {
            Context context = new Context();
            preaction(context);
            context.Eval(source);
            endaction(context);
        }
        public static void ExecuteJavascriptFile(this string fileName, Action<Context> preaction, Action<Context> endaction)
        {
            if (fileName.jIsNull()) throw new FileNotFoundException("jsPath is empty");
            if (!fileName.jFileExists()) throw new FileNotFoundException();
            if (!fileName.Contains(".js")) throw new FileLoadException("file is not support javascript");
            
            Context context = new Context();
            preaction(context);
            context.Eval(string.Join(" ", fileName.jReadLines()));
            endaction(context);
        }
    }
}