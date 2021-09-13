using PlugInFramework.Models;
using PlugInFramework.PlugInBase.Abstractions;
using PlugInFramework.PlugInManager.Infrastructure;
using PlugInFramework.PlugInManager.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ArgumentsParser argParser;
                try
                {
                    argParser = new ArgumentsParser(args, new string[]
                    {
                        ArgumentsConfig.PluginLocations,
                        ArgumentsConfig.Help,
                        ArgumentsConfig.TargetPlugins,
                        ArgumentsConfig.PluginLocationsFromFile,
                        ArgumentsConfig.ActionName,
                        ArgumentsConfig.FunctionName
                    });
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(GetHelpText());
                    return;
                }

                PlugInHandlerContext.NotificationFunction = NotificationPlugInChange;

                // Inserisce la lista dei plugins dal folder previsto daio parametri
                List<string> pluginPaths = ResolvePluginPaths(argParser);

                // Load plugins and invoke OnLoad for each
                IEnumerable<IPlugIn> plugins = PlugInHandlerContext.LoadPlugins(pluginPaths.ToArray());

                PlugInHandlerContext.RunPlugins<string>(argParser, plugins);

                PlugInHandlerContext.UnloadPlugins(ref plugins);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void NotificationPlugInChange(IPlugIn plugin, string message, PlugInEventArgs args)
        {
            int id = args == null ? 0 : args.Id;
            string mes = $"Id: {id}- Message: {message}";
            Console.WriteLine($"Setting Name {plugin.Name}: {mes}");
        }
        private static List<string> ResolvePluginPaths(ArgumentsParser argParser)
        {
            // Get the plugins in the plugin folder
            List<string> pluginPaths = Directory.GetFiles(@".\Plugins\").ToList();

            // If other locations were passed add those (but add them in front so if they require re-downloading that happens first)
            List<string> locations = argParser.GetValues(ArgumentsConfig.PluginLocations);
            if (locations != null)
            {
                locations.AddRange(pluginPaths);
                pluginPaths = locations;
            }

            // If there are files to load paths from, do that
            locations = argParser.GetValues(ArgumentsConfig.PluginLocationsFromFile);
            if (locations != null)
            {
                List<string> loadedPaths = locations.SelectMany(filePath => LoadPathsFromFile(filePath)).ToList();
                // again insert in the front
                loadedPaths.AddRange(pluginPaths);
                pluginPaths = loadedPaths;
            }

            return pluginPaths;
        }

        private static List<string> LoadPathsFromFile(string filePath)
        {
            string fileName = filePath;
            if (PathTools.IsRemotePath(filePath))
            {
                string tempDir = @".\tmp\";
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

                fileName = PathTools.UrlToFilename(filePath, "txt", tempDir);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(filePath, fileName);
                }
                Console.WriteLine("Downloaded " + filePath);
            }

            return File.ReadAllLines(fileName).ToList();
        }

        private static string GetHelpText()
        {
            return "HELP:" + Environment.NewLine +
                $"\t--{ArgumentsConfig.Help} - This message" + Environment.NewLine +
                $"\t--{ArgumentsConfig.PluginLocations} - List of locations to load plugins from, can be filepath or URI" + Environment.NewLine +
                $"\t--{ArgumentsConfig.TargetPlugins} - List of plugin names to run" + Environment.NewLine +
                $"\t--{ArgumentsConfig.PluginLocationsFromFile} - List of files from which to load plugin locations. Each location should be on its own line and may be remote";
        }
    }
}