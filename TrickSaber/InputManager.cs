using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class InputManager : MonoBehaviour
    {
        private InputHandler _spinHandler;
        List<InputHandler> _throwInputHandlers = new List<InputHandler>();

        public void Init(SaberType type)
        {
            OVRInput.Controller _oculusController;
            InputDevice _controllerInputDevice;
            VrSystem vrSystem;
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

            vrSystem = OVRInput.IsControllerConnected(_oculusController) ? VrSystem.Oculus : VrSystem.SteamVR;

            if(PluginConfig.Instance.UseTrigger)_throwInputHandlers.Add(new TriggerHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.TriggerThreshold));
            if(PluginConfig.Instance.UseGrip) _throwInputHandlers.Add(new GripHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.GripThreshold));
            _spinHandler = new ThumbstickHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.ThumbstickThreshold);

            Plugin.Log.Debug("Started Input Manager using "+vrSystem);
        }

        public bool CheckThrowButton()
        {
            bool output = true;
            foreach (InputHandler handler in _throwInputHandlers)
            {
                output &= handler.Pressed();
            }

            return output;
        }

        public bool CheckThrowButtonUp()
        {
            foreach (InputHandler handler in _throwInputHandlers)
            {
                if (handler.Up()) return true;
            }

            return false;
        }

        public bool CheckSpinButton()
        {
            return _spinHandler.Pressed();
        }

        public bool CheckSpinButtonUp()
        {
            return _spinHandler.Up();
        }
    }
}