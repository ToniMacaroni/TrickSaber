using System;
using System.Collections;
using System.Reflection;
using OVRSimpleJSON;
using SemVer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using Version = SemVer.Version;

namespace TrickSaber
{
    class TrickSaberPlugin : MonoBehaviour
    {
        public static TrickSaberPlugin Instance;
        public static bool Initialized;

        public static string ControllerModel;
        public static bool IsKnucklesController => ControllerModel.Contains("Knuckles");

        public static Version Version;
        public static Version RemoteVersion;
        public static bool IsNewestVersion = true;

        public static void Create()
        {
            if (Instance != null) return;
            var obj = new GameObject("TrickSaberPlugin");
            Instance = obj.AddComponent<TrickSaberPlugin>();
            DontDestroyOnLoad(obj);
        }

        IEnumerator Start()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Version = new Version(ver.Major, ver.Minor, ver.Build);
            yield return StartCoroutine(CheckVersion());
            ControllerModel = GetControllerName();
            Initialized = true;
            Plugin.Log.Debug($"TrickSaber version {Version.GetVersionString()} started");
        }

        public IEnumerator CheckVersion()
        {
            UnityWebRequest www = UnityWebRequest.Get("https://api.github.com/repos/ToniMacaroni/TrickSaber/releases");
            www.SetRequestHeader("User-Agent", "TrickSaber-" + Version.GetVersionString());
            www.timeout = 10;
            yield return www.SendWebRequest();
            try
            {
                if (!www.isNetworkError && !www.isHttpError)
                {
                    JSONNode releases = JSON.Parse(www.downloadHandler.text);
                    JSONNode latestRelease = releases[0];
                    JSONNode jsonnode = latestRelease["tag_name"];
                    string githubVerStr = (jsonnode != null) ? jsonnode.Value : null;
                    RemoteVersion = new Version(githubVerStr);
                    IsNewestVersion = !new Range($">{Version}").IsSatisfied(RemoteVersion);
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("couldn't get version: " + ex.Message);
            }
        }

        public static string GetControllerName()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (!device.isValid) device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (!device.isValid) return "";
            return device.name;
        }
    }
}
