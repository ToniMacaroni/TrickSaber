using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using IPA.Utilities;
using SiraUtil.Services;
using SiraUtil.Tools;
using TrickSaber.Configuration;
using TrickSaber.InputHandling;
using TrickSaber.Tricks;
using UnityEngine;
using Zenject;

namespace TrickSaber
{
    internal class SaberTrickManager : MonoBehaviour, IInitializable
    {
        public readonly Dictionary<TrickAction, Trick> Tricks = new Dictionary<TrickAction, Trick>();

        public SaberTrickModel SaberTrickModel;

        public bool Enabled = true;

        public bool IsLeftSaber => _saber.saberType == SaberType.SaberA;

        private VRController _vrController;

        private Saber _saber;

        private PluginConfig _config;
        private GlobalTrickManager _globalTrickManager;
        private SiraLog _logger;
        private PauseController _pauseController;
        private MovementController _movementController;
        private InputManager _inputManager;

        private SpinTrick _spinTrick;
        private ThrowTrick _throwTrick;

        [Inject]
        private void Construct(
            PluginConfig config,
            GlobalTrickManager globalTrickManager,
            SiraLog logger,
            PauseController pauseController,
            MovementController movementController,
            InputManager inputManager,
            Saber saber,
            VRController vrController,
            SpinTrick spinTrick,
            ThrowTrick throwTrick)
        {
            _config = config;
            _globalTrickManager = globalTrickManager;
            _logger = logger;
            _pauseController = pauseController;
            _movementController = movementController;
            _inputManager = inputManager;
            _saber = saber;
            _vrController = vrController;

            _spinTrick = spinTrick;
            _throwTrick = throwTrick;
        }

        public async void Initialize()
        {
            _logger.Debug($"Instantiated on {gameObject.name}");

            if (!_vrController)
            {
                _logger.Error("Controller not present");
                Cleanup();
                return;
            }

            if (IsLeftSaber) _globalTrickManager.LeftSaberSaberTrickManager = this;
            else _globalTrickManager.RightSaberSaberTrickManager = this;

            _movementController.Init(_vrController, this);

            _inputManager.Init(_saber.saberType, _vrController.GetField<VRControllersInputManager, VRController>("_vrControllersInputManager"));
            _inputManager.TrickActivated += OnTrickActivated;
            _inputManager.TrickDeactivated += OnTrickDeactivated;

            GameObject saberModel = await GetSaberModel();
            if (saberModel) _logger.Info($"Got saber model ({saberModel.name})");
            else
            {
                _logger.Error("Couldn't get saber model");
                Cleanup();
                return;
            }

            SaberTrickModel = new SaberTrickModel(saberModel);

            AddTrick(_spinTrick);
            AddTrick(_throwTrick);

            _logger.Info($"{Tricks.Count} tricks initialized");

            _pauseController.didResumeEvent += EndAllTricks;

            _logger.Info("Trick Manager initialized");
        }

        private void Cleanup()
        {
            foreach (var trick in Tricks.Values)
            {
                DestroyImmediate(trick);
            }

            DestroyImmediate(_movementController);
            DestroyImmediate(this);
        }

        private void Update()
        {
            _inputManager.Tick();
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
            if (_globalTrickManager.AudioTimeSyncController.state ==
                AudioTimeSyncController.State.Paused) return;
            trick.StartTrick();
        }

        #region Trick Events

        private void OnTrickStart(TrickAction trickAction)
        {
            _globalTrickManager.OnTrickStarted(trickAction);
        }

        private void OnTrickEnding(TrickAction trickAction)
        {
            _globalTrickManager.OnTrickEndRequested(trickAction);
        }

        private void OnTrickEnd(TrickAction trickAction)
        {
            _globalTrickManager.OnTrickEnded(trickAction);
        }

        #endregion

        public async Task<GameObject> GetSaberModel()
        {
            Transform result = null;

            var timeout = 2000;
            var interval = 300;
            var time = 0;

            while (result==null)
            {
                result = _saber.transform.Find("SF Saber"); // Saber Factory
                if (!result) result = _saber.transform.Find(SaberTrickModel.BasicSaberModelName); // Default Saber

                if (time > timeout) return null;

                time += interval;
                await Task.Delay(interval);
            }

            return result.gameObject;
        }

        private void AddTrick(Trick trick)
        {
            trick.Init(this, _movementController);
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
            return _config.TrickSaberEnabled &&
                   Enabled &&
                   _globalTrickManager.CanDoTrick();
        }
    }
}
