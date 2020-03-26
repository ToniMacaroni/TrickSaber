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
        private ButtonMapping _mapping;

        private InputDevice _controllerInputDevice;

        public void Init(SaberType type)
        {
            if (type == SaberType.SaberA)
            {
                _mapping = ButtonMapping.LeftButtons;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            else
            {
                _mapping = ButtonMapping.RightButtons;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }

            if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch)) _isOculus = true;
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
                _axisUpTriggered = false;
                return true;
            }

            return false;
        }

        bool GetTriggerOculus()
        {
            return OVRInput.Get(_mapping.ThrowButtton);
        }

        bool GetTriggerUpOculus()
        {
            return OVRInput.GetUp(_mapping.ThrowButtton);
        }

        public bool GetTrigger()
        {
            if (_isOculus) return GetTriggerOculus();

            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool outvar) && outvar)
            {
                _triggerUpTriggered = false;
                return true;
            }
            return false;
        }

        public bool GetTriggerUp()
        {
            if (_isOculus) return GetTriggerUpOculus();

            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool outvar) && !outvar && !_triggerUpTriggered)
            {
                _triggerUpTriggered = true;
                return true;
            }
            return false;
        }
    }
}