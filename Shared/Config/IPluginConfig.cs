using System.ComponentModel;

namespace Shared.Config
{
    public interface IPluginConfig : INotifyPropertyChanged
    {
        bool Enabled { get; set; }

        bool ClipboardFixEnabled { get; set; }
        int ThreadExecutionIntervalMs { get; set; }
        int MaxCopyRetries { get; set; }

        bool LogOpeningEnabled { get; set; }

        bool ShowExitToLinux { get; set; }
    }
}
