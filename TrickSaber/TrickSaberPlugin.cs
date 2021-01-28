using System;
using System.Collections;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SemVer;
using SiraUtil;
using SiraUtil.Tools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using Zenject;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Version = SemVer.Version;

namespace TrickSaber
{
    class TrickSaberPlugin : IInitializable
    {
        public bool Initialized;

        public string ControllerModel;
        public bool IsKnucklesController => ControllerModel.Contains("Knuckles");

        public Version Version;
        public Version RemoteVersion;
        public bool IsNewestVersion = true;

        private readonly SiraLog _logger;
        private readonly WebClient _webClient;

        public TrickSaberPlugin(SiraLog logger, WebClient webClient)
        {
            _logger = logger;
            _webClient = webClient;
        }

        public async void Initialize()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Version = new Version(ver.Major, ver.Minor, ver.Build);
            await CheckVersion();

            ControllerModel = GetControllerName();
            Initialized = true;

            _logger.Debug($"TrickSaber version {Version.GetVersionString()} started");
        }

        private async Task CheckVersion()
        {
            try
            {
                var response = await _webClient.GetAsync("https://api.github.com/repos/ToniMacaroni/TrickSaber/releases",
                    CancellationToken.None);

                var releases = response.ContentToJson<Release[]>();

                RemoteVersion = new Version(releases[0].TagName);
                IsNewestVersion = new Range($"<={Version}").IsSatisfied(RemoteVersion);

                _logger.Info($"Retrieved remote version ({RemoteVersion})");
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
        }

        public string GetControllerName()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (!device.isValid) device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (!device.isValid) return "";
            return device.name;
        }

        internal class Release
        {
            [JsonProperty("tag_name")] public string TagName;
        }
    }
}
