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
        private PlugInEventHandler<IPlugIn, string, PlugInEventArgs> _HandlerEventListNotifier;


        public PlugInJson()
        {
            //EventListNotifier += HandlePlugInChange;
        }

        event PlugInEventHandler<IPlugIn, string, PlugInEventArgs> IPlugIn.PlugInNotifier
        {
            add
            {
                _HandlerEventListNotifier += value;
            }

            remove
            {
                _HandlerEventListNotifier -= value;
            }
        }

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
        //public void HandlePlugInChange(IPlugIn plugin, PlugInEventArgs args) 
        //{
        //    string mes = $"Id: {args.Id}- Message: {args.Message}";
        //    Console.WriteLine($"Setting Name {plugin.Name}: {mes}");
        //}
        public void OnLoad(PlugInConfig config)
        {
            string message = $"Event OnLoading {this}";

            _HandlerEventListNotifier(this, message, null);
        }

        public void OnUnload()
        {
            string message = $"Event OnUnLoading {this}";
            _HandlerEventListNotifier(this, message, null);
        }

        public PlugInReturnData<T> ExecuteFunction<T>(string namefunction, PlugInEventArgs args)
        {
            _HandlerEventListNotifier(this, $"Event executeFunction {this}", null);

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
            _HandlerEventListNotifier(this, $"ImportLeads: Event World Task {this}", null);

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