using System.Collections.Generic;
using IPA.Utilities;
using SiraUtil.Logging;
using SiraUtil.Tools;
using TrickSaber.Configuration;
using TrickSaber.InputHandling;
using TrickSaber.Tricks;
using UnityEngine;
using Zenject;

namespace TrickSaber
{
    internal class SaberTrickManager : MonoBehaviour
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
        private AudioTimeSyncController _audioTimeSyncController;

        private Trick.Factory _trickFactory;

        [Inject]
        private void Construct(
            PluginConfig config,
            SiraLog logger,
            [InjectOptional] PauseController pauseController,
            MovementController movementController,
            InputManager inputManager,
            SaberControllerBearer saberControllerBearer,
            SaberType saberType,
            SaberTrickModel saberTrickModel,
            AudioTimeSyncController audioTimeSyncController,
            Trick.Factory trickFactory)
        {
            _config = config;
            _logger = logger;
            _pauseController = pauseController;
            _movementController = movementController;
            _inputManager = inputManager;
            _audioTimeSyncController = audioTimeSyncController;
            SaberTrickModel = saberTrickModel;

            _saber = saberControllerBearer[saberType].Saber;
            _vrController = saberControllerBearer[saberType].VRController;

            _trickFactory = trickFactory;
        }

        public async void Init(GlobalTrickManager globalTrickManager)
        {
            _globalTrickManager = globalTrickManager;

            _logger.Debug($"Instantiated on {gameObject.name}");

            if (!_vrController)
            {
                _logger.Error("Controller not present");
                Cleanup();
                return;
            }

            if (IsLeftSaber) _globalTrickManager.LeftSaberTrickManager = this;
            else _globalTrickManager.RightSaberTrickManager = this;

            _movementController.Init(_vrController, this);

            _inputManager.Init(_saber.saberType);
            _inputManager.TrickActivated += OnTrickActivated;
            _inputManager.TrickDeactivated += OnTrickDeactivated;

            var success = await SaberTrickModel.Init(_saber);
            if (success) _logger.Info($"Got saber model");
            else
            {
                _logger.Error("Couldn't get saber model");
                Cleanup();
                return;
            }

            _movementController.enabled = true;

            AddTrick<SpinTrick>();
            AddTrick<ThrowTrick>();

            _logger.Info($"{Tricks.Count} tricks initialized");

            if (_pauseController)
            {
                _pauseController.didResumeEvent += EndAllTricks;
            }

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
            if (_audioTimeSyncController.state ==
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

        private void AddTrick<T>() where T : Trick
        {
            var trick = _trickFactory.Create(typeof(T), gameObject);
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
