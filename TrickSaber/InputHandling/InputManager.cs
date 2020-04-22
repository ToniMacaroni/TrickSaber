using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    public class InputManager : MonoBehaviour
    {
        private readonly TrickInputHandler _trickInputHandler = new TrickInputHandler();

        public event Action<TrickAction, float> TrickActivated;
        public event Action<TrickAction> TrickDeactivated;

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

            var vrSystem = OVRInput.IsControllerConnected(oculusController) ? VRSystem.Oculus : VRSystem.SteamVR;

            var dir = (ThumstickDir) Enum.Parse(typeof(ThumstickDir), PluginConfig.Instance.ThumstickDirection, true);

            var triggerHandler = new TriggerHandler(node, PluginConfig.Instance.TriggerThreshold);
            var gripHandler = new GripHandler(vrSystem, oculusController, controllerInputDevice,
                PluginConfig.Instance.GripThreshold);
            var thumbstickAction = new ThumbstickHandler(node, PluginConfig.Instance.ThumbstickThreshold, dir);

            _trickInputHandler.Add(PluginConfig.Instance.TriggerAction.GetEnumValue<TrickAction>(), triggerHandler);
            _trickInputHandler.Add(PluginConfig.Instance.GripAction.GetEnumValue<TrickAction>(), gripHandler);
            _trickInputHandler.Add(PluginConfig.Instance.ThumbstickAction.GetEnumValue<TrickAction>(),
                thumbstickAction);

            Plugin.Log.Debug("Started Input Manager using " + vrSystem);
        }

        private void Update()
        {
            foreach (TrickAction trickAction in _trickInputHandler.TrickHandlerSets.Keys)
            {
                var handlers = _trickInputHandler.GetHandlers(trickAction);
                if (CheckHandlersDown(handlers, out var val))
                    TrickActivated?.Invoke(trickAction, val);
                    
                else if (CheckHandlersUp(handlers)) TrickDeactivated?.Invoke(trickAction);
            }
        }

        private bool CheckHandlersDown(HashSet<InputHandler> handlers, out float val)
        {
            val = 0;
            if (handlers.Count == 0) return false;
            bool output = true;
            foreach (InputHandler handler in handlers)
            {
                output &= handler.Activated(out var handlerValue);
                val += handlerValue;
            }

            if (output) val /= handlers.Count;

            return output;
        }

        private bool CheckHandlersUp(HashSet<InputHandler> handlers)
        {
            foreach (InputHandler handler in handlers)
                if (handler.Deactivated())
                    return true;

            return false;
        }
    }

    public class TrickInputHandler
    {
        public Dictionary<TrickAction, HashSet<InputHandler>> TrickHandlerSets =
            new Dictionary<TrickAction, HashSet<InputHandler>>();

        public TrickInputHandler()
        {
            foreach (object value in Enum.GetValues(typeof(TrickAction)))
            {
                var action = (TrickAction) value;
                if (action == TrickAction.None) continue;
                TrickHandlerSets.Add(action, new HashSet<InputHandler>());
            }
        }

        public void Add(TrickAction action, InputHandler handler)
        {
            if (action == TrickAction.None) return;
            TrickHandlerSets[action].Add(handler);
        }

        public HashSet<InputHandler> GetHandlers(TrickAction action)
        {
            return TrickHandlerSets[action];
        }
    }
}