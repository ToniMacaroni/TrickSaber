using System;
using System.Collections;
using System.Net;
using System.Reflection;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using OVRSimpleJSON;
using SemVer;
using TMPro;
using TrickSaber.UI;
using UnityEngine.Networking;
using UnityEngine.XR;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;
using Version = SemVer.Version;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        public static string ControllerModel;

        public static Version Version;
        public static string VersionString;

        public static Logger Log { get; set; }
        public static Harmony Harmony { get; set; }
        public static bool IsControllerSupported => !ControllerModel.Contains("Knuckles");

        [Init]
        public Plugin(Logger logger, Config config)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Version = new Version(ver.Major, ver.Minor, ver.Build);
            VersionString = Version.Major + "." + Version.Minor + "." + Version.Patch;
        }

        [OnStart]
        public void OnStart()
        {
            TrickSaberPlugin.Create();
            SettingsUI.CreateMenu();
            BSEvents.gameSceneLoaded += GameplayManager.OnGameSceneLoaded;
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
            Log.Debug($"TrickSaber version {VersionString} started");
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