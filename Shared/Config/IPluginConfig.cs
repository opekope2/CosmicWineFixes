using System.ComponentModel;

namespace Shared.Config
{
    public interface IPluginConfig : INotifyPropertyChanged
    {
        // Enables the plugin
        bool Enabled { get; set; }

        int ThreadExecutionIntervalMs { get; set; }

        int MaxCopyRetries { get; set; }
    }
}