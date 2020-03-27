using System;
using UnityEngine.XR;

namespace TrickSaber
{
    class GripHandler : InputHandler
    {
        private Func<bool> _pressedFunc;
        private Func<bool> _upFunc;

        public GripHandler(VrSystem vrSystem, OVRInput.Controller oculusController,
            InputDevice controllerInputDevice, float threshold) : base(vrSystem, oculusController,
            controllerInputDevice, threshold)
        {
            if (vrSystem == VrSystem.Oculus)
            {
                _pressedFunc = PressedOculus;
                _upFunc = UpOculus;
            }
            else
            {
                _pressedFunc = PressedSteam;
                _upFunc = UpSteam;
            }
        }

        private bool PressedOculus()
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _oculusController)>_threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        private bool PressedSteam()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.grip, out var outvar) && outvar > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        private bool UpOculus()
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _oculusController) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }
            return false;
        }

        private bool UpSteam()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.grip, out var outvar) && outvar < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }
            return false;
        }

        public override bool Pressed()
        {
            return _pressedFunc();
        }

        public override bool Up()
        {
            return _upFunc();
        }
    }
}