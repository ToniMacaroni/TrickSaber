using BS_Utils.Utilities;
using IPA;
using IPA.Config.Stores;
using TrickSaber.UI;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {

        public static Logger Log { get; set; }

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
        }
    }
}