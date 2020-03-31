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
        
        public VRPlatformHelper VrPlatformHelper;
        public VRController Controller;
        public Saber Saber;

        public MovementController MovementController;

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
            VrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            MovementController = gameObject.AddComponent<MovementController>();
            MovementController.Controller = Controller;
            MovementController.SaberTrickManager = this;

            _inputManager = new InputManager();
            _inputManager.Init(Saber.saberType, Controller.GetField<VRControllersInputManager, VRController>("_vrControllersInputManager"));

            _throwTrick = gameObject.AddComponent<ThrowTrick>();
            _throwTrick.MovementController = MovementController;
            _throwTrick.SaberTrickManager = this;

            _spinTrick = gameObject.AddComponent<SpinTrick>();
            _spinTrick.MovementController = MovementController;
            _spinTrick.SaberTrickManager = this;
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

            else if (_inputManager.CheckSpinAction() && !IsDoingTrick)
                _spinTrick.StartTrick();

            else if (_inputManager.CheckSpinActionUp())
                _spinTrick.EndTrick();
        }
    }
}