using System;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    class ThumbstickHandler : InputHandler
    {
        private ThumstickDir _thumstickDir;

        public ThumbstickHandler(VrSystem vrSystem, OVRInput.Controller oculusController,
            InputDevice controllerInputDevice, float threshold, ThumstickDir thumstickDir) : base(vrSystem, oculusController,
            controllerInputDevice, threshold)
        {
            _thumstickDir = thumstickDir;
        }

        private float GetAxis(Vector2 vec)
        {
            return Math.Abs(_thumstickDir == ThumstickDir.Horizontal ? vec.x : vec.y);
        }

        public override bool Pressed()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                GetAxis(outval) > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                GetAxis(outval) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}