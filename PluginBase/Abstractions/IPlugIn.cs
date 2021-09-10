using PlugInBase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlugInBase.Abstractions
{
    public interface IPlugIn
    {
        string Name { get; }
        string Description { get; }
        string OrderId { get; }

        void OnLoad(PlugInConfig config);
        void OnUnload();
        PlugInReturnData ExecuteFunction(string namefunction, PlugInSettings settings);
    }
}