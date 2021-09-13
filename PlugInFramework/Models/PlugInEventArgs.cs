
namespace PlugInFramework.Models
{
    public class PlugInEventArgs : System.EventArgs 
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public PlugInSettings Settings { get; set; } = new PlugInSettings();
    }
}
