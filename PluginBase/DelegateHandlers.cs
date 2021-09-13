using PlugInBase.Abstractions;
using PlugInBase.Models;

namespace PlugInBase
{
    public class DelegateHandlers
    {
        public delegate void PlugInEventHandler<T, S, U>(IPlugIn sender, string message, PlugInEventArgs eventArgs);
    }
}
