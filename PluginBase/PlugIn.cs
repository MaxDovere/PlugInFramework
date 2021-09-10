using PlugInBase.Abstractions;
using PlugInBase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlugInBase
{
    public class PlugIn : IPlugIn
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public string OrderId => throw new NotImplementedException();

        public PlugInReturnData ExecuteFunction(string namefunction, PlugInSettings settings)
        {
            throw new NotImplementedException();
        }

        public Task<PlugInReturnData> ExecuteFunctionAsync(string namefunction, PlugInSettings settings)
        {
            throw new NotImplementedException();
        }

        public List<PlugInReturnData> ExecuteFunctions(List<string> namefunctions, List<PlugInSettings> settings)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlugInReturnData>> ExecuteFunctionsAsync(List<string> namefunctions, List<PlugInSettings> settings)
        {
            throw new NotImplementedException();
        }

        public void OnLoad(PlugInConfig config)
        {
            throw new NotImplementedException();
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

    }
}
