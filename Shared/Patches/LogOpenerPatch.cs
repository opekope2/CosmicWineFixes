using System.Diagnostics;
using HarmonyLib;
using Sandbox;
using Shared.Config;
using Shared.Logging;
using Shared.Plugin;

namespace Shared.Patches
{
    [HarmonyPatch(typeof(MyInitializer))]
    public static class LogOpenerPatch
    {
        private static ClientPlugin.Plugin Plugin => Common.Plugin as ClientPlugin.Plugin;
        private static IPluginConfig Config => Common.Config;
        public static IPluginLogger Log => Common.Logger;

        [HarmonyPostfix]
        [HarmonyPatch("OnCrash")]
        private static void OnCrashPostfix(string logPath)
        {
            if (!Config.Enabled || !Config.LogOpeningEnabled)
            {
                return;
            }

            Process.Start("notepad.exe", logPath);
        }
    }
}
