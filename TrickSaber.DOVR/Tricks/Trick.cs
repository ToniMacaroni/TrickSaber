using System;
using UnityEngine;

namespace TrickSaber.DOVR
{
    public abstract class Trick : MonoBehaviour
    {
        protected bool _endRequested;
        protected MovementController MovementController;
        protected SaberTrickManager SaberTrickManager;
        protected SaberTrickModel SaberTrickModel;
        public TrickState State = TrickState.Inactive;
        public float Value;

        public abstract TrickAction TrickAction { get; }
        public string Name => TrickAction.ToString();

        public event Action<TrickAction> TrickStarted;
        public event Action<TrickAction> TrickEnding;
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
            if (State == TrickState.Inactive)
            {
                State = TrickState.Started;
                OnTrickStart();
                TrickStarted?.Invoke(TrickAction);
                return true;
            }

            return false;
        }

        public void EndTrick()
        {
            if (State == TrickState.Started) _endRequested = true;
        }

        protected void Reset()
        {
            State = TrickState.Inactive;
            _endRequested = false;
            TrickEnded?.Invoke(TrickAction);
        }

        private void Update()
        {
            if (State == TrickState.Started)
                if (!_endRequested)
                {
                    OnTrickUpdate();
                }
                else
                {
                    State = TrickState.Ending;
                    TrickEnding?.Invoke(TrickAction);
                    OnTrickEndRequested();
                }
        }

        public abstract void OnTrickStart();

        public abstract void OnTrickUpdate();

        public abstract void OnTrickEndRequested();

        public abstract void OnInit();
    }
}