using System;
using UnityEngine;

namespace TrickSaber
{
    public abstract class Trick : MonoBehaviour
    {
        protected MovementController MovementController;
        protected SaberTrickManager SaberTrickManager;
        protected SaberTrickModel SaberTrickModel;

        protected bool _allowTrickStart = true;
        protected bool _endRequested;
        protected bool _trickStarted;
        public float Value;

        public abstract TrickAction TrickAction { get; }
        public string Name => TrickAction.ToString();

        public event Action<TrickAction> TrickStarted;
        public event Action<TrickAction> TrickEnded;

        public void Init(SaberTrickManager saberTrickManager, MovementController movementController,
            SaberTrickModel saberTrickModel)
        {
            SaberTrickManager = saberTrickManager;
            MovementController = movementController;
            SaberTrickModel = saberTrickModel;
            OnInit();
        }

        public bool StartTrick()
        {
            if (!_trickStarted && _allowTrickStart)
            {
                _allowTrickStart = false;
                OnTrickStart();
                _trickStarted = true;
                TrickStarted?.Invoke(TrickAction);
                return true;
            }

            return false;
        }

        public void EndTrick()
        {
            if (_trickStarted) _endRequested = true;
        }

        protected void Reset()
        {
            _endRequested = false;
            _allowTrickStart = true;
            TrickEnded?.Invoke(TrickAction);
        }

        private void Update()
        {
            if (_trickStarted)
                if (!_endRequested)
                {
                    OnTrickUpdate();
                }
                else
                {
                    _trickStarted = false;
                    OnTrickEndRequested();
                }
        }

        public abstract void OnTrickStart();

        public abstract void OnTrickUpdate();

        public abstract void OnTrickEndRequested();

        public abstract void OnInit();
    }
}