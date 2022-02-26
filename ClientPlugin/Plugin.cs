using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ClientPlugin.GUI;
using HarmonyLib;
using Sandbox.Graphics.GUI;
using Shared.Config;
using Shared.Logging;
using Shared.Patches;
using Shared.Plugin;
using VRage.FileSystem;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin, ICommonPlugin
    {
        public const string Name = "SeThreadingFixes";
        public static Plugin Instance { get; private set; }

        public long Tick { get; private set; }

        public IPluginLogger Log => Logger;
        private static readonly IPluginLogger Logger = new PluginLogger(Name);

        public IPluginConfig Config => config?.Data;
        private PersistentConfig<PluginConfig> config;
        private static readonly string ConfigFileName = $"{Name}.cfg";

        private static readonly object InitializationMutex = new object();
        private static bool initialized;
        private static bool failed;

        private bool stopThread = false;
        private Thread staThread;
        private Queue<Action> threadActionQueue = new Queue<Action>();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            Log.Info("Loading");

            var configPath = Path.Combine(MyFileSystem.UserDataPath, ConfigFileName);
            config = PersistentConfig<PluginConfig>.Load(Log, configPath);

            Common.SetPlugin(this);

            if (!PatchHelpers.HarmonyPatchAll(Log, new Harmony(Name)))
            {
                failed = true;
                return;
            }

            Log.Debug("Successfully loaded");
        }

        public void Dispose()
        {
            try
            {
                Log.Debug("Stopping STAThread");
                stopThread = true;
                staThread.Join();
                Log.Debug("Stopped STAThread");
                // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.
            }
            catch (Exception ex)
            {
                Log.Critical(ex, "Dispose failed");
            }

            Instance = null;
        }

        public void Update()
        {
            EnsureInitialized();
            try
            {
                if (!failed)
                {
                    CustomUpdate();
                    Tick++;
                }
            }
            catch (Exception ex)
            {
                Log.Critical(ex, "Update failed");
                failed = true;
            }
        }

        private void EnsureInitialized()
        {
            lock (InitializationMutex)
            {
                if (initialized || failed)
                    return;

                Log.Info("Initializing");
                try
                {
                    Initialize();
                }
                catch (Exception ex)
                {
                    Log.Critical(ex, "Failed to initialize plugin");
                    failed = true;
                    return;
                }

                Log.Debug("Successfully initialized");
                initialized = true;
            }
        }

        private void Initialize()
        {
            Log.Debug("Starting STAThread");
            staThread = new Thread(() =>
            {
                while (!stopThread)
                {
                    while (threadActionQueue.Count > 0)
                    {
                        Log.Debug("Executing action on STAThread");
                        Action a = threadActionQueue.Dequeue();
                        a?.Invoke();
                    }
                    Thread.Sleep(Config.ThreadExecutionIntervalMs);
                }
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
        }

        private void CustomUpdate()
        {
            // Nothing to do in every tick
        }

        public void ExecuteActionOnStaThread(Action action)
        {
            threadActionQueue.Enqueue(action);
        }

        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyPluginConfigDialog());
        }
    }
}