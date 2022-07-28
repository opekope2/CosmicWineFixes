using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private bool clipboardFixEnabled = true;
        private int threadExecutionIntervalMs = 50;
        private int maxCopyRetries = 5;

        private bool logOpeningEnabled = false;

        public bool Enabled
        {
            get => enabled;
            set => SetValue(ref enabled, value);
        }

        public bool ClipboardFixEnabled
        {
            get => clipboardFixEnabled;
            set => SetValue(ref clipboardFixEnabled, value);
        }

        public int ThreadExecutionIntervalMs
        {
            get => threadExecutionIntervalMs;
            set => SetValue(ref threadExecutionIntervalMs, value);
        }

        public int MaxCopyRetries
        {
            get => maxCopyRetries;
            set => SetValue(ref maxCopyRetries, value);
        }

        public bool LogOpeningEnabled
        {
            get => logOpeningEnabled;
            set => SetValue(ref logOpeningEnabled, value);
        }
    }
}
