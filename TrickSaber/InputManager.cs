using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class InputManager : MonoBehaviour
    {
        HashSet<InputHandler> _throwInputHandlers = new HashSet<InputHandler>();
        HashSet<InputHandler> _spinInputHandlers = new HashSet<InputHandler>();

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

            var dir = (ThumstickDir)Enum.Parse(typeof(ThumstickDir), PluginConfig.Instance.ThumstickDirection, true);

            var triggerHandler = new TriggerHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.TriggerThreshold);
            var gripHandler = new GripHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.GripThreshold);
            var thumbstickAction = new ThumbstickHandler(vrSystem, _oculusController, _controllerInputDevice, PluginConfig.Instance.ThumbstickThreshold, dir);
            AddHandler(triggerHandler, PluginConfig.Instance.TriggerAction.GetTrickAction());
            AddHandler(gripHandler, PluginConfig.Instance.GripAction.GetTrickAction());
            AddHandler(thumbstickAction, PluginConfig.Instance.ThumbstickAction.GetTrickAction());

            Plugin.Log.Debug("Started Input Manager using "+vrSystem);
        }

        private void AddHandler(InputHandler handler, TrickAction action)
        {
            switch (action)
            {
                case TrickAction.None:
                    return;
                case TrickAction.Throw:
                    _throwInputHandlers.Add(handler);
                    break;
                case TrickAction.Spin:
                    _spinInputHandlers.Add(handler);
                    break;
                default:
                    return;
            }
        }

        private bool CheckHandlersDown(HashSet<InputHandler> handlers)
        {
            if (handlers.Count == 0) return false;
            bool output = true;
            foreach (InputHandler handler in handlers)
            {
                output &= handler.Pressed();
            }

            return output;
        }

        private bool CheckHandlersUp(HashSet<InputHandler> handlers)
        {
            foreach (InputHandler handler in handlers)
            {
                if (handler.Up()) return true;
            }

            return false;
        }

        public bool CheckThrowAction()
        {
            return CheckHandlersDown(_throwInputHandlers);
        }

        public bool CheckThrowActionUp()
        {
            return CheckHandlersUp(_throwInputHandlers);
        }

        public bool CheckSpinAction()
        {
            return CheckHandlersDown(_spinInputHandlers);
        }

        public bool CheckSpinActionUp()
        {
            return CheckHandlersUp(_spinInputHandlers);
        }
    }
}