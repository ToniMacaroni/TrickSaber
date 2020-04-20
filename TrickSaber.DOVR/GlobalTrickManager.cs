using System.Collections;
using IPA.Utilities;
using UnityEngine;

namespace TrickSaber.DOVR
{
    public class GlobalTrickManager : MonoBehaviour
    {
        public static GlobalTrickManager Instance;

        private Coroutine _applySlowmoCoroutine;
        private Coroutine _EndSlowmoCoroutine;

        private bool _slowmoApplied;
        private float _slowmoStepAmount;

        public AudioTimeSyncController AudioTimeSyncController;

        public SaberTrickManager LeftSaberSaberTrickManager;
        public SaberTrickManager RightSaberSaberTrickManager;

        private void Awake()
        {
            Instance = this;
            _slowmoStepAmount = PluginConfig.Instance.SlowmoStepAmount;
        }

        public void OnTrickStarted(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw && PluginConfig.Instance.SlowmoDuringThrow && !_slowmoApplied)
            {
                if (_EndSlowmoCoroutine != null) StopCoroutine(_EndSlowmoCoroutine);
                _applySlowmoCoroutine = StartCoroutine(ApplySlowmoSmooth(PluginConfig.Instance.SlowmoMultiplier));
                _slowmoApplied = true;
            }
        }

        public void OnTrickEndRequsted(TrickAction trickAction)
        {
            if (trickAction == TrickAction.Throw)
                if (PluginConfig.Instance.SlowmoDuringThrow &&
                    !IsTrickInState(trickAction, TrickState.Started) && _slowmoApplied)
                {
                    StopCoroutine(_applySlowmoCoroutine);
                    _EndSlowmoCoroutine = StartCoroutine(EndSlowmoSmooth());
                    _slowmoApplied = false;
                }
        }

        public void OnTrickEnded(TrickAction trickAction)
        {
        }

        private IEnumerator ApplySlowmoSmooth(float multiplier)
        {
            float timeScale = 1;
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            while (timeScale > multiplier)
            {
                timeScale -= _slowmoStepAmount;
                AudioTimeSyncController.SetField("_timeScale", timeScale);
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
            float timeScale = AudioTimeSyncController.timeScale;
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            while (timeScale < 1f)
            {
                timeScale += _slowmoStepAmount;
                AudioTimeSyncController.SetField("_timeScale", timeScale);
                audioSource.pitch = timeScale;
                yield return new WaitForFixedUpdate();
            }
        }

        private void EndSlowmo()
        {
            var audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            AudioTimeSyncController.SetField("_timeScale", 1f);
            audioSource.pitch = 1f;
        }

        public bool IsTrickInState(TrickAction trickAction, TrickState state)
        {
            return LeftSaberSaberTrickManager.IsTrickInState(trickAction, state) ||
                   RightSaberSaberTrickManager.IsTrickInState(trickAction, state);
        }
    }
}