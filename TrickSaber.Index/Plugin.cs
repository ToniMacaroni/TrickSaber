using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;

namespace TrickSaber.Index
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {

        public static Logger Log { get; set; }
        public static Harmony Harmony { get; set; }

        [Init]
        public Plugin(Logger logger, Config config)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnStart()
        {
            TrickSaberPlugin.Create();
            SettingsUI.CreateMenu();
            BSEvents.gameSceneLoaded += GameplayManager.OnGameSceneLoaded;
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
        }

        public static void OnMenuSceneLoadedFresh()
        {
            if (!TrickSaberPlugin.IsControllerSupported)
            {
                PluginConfig.Instance.GripAction = TrickAction.None.ToString();
                PluginConfig.Instance.ThumbstickAction = TrickAction.None.ToString();
            }
        }
    }
}