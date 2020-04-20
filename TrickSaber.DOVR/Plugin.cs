using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BS_Utils.Utilities;
using DynamicOpenVR;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Config = IPA.Config.Config;
using Log = IPA.Logging.Logger;

namespace TrickSaber.DOVR
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {

        public static Log Log { get; set; }
        public static Harmony Harmony { get; set; }

        private readonly string _actionManifestPath = Path.Combine(Environment.CurrentDirectory, "DynamicOpenVR", "action_manifest.json");

        [Init]
        public Plugin(Log logger, Config config)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnStart()
        {
            try
            {
                OpenVRUtilities.Init();
            }
            catch (Exception ex)
            {
                Log.Error("Failed to initialize OpenVR API");
                Log.Error(ex);
                return;
            }
            Log.Info("Successfully initialized OpenVR API");

            if (!OpenVRActionManager.instance.initialized) OpenVRActionManager.instance.Initialize(_actionManifestPath);
            Log.Info("Initialized ActionManager");

            TrickSaberPlugin.Create();
            SettingsUI.CreateMenu();
            BSEvents.gameSceneLoaded += GameplayManager.OnGameSceneLoaded;
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
        }

        public static void OnMenuSceneLoadedFresh()
        {
        }
    }
}