using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlugInImports
{
    public class ExportWorldPlugin : IPlugIn
    {
        public string Name => "ExportWorldPlugin";

        public string Description => "Simple example plugin";

        public string OrderId => "3";

        public  PlugInReturnData ExecuteFunction(string namefunction, PlugInSettings settings)
        {
            Console.WriteLine($"ExecuteFunction: Export World Task {this}");
            
            switch (namefunction.ToLower())
            {
                case "importleads":
                    ImportEstimated();
                    break;
                default:
                    break;
            }

            return new PlugInReturnData(namefunction, null);
        }
        private void ImportEstimated()
        {
            Console.WriteLine($"ImportEstimated: Export World Task {this}");
        }
        public void OnLoad(PlugInConfig config)
        {
            Console.WriteLine("Export World plugin OnLoad");
        }

        public void OnUnload()
        {
            Console.WriteLine("Export World plugin OnUnload");
        }

    }
}
