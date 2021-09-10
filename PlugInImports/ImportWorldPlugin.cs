using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlugInImports
{
    public class ImportWorldPlugin : IPlugIn
    {
        public string Name => "ImportWorldPlugin";

        public string Description => "Simple example plugin";

        public string OrderId => "1";

        public PlugInReturnData ExecuteFunction(string namefunction, PlugInSettings settings)
        {
            Console.WriteLine($"ExecuteFunction: Import World Task {this}");
            return new PlugInReturnData(namefunction, null);
        }
        public void OnLoad(PlugInConfig config)
        {
            Console.WriteLine("Import World plugin OnLoad");
        }

        public void OnUnload()
        {
            Console.WriteLine("Import World plugin OnUnload");
        }

    }
}
