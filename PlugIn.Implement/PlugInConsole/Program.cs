using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlugInAbstract;

namespace PlugInConsole {
    class Program {
        static void Main(string[] args) {
            // PluginLoader.CreateFromAssemblyFile(
            //     assemblyFile: "./plugins/MyPlugin/MyPlugin1.dll",
            //     sharedTypes: new[] {typeof(IPlugIn), typeof(IServiceCollection), typeof(ILogger)},
            //     isUnloadable: true);
            
            var loaders = new List<PluginLoader>();

            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new [] { typeof(IPlugIn) });
                    loaders.Add(loader);

                    loader.Reloaded += (s, e) => {
                        Console.WriteLine($"reload : {e.Loader.GetType().Name}");
                    };
                }
            }
            
            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IPlugIn).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    IPlugIn plugin = (IPlugIn)Activator.CreateInstance(pluginType);

                    Console.WriteLine($"Created plugin instance '{plugin.Run("seokwon")}'.");
                }
            }
        }
    }
}