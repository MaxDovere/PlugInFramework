using PlugInBase.Models;

namespace PlugInBase.Abstractions
{
    public interface IPlugIn
    {
        string Name { get; }
        string Description { get; }
        string OrderId { get; }

        event DelegateHandlers.PlugInEventHandler<IPlugIn, string, PlugInEventArgs> PlugInNotifier;

        void OnLoad(PlugInConfig config);
        void OnUnload();
        PlugInReturnData<T> ExecuteFunction<T>(string namefunction, PlugInEventArgs args);
    }
}