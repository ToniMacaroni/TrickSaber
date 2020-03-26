using System;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class InputManager : MonoBehaviour
    {
        public SaberType type;

        private SteamVR_Controller.Device _device;

        private bool _axisUpTriggered = true;
        private bool _triggerUpTriggered = true;

        private bool _isOculus;
        private ButtonMapping _mapping;

        private InputDevice _controllerInputDevice;

        public void Init(SaberType type)
        {
            if (type == SaberType.SaberA)
            {
                _device = SteamVR_Controller.Input(1);
                _mapping = ButtonMapping.LeftButtons;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            else
            {
                _device = SteamVR_Controller.Input(2);
                _mapping = ButtonMapping.RightButtons;
                _controllerInputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }

            /*if (!_device.connected)*/ _isOculus = true;
            Plugin.Log.Debug("Input Manager Init: "+(_isOculus ? "Oculus" : "Steam"));
        }

        private bool CheckAxisSteam()
        {
            if (Math.Abs(_device.GetAxis().x) > 0.8f)
            {
                _axisUpTriggered = false;
                return true;
            }

            return false;
        }

        private bool CheckAxisUpSteam()
        {
            if (Math.Abs(_device.GetAxis().x) < 0.5f && !_axisUpTriggered)
            {
                _axisUpTriggered = true;
                return true;
            }

            return false;
        }

        private bool CheckAxisOculus()
        {
            return OVRInput.Get(_mapping.RotateButton);
        }

        private bool CheckAxisUpOculus()
        {
            return OVRInput.GetUp(_mapping.RotateButton);
        }

        public bool CheckAxis()
        {
            if (_isOculus) return CheckAxisOculus();
            return CheckAxisSteam();
        }

        public bool CheckAxisUp()
        {
            if (_isOculus) return CheckAxisUpOculus();
            return CheckAxisUpSteam();
        }

        /*bool GetTriggerOculus()
        {
            return OVRInput.Get(_mapping.ThrowButtton);
        }*/

        /*bool GetTriggerUpOculus()
        {
            return OVRInput.GetUp(_mapping.ThrowButtton);
        }*/

        /*bool GetTriggerSteam()
        {
            return _device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        }*/

        /*bool GetTriggerUpSteam()
        {
            return _device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
        }*/

        public bool GetTrigger()
        {
            bool outvar = false;
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out outvar) && outvar)
            {
                _triggerUpTriggered = false;
                return true;
            }
            return false;
        }

        public bool GetTriggerUp()
        {
            bool outvar = true;
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out outvar) && !outvar && !_triggerUpTriggered)
            {
                _triggerUpTriggered = true;
                return true;
            }
            return false;
        }
    }
}