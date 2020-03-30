using System.Collections;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class SaberTrickManager : MonoBehaviour
    {
        public bool IsDoingTrick;

        public BoxCollider _collider;

        public Vector3 _controllerPosition = Vector3.zero;
        public Quaternion _controllerRotation = Quaternion.identity;

        private InputManager _inputManager;
        private Vector3 _prevPos = Vector3.zero;
        public Rigidbody Rigidbody;

        public float SaberSpeed;

        public Vector3 _velocity = Vector3.zero;
        private VRPlatformHelper _vrPlatformHelper;
        public VRController Controller;
        public Saber Saber;

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

            _collider = Saber.gameObject.GetComponent<BoxCollider>();
            _vrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            _inputManager = new InputManager();
            _inputManager.Init(Saber.saberType);

            _throwTrick = gameObject.AddComponent<ThrowTrick>();
            _throwTrick.SaberTrickManager = this;

            _spinTrick = gameObject.AddComponent<SpinTrick>();
            _spinTrick.SaberTrickManager = this;
        }

        private void Update()
        {
            (_controllerPosition, _controllerRotation) = GetTrackingPos();
            if (Controller.enabled)
            {
                var controllerPosition = Controller.position;
                _velocity = (controllerPosition - _prevPos)/Time.deltaTime;
                SaberSpeed = _velocity.magnitude;
                _prevPos = controllerPosition;
            }
            CheckButtons();
        }

        private (Vector3, Quaternion) GetTrackingPos()
        {
            var success = _vrPlatformHelper.GetNodePose(Controller.node, Controller.nodeIdx, out var pos, out var rot);
            if (!success) return (new Vector3(-0.2f, 0.05f, 0f), Quaternion.identity);
            return (pos, rot);
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