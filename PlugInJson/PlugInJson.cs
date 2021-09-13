using Newtonsoft.Json;
using PlugInBase;
using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;
using System.Reflection;

namespace PlugInJson
{
    public class PlugInJson : IPlugIn
    {
        public event DelegateHandlers.PlugInEventHandler<IPlugIn, string, PlugInEventArgs> PlugInNotifier;

        public string Name => "PlugInJson";

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
            string message = $"Event OnLoading {this}";

            PlugInNotifier(this, message, null);
        }

        public void OnUnload()
        {
            string message = $"Event OnUnLoading {this}";
            PlugInNotifier(this, message, null);
        }

        public PlugInReturnData<T> ExecuteFunction<T>(string namefunction, PlugInEventArgs args)
        {
            PlugInNotifier(this, $"Event executeFunction {this}", null);

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

        public string Add(string name, string name2)  // Object[] parms)
        {
            return $"{Name}.{name}.{name2}";
        }
        private void ImportLeads()
        {
            PlugInNotifier(this, $"ImportLeads: Event World Task {this}", null);

            Assembly jsonAssembly = typeof(JsonConvert).Assembly;
            Info info = new Info()
            {
                JsonVersion = jsonAssembly.FullName,
                JsonLocation = jsonAssembly.Location,
                Machine = Environment.MachineName,
                User = Environment.UserName,
                Date = DateTime.Now
            };
            string message = JsonConvert.SerializeObject(info, Formatting.Indented);
            PlugInNotifier(this, $"ImportLeads: Event World Task the end {message}", null);
        }

    }
}