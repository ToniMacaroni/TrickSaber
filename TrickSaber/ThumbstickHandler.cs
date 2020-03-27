using System;
using UnityEngine.XR;

namespace TrickSaber
{
    class ThumbstickHandler : InputHandler
    {
        public ThumbstickHandler(VrSystem vrSystem, OVRInput.Controller oculusController,
            InputDevice controllerInputDevice, float threshold) : base(vrSystem, oculusController,
            controllerInputDevice, threshold)
        {

        }

        public override bool Pressed()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                Math.Abs(outval.x) > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var outval) &&
                Math.Abs(outval.x) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}