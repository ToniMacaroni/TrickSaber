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
        private Coroutine _slowmoCoroutine;
        private bool _slowmoApplied;

        void Awake()
        {
            Instance = this;
        }

        public void OnTrickStarted(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw && PluginConfig.Instance.SlowmoDuringThrow && !_slowmoApplied)
            {
                _slowmoCoroutine = StartCoroutine(ApplySlowmoSmooth(PluginConfig.Instance.SlowmoMultiplier));
                _slowmoApplied = true;
            }
        }

        public void OnTrickEnded(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw)
                if (PluginConfig.Instance.SlowmoDuringThrow &&
                    !LeftSaberSaberTrickManager.IsDoingTrick(trickAction) &&
                    !RightSaberSaberTrickManager.IsDoingTrick(trickAction) && _slowmoApplied)
                {
                    StopCoroutine(_slowmoCoroutine);
                    EndSlowmo();
                    _slowmoApplied = false;
                }
        }

        private IEnumerator ApplySlowmoSmooth(float multiplier)
        {
                float timeScale = 1;
                var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                while (timeScale > multiplier)
                {
                    timeScale -= 0.02f;
                    AudioTimeSyncController.SetField("_timeScale",timeScale);
                    audioSource.pitch = timeScale;
                    yield return new WaitForFixedUpdate();
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
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            AudioTimeSyncController.SetField("_timeScale", 1f);
            audioSource.pitch = 1f;
        }
    }
}
