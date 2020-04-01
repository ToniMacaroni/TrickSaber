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
using UnityEngine.XR;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        public static Logger Log { get; set; }
        public static Harmony Harmony { get; set; }

        public static string ControllerModel;
        public static bool IsControllerSupported => ControllerModel != "OpenVR Controller(Knuckles Left) - Left";

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
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += GameplayManager.OnGameSceneLoaded;
            BS_Utils.Utilities.BSEvents.menuSceneLoadedFresh += GameplayManager.OnMenuSceneLoadedFresh;
            Log.Debug("TrickSaber Started");
            ControllerModel = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).name;
            Log.Debug("Controller Model: "+ControllerModel);
            if (!IsControllerSupported)
            {
                PluginConfig.Instance.GripAction = TrickAction.None.ToString();
                PluginConfig.Instance.ThumbstickAction = TrickAction.None.ToString();
            }
        }
    }
}
