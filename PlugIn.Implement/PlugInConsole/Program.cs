using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using PlugInAbstract;

namespace PlugInConsole {
    internal class Program {
        private static List<PluginLoader> _loaders = new List<PluginLoader>();
        static void Main(string[] args)
        {
            // PluginLoader.CreateFromAssemblyFile(
            //     assemblyFile: "./plugins/MyPlugin/MyPlugin1.dll",
            //     sharedTypes: new[] {typeof(IPlugIn), typeof(IServiceCollection), typeof(ILogger)},
            //     isUnloadable: true);

            while (true)
            {
                Console.WriteLine("1.add, 2.run, 3.reload");
                var read = Console.ReadLine();
                if(read == "1") AddPlugin();
                else if(read == "2") RunPlugin();
                else if (read == "3")
                {
                    //_loaders[0].Dispose();
                    _loaders[0].Reload();
                }
            }
        }

        static void AddPlugin()
        {
            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir)) {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll)) {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        assemblyFile:pluginDll,
                        sharedTypes:new[] {typeof(IPlugIn)},
                        isUnloadable:true,
                        configure:config => config.EnableHotReload = true);
                    
                    _loaders.Add(loader);

                    loader.Reloaded += (s, e) =>
                    {
                        Console.WriteLine($"reload : {e.Loader.GetType().Name}");
                    };
                }
            }
        }

        static void RunPlugin()
        {
            // Create an instance of plugin types
            foreach (var loader in _loaders)
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IPlugIn).IsAssignableFrom(t) && !t.IsAbstract)) {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    var plugin = (IPlugIn) Activator.CreateInstance(pluginType);

                    Console.WriteLine($"Created plugin instance '{plugin.Run("seokwon")}'.");
                }

            Console.WriteLine("Enter : reload, Q: Quit");
            var en = Console.ReadLine();
            if (en.ToUpper() == "Q")
            {
                return;
            }
        }
    }
}
