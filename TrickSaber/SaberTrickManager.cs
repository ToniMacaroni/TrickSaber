using System.Collections;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class SaberTrickManager : MonoBehaviour
    {
        public bool IsDoingTrick;

        public BoxCollider Collider;

        private InputManager _inputManager;
        public Rigidbody Rigidbody;
        
        public VRController Controller;
        public Saber Saber;

        private MovementController _movementController;

        public bool IsLeftSaber => Saber.saberType == SaberType.SaberA;

        //Tricks
        private ThrowTrick _throwTrick;
        private SpinTrick _spinTrick;

        private void Start()
        {
            Plugin.Log.Debug("Trick Manager Start");
            if (IsLeftSaber) Globals.LeftSaberSaberTrickManager = this;
            else Globals.RightSaberSaberTrickManager = this;

            Rigidbody = Saber.gameObject.GetComponent<Rigidbody>();
            Rigidbody.maxAngularVelocity = 800;
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            Collider = Saber.gameObject.GetComponent<BoxCollider>();
            VRPlatformHelper vrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            _movementController = gameObject.AddComponent<MovementController>();
            _movementController.Controller = Controller;
            _movementController.VrPlatformHelper = vrPlatformHelper;
            _movementController.SaberTrickManager = this;

            _inputManager = new InputManager();
            _inputManager.Init(Saber.saberType, Controller.GetField<VRControllersInputManager, VRController>("_vrControllersInputManager"));

            _throwTrick = gameObject.AddComponent<ThrowTrick>();
            _throwTrick.MovementController = _movementController;
            _throwTrick.SaberTrickManager = this;
            _throwTrick.TrickStarted += OnTrickStart;
            _throwTrick.TrickEnded += OnTrickEnd;

            _spinTrick = gameObject.AddComponent<SpinTrick>();
            _spinTrick.MovementController = _movementController;
            _spinTrick.SaberTrickManager = this;
            _spinTrick.TrickStarted += OnTrickStart;
            _spinTrick.TrickEnded += OnTrickEnd;
        }

        private void Update()
        {
            CheckButtons();
        }

        private void CheckButtons()
        {
            if (_inputManager.CheckThrowAction() && !IsDoingTrick)
                _throwTrick.StartTrick();

            else if (_inputManager.CheckThrowActionUp())
                _throwTrick.EndTrick();

            else if (_inputManager.CheckSpinAction(out var val) && !IsDoingTrick)
            {
                _spinTrick.Value = val;
                _spinTrick.StartTrick();
            }

            else if (_inputManager.CheckSpinActionUp())
                _spinTrick.EndTrick();
        }

        private void OnTrickStart(string trickName)
        {
            IsDoingTrick = true;
            if(!PluginConfig.Instance.EnableCuttingDuringTrick&&trickName!="Spin") Saber.disableCutting = true;
        }

        private void OnTrickEnd(string trickName)
        {
            IsDoingTrick = false;
            Saber.disableCutting = false;
        }

        public GameObject GetSaberModel()
        {
            var model = Saber.transform.Find(Saber.name);
            if (model == null) model = Saber.transform.Find("BasicSaberModel(Clone)");
            return model.gameObject;
        }
    }
}