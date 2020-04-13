using System;
using System.Reflection;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using UnityEngine.XR;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        public static string ControllerModel;

        [Init]
        public Plugin(Logger logger, Config config)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        public static string Version
        {
            get
            {
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;
                return ver.Major + "." + ver.Minor + "." + ver.Build;
            }
        }

        public static Logger Log { get; set; }
        public static Harmony Harmony { get; set; }
        public static bool IsControllerSupported => !ControllerModel.Contains("Knuckles");

        [OnStart]
        public void OnStart()
        {
            SettingsUI.CreateMenu();
            BSEvents.gameSceneLoaded += GameplayManager.OnGameSceneLoaded;
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
            Log.Debug($"TrickSaber version {Version} started");
        }

        public static string GetControllerName()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (!device.isValid) device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (!device.isValid) return "";
            return device.name;
        }

        public static void OnMenuSceneLoadedFresh()
        {
            ControllerModel = GetControllerName();
            if (!IsControllerSupported)
            {
                PluginConfig.Instance.GripAction = TrickAction.None.ToString();
                PluginConfig.Instance.ThumbstickAction = TrickAction.None.ToString();
            }
        }
    }
}