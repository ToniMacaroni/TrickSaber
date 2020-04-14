using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OVRSimpleJSON;
using SemVer;
using UnityEngine;
using UnityEngine.Networking;
using Version = SemVer.Version;

namespace TrickSaber.UI
{
    class TrickSaberPlugin : MonoBehaviour
    {
        public static TrickSaberPlugin Instance;

        public bool IsNewestVersion = true;

        public static void Create()
        {
            Instance = new GameObject("TrickSaberPlugin").AddComponent<TrickSaberPlugin>();
        }

        void Start()
        {
            StartCoroutine(CheckVersion());
        }

        public IEnumerator CheckVersion()
        {
            UnityWebRequest www = UnityWebRequest.Get("https://api.github.com/repos/ToniMacaroni/TrickSaber/releases");
            www.SetRequestHeader("User-Agent", "TrickSaber-" + Plugin.VersionString);
            www.timeout = 10;
            yield return www.SendWebRequest();
            try
            {
                if (!www.isNetworkError && !www.isHttpError)
                {
                    JSONNode releases = JSON.Parse(www.downloadHandler.text);
                    JSONNode latestRelease = releases[0];
                    JSONNode jsonnode = latestRelease["tag_name"];
                    string githubVerStr = (jsonnode != null) ? jsonnode.Value.Replace("-L", "") : null;
                    Version githubVer = new Version(githubVerStr);
                    IsNewestVersion = !new Range($">{Plugin.Version}").IsSatisfied(githubVer);
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("couldn't get version: " + ex.Message);
            }
        }
    }
}
