using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BS_Utils.Gameplay;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
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

            Harmony = new Harmony("tricksaber.toni.com");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnStart]
        public void OnStart()
        {
            SettingsUI.CreateMenu();
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += GameplayManager.DisableScoreSubmissionIfNeeded;
            Log.Debug("TrickSaber Started");
        }

        [OnExit]
        public void OnExit()
        {
        }
    }
}
