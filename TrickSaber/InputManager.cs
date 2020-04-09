using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    public class InputManager : MonoBehaviour
    {
        readonly HashSet<InputHandler> _throwInputHandlers = new HashSet<InputHandler>();
        readonly HashSet<InputHandler> _spinInputHandlers = new HashSet<InputHandler>();

        public void Init(SaberType type, VRControllersInputManager vrControllersInputManager)
        {
            OVRInput.Controller oculusController;
            XRNode node;
            if (type == SaberType.SaberA)
            {
                oculusController = OVRInput.Controller.LTouch;
                node = XRNode.LeftHand;
            }
            else
            {
                oculusController = OVRInput.Controller.RTouch;
                node = XRNode.RightHand;
            }
            var controllerInputDevice = InputDevices.GetDeviceAtXRNode(node);

            var vrSystem = OVRInput.IsControllerConnected(oculusController) ? VrSystem.Oculus : VrSystem.SteamVR;

            var dir = (ThumstickDir)Enum.Parse(typeof(ThumstickDir), PluginConfig.Instance.ThumstickDirection, true);

            var triggerHandler = new TriggerHandler(node, PluginConfig.Instance.TriggerThreshold);
            var gripHandler = new GripHandler(vrSystem, oculusController, controllerInputDevice, PluginConfig.Instance.GripThreshold);
            var thumbstickAction = new ThumbstickHandler(node, PluginConfig.Instance.ThumbstickThreshold, dir);
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

        private bool CheckHandlersDown(HashSet<InputHandler> handlers, out float val)
        {
            val = 0;
            if (handlers.Count == 0){ return false;}
            bool output = true;
            foreach (InputHandler handler in handlers)
            {
                output &= handler.Pressed();
                val += handler.GetValue();
            }

            if (output) val /= handlers.Count;

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
            return CheckHandlersDown(_throwInputHandlers, out var _);
        }

        public bool CheckThrowActionUp()
        {
            return CheckHandlersUp(_throwInputHandlers);
        }

        public bool CheckSpinAction(out float val)
        {
            return CheckHandlersDown(_spinInputHandlers, out val);
        }

        public bool CheckSpinActionUp()
        {
            return CheckHandlersUp(_spinInputHandlers);
        }
    }
}