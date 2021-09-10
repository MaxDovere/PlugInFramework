using Newtonsoft.Json;
using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PlugInJson
{
    public class PlugInJson : IPlugIn
    {
        public string Name => "json";

        public string Description => "Outputs JSON value.";

        public string OrderId => "2";

        private struct Info
        {
            public string JsonVersion;
            public string JsonLocation;
            public string Machine;
            public string User;
            public DateTime Date;
        }

        public void OnLoad(PlugInConfig config)
        {
            Console.WriteLine($"Event OnLoading {this}");
        }

        public void OnUnload()
        {
            Console.WriteLine($"Event OnUnLoading {this}");
        }

        public PlugInReturnData ExecuteFunction(string namefunction, PlugInSettings settings)
        {
            Console.WriteLine($"Event executeFunction {this}");

            switch (namefunction.ToLower())
            {
                case "importleads":
                    ImportLeads();
                    break;
                default:
                    break;
            }

            return null;
        }
        private void ImportLeads()
        {
            Console.WriteLine($"ImportLeads: Event World Task {this}");

            Assembly jsonAssembly = typeof(JsonConvert).Assembly;
            Info info = new Info()
            {
                JsonVersion = jsonAssembly.FullName,
                JsonLocation = jsonAssembly.Location,
                Machine = Environment.MachineName,
                User = Environment.UserName,
                Date = DateTime.Now
            };

            Console.WriteLine(JsonConvert.SerializeObject(info, Formatting.Indented));
        }
    }
}