using IPA.Utilities;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class TrickManager : MonoBehaviour
    {
        public Saber Saber;
        public VRController Controller;
        private VRPlatformHelper _vrPlatformHelper;

        public bool IsLeftSaber => Saber.saberType == SaberType.SaberA;
        private bool _isThrowing;
        private bool _isRotatingInPlace;
        private bool _getBack;
        private Rigidbody _rigidbody;
        private BoxCollider _collider;

        Vector3 _controllerPosition = Vector3.zero;
        Quaternion _controllerRotation = Quaternion.identity;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _prevPos = Vector3.zero;
        private float _currentRotation;

        private float _saberSpeed;
        private float _saberRotSpeed;

        private InputManager _inputManager;

        void Start()
        {
            Plugin.Log.Debug("Trick Manager Start");
            _rigidbody = Saber.gameObject.GetComponent<Rigidbody>();
            _collider = Saber.gameObject.GetComponent<BoxCollider>();
            _vrPlatformHelper = Controller.GetField<VRPlatformHelper, VRController>("_vrPlatformHelper");

            _inputManager = new InputManager();
            _inputManager.Init(Saber.saberType);
        }

        void Update()
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

                if (distance < 0.3f)
                {
                    ThrowEnd();
                    
                }/*else if (distance < 0.5)
                {
                    //Lerp to Controller Rotation
                    Quaternion rotation = Saber.transform.rotation;
                    rotation = Quaternion.Lerp(rotation, origRot, interpolation);
                    Saber.gameObject.transform.rotation = rotation;
                }*/
                else
                {
                    Saber.gameObject.transform.Rotate(Vector3.right, _saberRotSpeed);
                }
            }

            CheckButtons();
        }

        (Vector3, Quaternion) GetTrackingPos()
        {
            var success = _vrPlatformHelper.GetNodePose(Controller.node, Controller.nodeIdx, out var pos, out var rot);
            if (!success)
            {
                return (new Vector3(-0.2f, 0.05f, 0f), Quaternion.identity);
            }
            return (pos, rot);
        }

        void CheckButtons()
        {
            if (_inputManager.CheckThrowButton() && !_getBack)
            {
                ThrowStart();
            }
            else if (_inputManager.CheckThrowButtonUp() && !_getBack)
            {
                ThrowReturn();
            }
            else if (_inputManager.CheckSpinButton() && !_isThrowing && !_getBack)
            {
                InPlaceRotation();
            }
            else if (_inputManager.CheckSpinButtonUp() && _isRotatingInPlace)
            {
                InPlaceRotationEnd();
            }
        }

        void ThrowStart()
        {
            if (!_isThrowing)
            {
                Controller.enabled = false;
                //--------------
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _velocity * 220;
                _collider.enabled = false;
                _saberRotSpeed = _saberSpeed*400;
                //--------------
                _isThrowing = true;
            }

            ThrowUpdate();
        }

        void ThrowUpdate()
        {
            Saber.gameObject.transform.Rotate(Vector3.right, _saberRotSpeed);
        }

        void ThrowReturn()
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

        void ThrowEnd()
        {
            _getBack = false;
            _collider.enabled = true;
            Controller.enabled = true;
        }

        void InPlaceRotationStart()
        {
            _currentRotation = 0;
            Controller.enabled = false;
            _isRotatingInPlace = true;
        }

        void InPlaceRotationEnd()
        {
            _isRotatingInPlace = false;
            Controller.enabled = true;
        }

        void InPlaceRotation()
        {
            if (!_isRotatingInPlace)
            {
                InPlaceRotationStart();
            }

            Saber.transform.rotation = _controllerRotation;
            Saber.transform.position = _controllerPosition;
            _currentRotation -= 18;
            Saber.transform.Rotate(Vector3.right, _currentRotation);
        }
    }
}