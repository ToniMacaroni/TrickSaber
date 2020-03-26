using System;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class InputManager : MonoBehaviour
    {
        private bool _axisUpTriggered = true;
        private bool _triggerUpTriggered = true;

        private bool _isOculus;
        private ButtonMapping _mapping = ButtonMapping.DefaultMapping;

        private InputDevice _controllerInputDevice;
        private OVRInput.Controller _oculusController;

        public void Init(SaberType type)
        {
            if (type == SaberType.SaberA)
            {
                _oculusController = OVRInput.Controller.LTouch;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            else
            {
                _oculusController = OVRInput.Controller.RTouch;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }

            if (OVRInput.IsControllerConnected(_oculusController)) _isOculus = true;

            Plugin.Log.Debug("Input Manager Init: "+(_isOculus ? "Oculus" : "Steam"));
        }

        public bool CheckAxis()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                Math.Abs(outval.x) > 0.8f)
            {
                _axisUpTriggered = false;
                return true;
            }

            return false;
        }

        public bool CheckAxisUp()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                Math.Abs(outval.x) < 0.5f && !_axisUpTriggered)
            {
                _axisUpTriggered = true;
                return true;
            }

            return false;
        }

        bool GetTriggerOculus()
        {
            return OVRInput.Get(_mapping.ThrowButtton, _oculusController);
        }

        bool GetTriggerUpOculus()
        {
            return OVRInput.GetUp(_mapping.ThrowButtton, _oculusController);
        }

        public bool GetTrigger()
        {
            if (_isOculus) return GetTriggerOculus();

            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out var outvar) && outvar)
            {
                _triggerUpTriggered = false;
                return true;
            }
            return false;
        }

        public bool GetTriggerUp()
        {
            if (_isOculus) return GetTriggerUpOculus();

            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out var outvar) && !outvar && !_triggerUpTriggered)
            {
                _triggerUpTriggered = true;
                return true;
            }
            return false;
        }
    }
}