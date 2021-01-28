using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Tags;
using HMUI;
using SiraUtil.Tools;
using TrickSaber.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace TrickSaber
{
    internal class GameplayManager : IInitializable
    {
        private readonly PluginConfig _config;
        private readonly SiraLog _logger;

        public GameplayManager(PluginConfig config, SiraLog logger)
        {
            _config = config;
            _logger = logger;
        }

        public void DisableScoreSubmissionIfNeeded()
        {
            // TODO: if (_config.SlowmoDuringThrow) disable score submission
        }

        public void Initialize()
        {
            DisableScoreSubmissionIfNeeded();

            try
            {
                CreateCheckbox();
            }
            catch
            {
                _logger.Warning($"No checkbox for you sir");
            }
        }

        public void CreateCheckbox()
        {
            var canvas = GameObject.Find("Wrapper/StandardGameplay/PauseMenu/Wrapper/MenuWrapper/Canvas").GetComponent<Canvas>();
            if (!canvas) return;

            var toggleObject = new ToggleSettingTag().CreateObject(canvas.transform);

            (toggleObject.transform as RectTransform).anchoredPosition = new Vector2(26, -15);
            (toggleObject.transform as RectTransform).sizeDelta = new Vector2(-130, 7);

            toggleObject.transform.Find("NameText").GetComponent<CurvedTextMeshPro>().text = "Tricksaber Enabled";

            var toggleSetting = toggleObject.GetComponent<ToggleSetting>();
            toggleSetting.Value = _config.TrickSaberEnabled;
            toggleSetting.toggle.onValueChanged.AddListener(enabled => { _config.TrickSaberEnabled = enabled; });
        }
    }
}