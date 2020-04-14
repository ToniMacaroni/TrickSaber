using System.Collections;
using System.Collections.Generic;
using IPA.Utilities;
using UnityEngine;

namespace TrickSaber
{
    public class SaberTrickManager : MonoBehaviour
    {
        public readonly Dictionary<TrickAction, Trick> Tricks = new Dictionary<TrickAction, Trick>();

        private InputManager _inputManager;

        private MovementController _movementController;

        private SaberTrickModel _saberTrickModel;

        public BoxCollider Collider;

        public VRController Controller;

        public Saber Saber;

        public bool IsLeftSaber => Saber.saberType == SaberType.SaberA;

        private IEnumerator Start()
        {
            if (IsLeftSaber) GlobalTrickManager.Instance.LeftSaberSaberTrickManager = this;
            else GlobalTrickManager.Instance.RightSaberSaberTrickManager = this;

            Collider = Saber.gameObject.GetComponent<BoxCollider>();
            VRPlatformHelper vrPlatformHelper =
                Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            _movementController = gameObject.AddComponent<MovementController>();
            _movementController.Controller = Controller;
            _movementController.VrPlatformHelper = vrPlatformHelper;
            _movementController.SaberTrickManager = this;

            _inputManager = gameObject.AddComponent<InputManager>();
            _inputManager.Init(Saber.saberType, Controller.GetField<VRControllersInputManager, VRController>("_vrControllersInputManager"));
            _inputManager.TrickActivated += OnTrickActivated;
            _inputManager.TrickDeactivated += OnTrickDeactivated;

            yield return WaitForSaberModel(1);
            _saberTrickModel = new SaberTrickModel(GetSaberModel());

            AddTrick<ThrowTrick>();
            AddTrick<SpinTrick>();

            Plugin.Log.Debug("Trick Manager initialized");
        }

        private void OnTrickDeactivated(TrickAction trickAction)
        {
            var trick = Tricks[trickAction];
            if (trick.State!=TrickState.Started) return;
            trick.EndTrick();
        }

        private void OnTrickActivated(TrickAction trickAction, float val)
        {
            var trick = Tricks[trickAction];
            trick.Value = val;
            if (trick.State!=TrickState.Inactive) return;
            if (GlobalTrickManager.Instance.AudioTimeSyncController.state == AudioTimeSyncController.State.Paused) return;
            trick.StartTrick();
        }

        private void OnTrickStart(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickStarted(trickAction);
        }

        private void OnTrickEnding(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickEndRequsted(trickAction);
        }

        private void OnTrickEnd(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickEnded(trickAction);
        }

        public GameObject GetSaberModel()
        {
            var model = Saber.transform.Find(Saber.name);
            if (model == null) model = Saber.transform.Find("BasicSaberModel(Clone)");
            return model.gameObject;
        }

        private IEnumerator WaitForSaberModel(int timeout)
        {
            float time = 0;
            while (!transform.Find(Saber.name) && time < timeout)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }

        private void AddTrick<T>() where T : Trick
        {
            var trick = gameObject.AddComponent<T>();
            trick.Init(this, _movementController, _saberTrickModel);
            trick.TrickStarted += OnTrickStart;
            trick.TrickEnding += OnTrickEnding;
            trick.TrickEnded += OnTrickEnd;
            Tricks.Add(trick.TrickAction, trick);
        }

        public bool IsTrickInState(TrickAction trickAction, TrickState state)
        {
            return Tricks[trickAction].State == state;
        }
    }
}