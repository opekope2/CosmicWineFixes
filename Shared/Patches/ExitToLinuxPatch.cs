using System.Text;
using HarmonyLib;
using Shared.Config;
using Shared.Logging;
using Shared.Plugin;

namespace Shared.Patches
{
    [HarmonyPatch(typeof(StringBuilder))]
    public static class ExitToLinuxPatch
    {
        private static ClientPlugin.Plugin Plugin => Common.Plugin as ClientPlugin.Plugin;
        private static IPluginConfig Config => Common.Config;
        public static IPluginLogger Log => Common.Logger;

        [HarmonyPatch(MethodType.Constructor, new[] { typeof(string), typeof(int), typeof(int), typeof(int) })]
        private static bool Prefix(ref string value, ref int length)
        {
            if (!Config.Enabled || !Config.ShowExitToLinux)
            {
                return true;
            }

            if (value == "Exit to Windows")
            {
                value = "Exit to GNU+Linux";
                length = value.Length;
            }

            // InGameExit mod
            if (value == "Exit To Windows")
            {
                value = "Exit To GNU+Linux";
                length = value.Length;
            }

            return true;
        }
    }
}
