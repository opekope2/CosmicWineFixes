using System;
using System.Reflection;
using System.Windows.Forms;
using HarmonyLib;
using Shared.Config;
using Shared.Logging;
using Shared.Plugin;
using VRage.Platform.Windows.Forms;

namespace Shared.Patches
{
    [HarmonyPatch(typeof(MyClipboardHelper))]
    public static class MyClipboardHelperPatch
    {
        private static ClientPlugin.Plugin Plugin => Common.Plugin as ClientPlugin.Plugin;
        private static IPluginConfig Config => Common.Config;
        public static IPluginLogger Log => Common.Logger;


        [HarmonyPrefix]
        [HarmonyPatch(nameof(MyClipboardHelper.SetClipboard))]
        private static bool SetClipboardPrefix(ref string text)
        {
            if (!Config.Enabled || !Config.EnableClipboardFix)
            {
                return true;
            }

            string localText = text;
            Plugin.ExecuteActionOnStaThread(() =>
            {
                Log.Debug("Copying text to clipboard via fix");
                Clipboard.Clear();
                Clipboard.SetText(localText);
            });

            // Don't run the original method, run this fixed implementation instead
            return false;
        }
    }
}