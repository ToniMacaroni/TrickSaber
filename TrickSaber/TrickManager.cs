using IPA.Utilities;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class TrickManager : MonoBehaviour
    {
        public bool IsDoingTrick;

        private BoxCollider _collider;

        private Vector3 _controllerPosition = Vector3.zero;
        private Quaternion _controllerRotation = Quaternion.identity;
        private float _controllerSnapThreshold = 0.3f;
        private float _currentRotation;
        private bool _getBack;

        private InputManager _inputManager;
        private bool _isRotatingInPlace;
        private bool _isThrowing;
        private Vector3 _prevPos = Vector3.zero;
        private Rigidbody _rigidbody;
        private float _saberRotSpeed;

        private float _saberSpeed;

        private float _spinSpeedMultiplier = 1f;
        private Vector3 _velocity = Vector3.zero;
        private float _velocityMultiplier = 1f;
        private VRPlatformHelper _vrPlatformHelper;
        public VRController Controller;
        public Saber Saber;

        public bool IsLeftSaber => Saber.saberType == SaberType.SaberA;

        private void Start()
        {
            Plugin.Log.Debug("Trick Manager Start");
            if (IsLeftSaber) Globals.LeftSaberTrickManager = this;
            else Globals.RightSaberTrickManager = this;
            _rigidbody = Saber.gameObject.GetComponent<Rigidbody>();
            _collider = Saber.gameObject.GetComponent<BoxCollider>();
            _vrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            _inputManager = new InputManager();
            _inputManager.Init(Saber.saberType);

            _controllerSnapThreshold = PluginConfig.Instance.ControllerSnapThreshold;
            _spinSpeedMultiplier = PluginConfig.Instance.SpinSpeed;
            if (PluginConfig.Instance.SpinDirection == SpinDir.Backward.ToString())
                _spinSpeedMultiplier = -_spinSpeedMultiplier;
            _velocityMultiplier = PluginConfig.Instance.ThrowVelocity;
        }

        private void Update()
        {
            (_controllerPosition, _controllerRotation) = GetTrackingPos();
            _velocity = _controllerPosition - _prevPos;
            _saberSpeed = Vector3.Distance(_controllerPosition, _prevPos);
            _prevPos = _controllerPosition;

            if (_getBack)
            {
                float interpolation = 8 * Time.deltaTime;

                Vector3 position = Saber.transform.position;
                position = Vector3.Lerp(position, _controllerPosition, interpolation);
                Saber.gameObject.transform.position = position;

                float distance = Vector3.Distance(_controllerPosition, position);

                if (distance < _controllerSnapThreshold)
                    ThrowEnd();
                else
                    Saber.gameObject.transform.Rotate(Vector3.right, _saberRotSpeed);
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
            if (_inputManager.CheckThrowAction() && !_getBack)
                ThrowStart();

            else if (_inputManager.CheckThrowActionUp() && !_getBack)
                ThrowReturn();

            else if (_inputManager.CheckSpinAction() && !_isThrowing && !_getBack)
                InPlaceRotation();

            else if (_inputManager.CheckSpinActionUp() && _isRotatingInPlace)
                InPlaceRotationEnd();
        }

        private void ThrowStart()
        {
            if (!_isThrowing)
            {
                IsDoingTrick = true;
                Controller.enabled = false;
                //--------------
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _velocity * 220 * _velocityMultiplier;
                _collider.enabled = false;
                _saberRotSpeed = _saberSpeed * 400;
                //--------------
                _isThrowing = true;
            }

            ThrowUpdate();
        }

        private void ThrowUpdate()
        {
            Saber.gameObject.transform.Rotate(Vector3.right, _saberRotSpeed);
        }

        private void ThrowReturn()
        {
            if (_isThrowing)
            {
                //--------------
                _rigidbody.isKinematic = true;
                _rigidbody.velocity = Vector3.zero;
                _getBack = true;
                //--------------
                _isThrowing = false;
            }
        }

        private void ThrowEnd()
        {
            _getBack = false;
            _collider.enabled = true;
            Controller.enabled = true;
            IsDoingTrick = false;
        }

        private void InPlaceRotationStart()
        {
            IsDoingTrick = true;
            _currentRotation = 0;
            Controller.enabled = false;
            _isRotatingInPlace = true;
        }

        private void InPlaceRotationEnd()
        {
            _isRotatingInPlace = false;
            Controller.enabled = true;
            IsDoingTrick = false;
        }

        private void InPlaceRotation()
        {
            if (!_isRotatingInPlace) InPlaceRotationStart();

            Saber.transform.rotation = _controllerRotation;
            Saber.transform.position = _controllerPosition;
            _currentRotation += 18 * _spinSpeedMultiplier;
            Saber.transform.Rotate(Vector3.right, _currentRotation);
        }
    }
}