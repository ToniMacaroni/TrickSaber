using System;
using UnityEngine;

namespace TrickSaber.Tricks
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

        public void Init(SaberTrickManager saberTrickManager)
        {
            SaberTrickManager = saberTrickManager;
            MovementController = SaberTrickManager.MovementController;
            SaberTrickModel = SaberTrickManager.SaberTrickModel;
            OnInit();
            Plugin.Log.Debug($"Trick: {Name} initialized");
        }

        public bool StartTrick()
        {
            if (State != TrickState.Inactive) return false;

            enabled = true;
            State = TrickState.Started;
            OnTrickStart();
            TrickStarted?.Invoke(TrickAction);
            return true;

        }

        public void EndTrick()
        {
            if (State == TrickState.Started)
            {
                enabled = false;
                _endRequested = true;
                State = TrickState.Ending;
                TrickEnding?.Invoke(TrickAction);
                OnTrickEndRequested();
            }
        }

        protected void Reset()
        {
            State = TrickState.Inactive;
            _endRequested = false;
            TrickEnded?.Invoke(TrickAction);
        }

        public abstract void OnTrickStart();

        public abstract void OnTrickEndRequested();

        public abstract void OnTrickEndImmediately();

        public abstract void OnInit();
    }
}