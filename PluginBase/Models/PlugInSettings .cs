using System;

namespace PlugInBase.Models
{
    public class PlugInSettings
    {
        public string Name;
        public byte[] Data;
        public string Base64Data => Convert.ToBase64String(Data);

        public PlugInSettings(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        public PlugInSettings()
        {
        }
    }
}
