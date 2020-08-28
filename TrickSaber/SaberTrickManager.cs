using System.Collections;
using System.Collections.Generic;
using BeatSaberMarkupLanguage;
using IPA.Utilities;
using TrickSaber.InputHandling;
using TrickSaber.Tricks;
using UnityEngine;

namespace TrickSaber
{
    public class SaberTrickManager : MonoBehaviour
    {
        public readonly Dictionary<TrickAction, Trick> Tricks = new Dictionary<TrickAction, Trick>();

        private InputManager _inputManager;

        public MovementController MovementController;

        public SaberTrickModel SaberTrickModel;

        public VRController Controller;

        public Saber Saber;

        public bool Enabled = true;

        public bool IsLeftSaber => Saber.saberType == SaberType.SaberA;

        private IEnumerator Start()
        {
            Saber = gameObject.GetComponent<Saber>();
            Controller = gameObject.GetComponent<VRController>();

            if (!Controller)
            {
                Destroy(gameObject);
                yield break;
            }

            if (IsLeftSaber) GlobalTrickManager.Instance.LeftSaberSaberTrickManager = this;
            else GlobalTrickManager.Instance.RightSaberSaberTrickManager = this;

            VRPlatformHelper vrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            MovementController = gameObject.AddComponent<MovementController>();
            MovementController.Controller = Controller;
            MovementController.VrPlatformHelper = vrPlatformHelper;
            MovementController.SaberTrickManager = this;

            _inputManager = gameObject.AddComponent<InputManager>();
            _inputManager.Init(Saber.saberType, Controller.GetField<VRControllersInputManager, VRController>("_vrControllersInputManager"));
            _inputManager.TrickActivated += OnTrickActivated;
            _inputManager.TrickDeactivated += OnTrickDeactivated;

            //We need to wait for CustomSabers to potentially change the saber models
            yield return WaitForSaberModel(1);

            GameObject saberModel = GetSaberModel();
            if(saberModel) Plugin.Log.Debug($"Got saber model ({saberModel.name})");
            else
            {
                Plugin.Log.Debug("Couldn't get saber model");
                Destroy(gameObject);
                yield break;
            }

            SaberTrickModel = new SaberTrickModel(saberModel);

            AddTrick<ThrowTrick>();
            AddTrick<SpinTrick>();

            Plugin.Log.Debug($"{Tricks.Count} tricks initialized");

            BS_Utils.Utilities.BSEvents.songUnpaused += EndAllTricks;

            Plugin.Log.Debug("Trick Manager initialized");
        }

        private void OnTrickDeactivated(TrickAction trickAction)
        {
            var trick = Tricks[trickAction];
            if (trick.State != TrickState.Started) return;
            trick.EndTrick();
        }

        private void OnTrickActivated(TrickAction trickAction, float val)
        {
            if (!CanDoTrick()) return;
            var trick = Tricks[trickAction];
            trick.Value = val;
            if (trick.State != TrickState.Inactive) return;
            if (GlobalTrickManager.Instance.AudioTimeSyncController.state ==
                AudioTimeSyncController.State.Paused) return;
            trick.StartTrick();
        }

        #region Trick Events
        private void OnTrickStart(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickStarted(trickAction);
        }

        private void OnTrickEnding(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickEndRequested(trickAction);
        }

        private void OnTrickEnd(TrickAction trickAction)
        {
            GlobalTrickManager.Instance.OnTrickEnded(trickAction);
        }
        #endregion

        public GameObject GetSaberModel()
        {
            var model = Saber.transform.Find("SFSaber"); // Saber Factory
            if (model == null) model = Saber.transform.Find(Saber.name); // Custom Sabers
            if (model == null) model = Saber.transform.Find(SaberTrickModel.BasicSaberModelName); // Default Saber
            if (model != null) return model.gameObject;
            return null;
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
            trick.Init(this);
            trick.TrickStarted += OnTrickStart;
            trick.TrickEnding += OnTrickEnding;
            trick.TrickEnded += OnTrickEnd;
            Tricks.Add(trick.TrickAction, trick);
        }

        public bool IsTrickInState(TrickAction trickAction, TrickState state)
        {
            return Tricks[trickAction].State == state;
        }

        public bool IsDoingTrick()
        {
            foreach (var trick in Tricks.Values)
            {
                if (trick.State != TrickState.Inactive) return true;
            }

            return false;
        }

        public void EndAllTricks()
        {
            foreach (var trick in Tricks.Values)
            {
                trick.OnTrickEndImmediately();
            }
        }

        private bool CanDoTrick()
        {
            return PluginConfig.Instance.TrickSaberEnabled &&
                   Enabled &&
                   GlobalTrickManager.Instance.CanDoTrick();
        }
    }
}
