using System.Collections;
using IPA.Utilities;
using UnityEngine;

namespace TrickSaber
{
    public class GlobalTrickManager : MonoBehaviour
    {
        public static GlobalTrickManager Instance;

        private Coroutine _applySlowmoCoroutine;
        private Coroutine _EndSlowmoCoroutine;

        private bool _slowmoApplied;
        private float _slowmoStepAmount;
        private float _originalTimeScale;

        public AudioTimeSyncController AudioTimeSyncController;
        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if(_audioSource==null)
                    _audioSource = AudioTimeSyncController.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                return _audioSource;
            }
        }

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
                _applySlowmoCoroutine = StartCoroutine(ApplySlowmoSmooth(PluginConfig.Instance.SlowmoAmount));
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

        private IEnumerator ApplySlowmoSmooth(float amount)
        {
            _originalTimeScale = AudioTimeSyncController.timeScale;
            float timeScale = _originalTimeScale;
            float targetTimeScale = timeScale - amount;
            if (targetTimeScale < 0.1f) targetTimeScale = 0.1f;
            while (timeScale > targetTimeScale)
            {
                timeScale -= _slowmoStepAmount;
                SetTimescale(timeScale);
                yield return new WaitForFixedUpdate();
            }

            SetTimescale(targetTimeScale);
        }

        private IEnumerator EndSlowmoSmooth()
        {
            float timeScale = AudioTimeSyncController.timeScale;
            float targetTimeScale = _originalTimeScale;
            while (timeScale < targetTimeScale)
            {
                timeScale += _slowmoStepAmount;
                SetTimescale(timeScale);
                yield return new WaitForFixedUpdate();
            }

            SetTimescale(targetTimeScale);
        }

        void SetTimescale(float timescale)
        {
            AudioTimeSyncController.SetField("_timeScale", timescale);
            AudioSource.pitch = timescale;
        }

        public bool IsTrickInState(TrickAction trickAction, TrickState state)
        {
            return LeftSaberSaberTrickManager.IsTrickInState(trickAction, state) ||
                   RightSaberSaberTrickManager.IsTrickInState(trickAction, state);
        }
    }
}