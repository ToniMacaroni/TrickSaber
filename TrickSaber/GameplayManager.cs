using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Tags;
using HMUI;
using IPA.Utilities;
using SiraUtil.Services;
using SiraUtil.Tools;
using TrickSaber.Configuration;
using UnityEngine;
using Zenject;

namespace TrickSaber
{
    internal class GameplayManager : IInitializable
    {
        private readonly PluginConfig _config;
        private readonly SiraLog _logger;
        private readonly Submission _submission;
        private readonly PauseMenuManager _pauseMenuManager;

        public GameplayManager(PluginConfig config, SiraLog logger, Submission submission, [InjectOptional] PauseMenuManager pauseMenuManager)
        {
            _config = config;
            _logger = logger;
            _submission = submission;
            _pauseMenuManager = pauseMenuManager;
        }

        public void DisableScoreSubmissionIfNeeded()
        {
            foreach (var propertyInfo in typeof(PluginConfig).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if(propertyInfo.PropertyType!=typeof(bool)) continue;

                var attr = Attribute.GetCustomAttribute(
                            propertyInfo,
                            typeof(DisablesScoringAttribute))
                    as DisablesScoringAttribute;

                if(attr==null) continue;

                DisableScore(
                    (bool)propertyInfo.GetValue(_config),
                    string.IsNullOrEmpty(attr.Reason)
                        ? propertyInfo.Name
                        : attr.Reason);
            }
        }

        public void DisableScore(bool disable, string reason)
        {
            if (!disable) return;
            _submission.DisableScoreSubmission("Tricksaber", reason);
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
            if (_pauseMenuManager == null) return;

            var canvas = _pauseMenuManager.GetField<LevelBar, PauseMenuManager>("_levelBar")
                .transform
                .parent
                .parent
                .GetComponent<Canvas>();
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