using System;
using UnityEngine.XR;

namespace TrickSaber
{
    class GripHandler : InputHandler
    {
        private readonly Func<float> _valueFunc;

        private readonly OVRInput.Controller _oculusController;
        private InputDevice _controllerInputDevice;

        public GripHandler(VrSystem vrSystem, OVRInput.Controller oculusController,
            InputDevice controllerInputDevice, float threshold) : base(threshold)
        {
            _oculusController = oculusController;
            _controllerInputDevice = controllerInputDevice;
            if (vrSystem == VrSystem.Oculus)
            {
                _valueFunc = GetValueOculus;
            }
            else
            {
                _valueFunc = GetValueSteam;
            }
        }

        private float GetValueSteam()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.grip, out var outvar)) return outvar;
            return 0;
        }

        private float GetValueOculus()
        {
            return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _oculusController);
        }

        public override float GetValue()
        {
            return _valueFunc();
        }

        public override bool Pressed()
        {
            if (_valueFunc() > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (_valueFunc() < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }
            return false;
        }
    }
}