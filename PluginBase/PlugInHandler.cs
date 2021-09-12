using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;

namespace PlugInBase
{
    public delegate void PlugInEventHandler<T, S, U>(T sender, S message, U eventArgs);
    public class PlugInHandler<T>
    {
        private readonly IPlugIn _plugin;
        public event PlugInEventHandler<IPlugIn, string, PlugInEventArgs> EventNotifierPlugIn;
        public PlugInHandler(IPlugIn plugin)
        {
            _plugin = plugin;
        }
        protected virtual void OnPlugInNotifier(string message, PlugInEventArgs a)
        {
            EventNotifierPlugIn(_plugin, message, a);
        }
    }
}
