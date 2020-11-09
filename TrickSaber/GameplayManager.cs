using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Tags;
using BS_Utils.Gameplay;
using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TrickSaber
{
    internal class GameplayManager
    {
        public static IDifficultyBeatmap CurrentDifficultyBeatmap;

        public static void DisableScoreSubmissionIfNeeded()
        {
            if (PluginConfig.Instance.SlowmoDuringThrow) ScoreSubmission.DisableSubmission("TrickSaber");
        }

        public static void OnGameSceneLoaded()
        {
            //UI for debugging purposes
            //ModUI.Create();

            if (!BS_Utils.Plugin.LevelData.IsSet) Plugin.Log.Debug("LevelData not set!");
            else CurrentDifficultyBeatmap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap;

            DisableScoreSubmissionIfNeeded();

            var globalTrickManager = new GameObject("GlobalTrickManager").AddComponent<GlobalTrickManager>();
            globalTrickManager.AudioTimeSyncController = Object.FindObjectOfType<AudioTimeSyncController>();

            Object.FindObjectsOfType<Saber>().ToList().ForEach(saber=>saber.gameObject.AddComponent<SaberTrickManager>());

            try
            {
                CreateCheckbox();
            }
            catch{}
        }

        public static void CreateCheckbox()
        {
            var canvas = GameObject.Find("Wrapper/StandardGameplay/PauseMenu/Wrapper/MenuWrapper/Canvas").GetComponent<Canvas>();
            if (!canvas) return;

            var toggleObject = new ToggleSettingTag().CreateObject(canvas.transform);

            (toggleObject.transform as RectTransform).anchoredPosition = new Vector2(26, -15);
            (toggleObject.transform as RectTransform).sizeDelta = new Vector2(-130, 7);

            toggleObject.transform.Find("NameText").GetComponent<CurvedTextMeshPro>().text = "Tricksaber Enabled";

            var toggleSetting = toggleObject.GetComponent<ToggleSetting>();
            toggleSetting.Value = PluginConfig.Instance.TrickSaberEnabled;
            toggleSetting.toggle.onValueChanged.AddListener(enabled => { PluginConfig.Instance.TrickSaberEnabled = enabled; });
        }
    }
}