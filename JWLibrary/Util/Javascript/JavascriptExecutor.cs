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
        public static void ExecuteJavascriptSource(this string jssource, Action<Context> preaction, Action<Context> endaction)
        {
            Context context = new Context();
            preaction(context);
            context.Eval(jssource);
            endaction(context);
        }
        public static void ExecuteJavascriptFile(this string jsPath, Action<Context> preaction, Action<Context> endaction)
        {
            if (jsPath.jIsNull()) throw new FileNotFoundException("jsPath is empty");
            if (!jsPath.jFileExists()) throw new FileNotFoundException();
            if (!jsPath.Contains(".js")) throw new FileLoadException("file is not support javascript");
            
            Context context = new Context();
            preaction(context);
            context.Eval(string.Join(" ", jsPath.jReadLines()));
            endaction(context);
        }
    }
}