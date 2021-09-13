using System;

namespace PlugInFramework.Models
{
    public class PlugInReturnData<T>
    {
        public string Name;
        public byte[] Data;
        public string Base64Data => Convert.ToBase64String(Data);
        
        public PlugInReturnData(string name, byte[] data)
        {
            Name = name;
            Data = data;
    }
    }
}
