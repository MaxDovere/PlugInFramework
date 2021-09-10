using PlugInBase.Abstractions;
using PlugInBase.Models;
using PlugInManager.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace PlugInManager
{
    public class PlugInHandlerContext
    {
        public static List<IPlugIn> LoadPlugins(string[] pluginPaths)
        {
            return ResolvePathsToLocal(pluginPaths).SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPluginAssembly(pluginPath);
                IEnumerable<IPlugIn> pluginsFromAssembly = CreateAllPluginsInAssembly(pluginAssembly);
                foreach (IPlugIn plugin in pluginsFromAssembly)
                {
                    plugin.OnLoad(null);
                }
                return pluginsFromAssembly;
            }).ToList();
        }

        private static IEnumerable<string> ResolvePathsToLocal(string[] pluginPaths)
        {
            return pluginPaths.ToList().Select<string, string>(pluginPath =>
            {
                if (PathTools.IsRemotePath(pluginPath))
                {
                    pluginPath = DownloadPlugin(pluginPath);
                    if (pluginPath == null)
                    {
                        Console.WriteLine("Failed to download " + pluginPath);
                        return null;
                    }
                }
                return pluginPath;
            }).Where(x => x != null).Distinct();
        }

        private static string DownloadPlugin(string path)
        {
            Console.WriteLine("Remote plugin - " + path);
            using (WebClient client = new WebClient())
            {
                string fileName = PathTools.UrlToFilename(path, "dll", @".\Plugins\");
                Console.WriteLine("Will save as " + fileName);
                if (File.Exists(fileName))
                {
                    Console.WriteLine("Will try to delete existing file");
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not delete - " + ex.Message);
                    }
                }

                try
                {
                    client.DownloadFile(path, fileName);
                    return fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to download plugin from " + path);
                    Console.WriteLine(ex.Message);
                    if(ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }
                    return null;
                }
            }
        }

        public static PlugInReturnData RunPlugins(ArgumentsParser argParser, IEnumerable<IPlugIn> plugins)
        {
            List<string> targetPlugins = argParser.GetValues(ArgumentsConfig.TargetPlugins);
            if (targetPlugins == null)
            {
                targetPlugins = plugins.OrderBy(p => p.OrderId).Select(p => p.Name).ToList();
            }

            foreach (string pluginName in targetPlugins)
            {
                Console.WriteLine($"-- {pluginName} --");

                IPlugIn plugin = plugins.OrderBy(p => Int32.Parse(p.OrderId)).FirstOrDefault(c => c.Name == pluginName);
                if (plugin == null)
                {
                    Console.WriteLine("No such plugin is known.");
                    return null;
                }

                Console.WriteLine("Running...");

                foreach(string function in argParser.GetValues(ArgumentsConfig.FunctionName))
                {
                    PlugInReturnData task = plugin.ExecuteFunction(function, null);
                }

                //Task<PlugInReturnData> task0 = plugin.ExecuteFunctionAsync(argParser.GetValues(ArgumentsConfig.FunctionName)[0], null);
                //Task<PlugInReturnData> task1 = plugin.ExecuteFunctionAsync(argParser.GetValues(ArgumentsConfig.FunctionName)[1], null);

                //var Tasks = new List<Task> { task0, task1 };
                //Task.WhenAll(Tasks);
                Console.WriteLine("Plugin.Tasks complete");

                //while (Tasks.Count > 0)
                //{

                //    //Task<PlugInReturnData> finishedTask = (Task<PlugInReturnData>)await Task.WhenAny<PlugInReturnData>(Tasks, Task.Delay(5000));
                //    if (finishedTask == task0)
                //    {
                //        Console.WriteLine("Plugin complete");
                //    }
                //    else if (finishedTask == task1)
                //    {
                //        Console.WriteLine("Plugin complete");
                //    }
                //    Tasks.Remove(finishedTask);
                //}
                //task.RunSynchronously();
                //if (!tasks.IsCompletedSuccessfully)
                //{
                //    Console.WriteLine("Plugin failed");

                //    Exception ex = task.Exception;
                //    while (ex != null)
                //    {
                //        Console.WriteLine(ex.Message);
                //        ex = ex.InnerException;
                //    }
                //}

                //Task<PlugInReturnData> task = plugin.ExecuteFunction("", null);
                //try
                //{
                //    List<PlugInReturnData> task = plugin.ExecuteFunctions(argParser.GetValues(ArgumentsConfig.FunctionName), null);
                //    //PlugInReturnData task = plugin.ExecuteFunction(argParser.GetValues(ArgumentsConfig.FunctionName), null);
                //    Console.WriteLine("Plugin complete");
                //}
                //catch(PlugInException pex)
                //{
                //    Console.WriteLine($"Plugin failed: {pex.Message}");

                //}
                //task.RunSynchronously();
                //if (!task.IsCompletedSuccessfully)
                //if (task.Is == "")
                //{

                //    Exception ex = task.Exception;
                //    while (ex != null)
                //    {
                //        Console.WriteLine(ex.Message);
                //        ex = ex.InnerException;
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("Plugin complete");
                //}
            }
            return null;

        }

        public static void UnloadPlugins(ref IEnumerable<IPlugIn> plugins)
        {
            foreach (IPlugIn plugin in plugins)
            {
                plugin.OnUnload();
            }
            plugins = new List<IPlugIn>();
        }

        static Assembly LoadPluginAssembly(string pluginLocation)
        {
            Console.WriteLine($"Loading plugins from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<IPlugIn> CreateAllPluginsInAssembly(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IPlugIn).IsAssignableFrom(type))
                {
                    IPlugIn result = Activator.CreateInstance(type) as IPlugIn;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements IPlugin in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}
