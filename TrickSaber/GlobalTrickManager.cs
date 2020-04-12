using System.Collections;
using IPA.Utilities;
using UnityEngine;

namespace TrickSaber
{
    public class GlobalTrickManager : MonoBehaviour
    {
        public static GlobalTrickManager Instance;

        public SaberTrickManager LeftSaberSaberTrickManager;
        public SaberTrickManager RightSaberSaberTrickManager;

        public AudioTimeSyncController AudioTimeSyncController;
        private bool _slowmoApplied;
        private bool _slowmoEndRequested;

        void Awake()
        {
            Instance = this;
        }

        public void OnTrickStarted(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw && PluginConfig.Instance.SlowmoDuringThrow)
                StartCoroutine(ApplySlowmoSmooth(PluginConfig.Instance.SlowmoMultiplier));
        }

        public void OnTrickEnded(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw)
                if (PluginConfig.Instance.SlowmoDuringThrow &&
                    !LeftSaberSaberTrickManager.IsDoingTrick(trickAction) &&
                    !RightSaberSaberTrickManager.IsDoingTrick(trickAction))
                {
                    _slowmoEndRequested = true;
                    EndSlowmo();
                }
        }

        private IEnumerator ApplySlowmoSmooth(float multiplier)
        {
            if (!_slowmoApplied)
            {
                float timeScale = 1;
                var audioSource =
                    AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                while (timeScale > multiplier)
                {
                    if (_slowmoEndRequested)
                    {
                        EndSlowmo();
                        yield break;
                    }
                    timeScale -= 0.02f;
                    AudioTimeSyncController.SetField("_timeScale",timeScale);
                    audioSource.pitch = timeScale;
                    yield return new WaitForFixedUpdate();
                }
                _slowmoApplied = true;
            }
        }

        private void ApplySlowmo(float multiplier)
        {
            if (_slowmoApplied) return;
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            AudioTimeSyncController.SetField("_timeScale", multiplier);
            audioSource.pitch = multiplier;
            _slowmoApplied = true;
        }

        private IEnumerator EndSlowmoSmooth()
        {
            if (_slowmoApplied)
            {
                _slowmoApplied = false;
                float timeScale = AudioTimeSyncController.timeScale;
                var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                while (timeScale<1f)
                {
                    timeScale += 0.02f;
                    AudioTimeSyncController.SetField("_timeScale", timeScale);
                    audioSource.pitch = timeScale;
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private void EndSlowmo()
        {
            if (!_slowmoApplied)return;
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            AudioTimeSyncController.SetField("_timeScale", 1f);
            audioSource.pitch = 1f;
            _slowmoEndRequested = false;
            _slowmoApplied = false;
        }
    }
}
