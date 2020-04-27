using System.Linq;
using BS_Utils.Gameplay;
using UnityEngine;

namespace TrickSaber
{
    public class GameplayManager
    {
        public static IDifficultyBeatmap CurrentDifficultyBeatmap;

        public static void DisableScoreSubmissionIfNeeded()
        {
            if (PluginConfig.Instance.SlowmoDuringThrow) ScoreSubmission.DisableSubmission("TrickSaber");
        }

        public static void OnGameSceneLoaded()
        {
            if (!BS_Utils.Plugin.LevelData.IsSet) Plugin.Log.Debug("LevelData not set!");
            else CurrentDifficultyBeatmap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap;

            DisableScoreSubmissionIfNeeded();

            var globalTrickManager = new GameObject("GlobalTrickManager").AddComponent<GlobalTrickManager>();
            globalTrickManager.AudioTimeSyncController = Object.FindObjectOfType<AudioTimeSyncController>();

            Object.FindObjectsOfType<Saber>().ToList().ForEach(saber=>saber.gameObject.AddComponent<SaberTrickManager>());
        }
    }
}