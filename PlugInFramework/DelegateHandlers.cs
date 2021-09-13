using PlugInFramework.Models;
using PlugInFramework.PlugInBase.Abstractions;

namespace PlugInFramework.PlugInManager
{
    public class DelegateHandlers
    {
        public delegate void PlugInEventHandler<T, S, U>(IPlugIn sender, string message, PlugInEventArgs eventArgs);
    }
}
