using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if !TORCH

namespace Shared.Config
{
    public class PluginConfig : IPluginConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;

            OnPropertyChanged(propName);
        }

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            propertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private bool enabled = true;
        private int threadExecutionIntervalMs = 50;
        private bool enableClipboardFix = true;

        public bool Enabled
        {
            get => enabled;
            set => SetValue(ref enabled, value);
        }

        public int ThreadExecutionIntervalMs
        {
            get => threadExecutionIntervalMs;
            set => SetValue(ref threadExecutionIntervalMs, value);
        }

        public bool EnableClipboardFix
        {
            get => enableClipboardFix;
            set => SetValue(ref enableClipboardFix, value);
        }
    }
}

#endif