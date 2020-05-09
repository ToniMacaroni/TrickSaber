using System.Linq;
using BS_Utils.Gameplay;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            CreateCheckbox();
        }

        public static void CreateCheckbox()
        {
            var canvas = GameObject.Find("Wrapper/PauseMenu/Wrapper/UI/Canvas").GetComponent<Canvas>();
            GameObject go = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(res => res.name == "Toggle");
            go = Object.Instantiate(go, canvas.transform, false);
            Object.Destroy(go.GetComponentInChildren<LocalizedTextMeshProUGUI>());
            RectTransform rect = go.transform as RectTransform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(40, 35);
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Tricksaber Enabled";
            var toggle = go.GetComponentInChildren<Toggle>();
            toggle.isOn = true;
            toggle.onValueChanged.AddListener(enabled =>
            {
                if (GlobalTrickManager.Instance)
                {
                    GlobalTrickManager.Instance.Enabled = enabled;
                }
            });
        }
    }
}