using BS_Utils.Gameplay;
using UnityEngine;

namespace TrickSaber
{
    public class GameplayManager
    {
        public static void DisableScoreSubmissionIfNeeded()
        {
            if (PluginConfig.Instance.SlowmoDuringThrow) ScoreSubmission.DisableSubmission("TrickSaber");
        }

        public static void OnGameSceneLoaded()
        {
            DisableScoreSubmissionIfNeeded();

            var globalTrickManager = new GameObject("GlobalTrickManager").AddComponent<GlobalTrickManager>();
            globalTrickManager.AudioTimeSyncController = Object.FindObjectOfType<AudioTimeSyncController>();

            GameObject leftSaber = GameObject.Find("GameCore/Origin/VRGameCore/LeftSaber");
            GameObject rightSaber = GameObject.Find("GameCore/Origin/VRGameCore/RightSaber");

            SaberTrickManager leftManager = new GameObject("TrickManager_LeftSaber").AddComponent<SaberTrickManager>();
            leftManager.Saber = leftSaber.GetComponent<Saber>();
            leftManager.Controller = leftSaber.GetComponent<VRController>();

            SaberTrickManager rightManager =
                new GameObject("TrickManager_RightSaber").AddComponent<SaberTrickManager>();
            rightManager.Saber = rightSaber.GetComponent<Saber>();
            rightManager.Controller = rightSaber.GetComponent<VRController>();
        }
    }
}