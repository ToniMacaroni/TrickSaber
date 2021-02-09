using System;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    internal class GripHandler : InputHandler
    {
        private readonly OVRInput.Controller _oculusController;
        private InputDevice _controllerInputDevice;

        private readonly Func<float> _valueFunc;

        public GripHandler(VRSystem vrSystem,
            OVRInput.Controller oculusController,
            InputDevice controllerInputDevice,
            float threshold,
            bool isReversed = false)
            : base(threshold, isReversed)
        {
            _oculusController = oculusController;
            _controllerInputDevice = controllerInputDevice;
            if (vrSystem == VRSystem.Oculus)
                _valueFunc = GetValueOculus;
            else
                _valueFunc = GetValueSteam;
        }

        private float GetValueSteam()
        {
            if (_controllerInputDevice.TryGetFeatureValue(CommonUsages.grip, out var outvar)) return outvar;
            return 0;
        }

        //CommomUsages doesn't work well with Touch Controllers, so we need to use the Oculus function for them
        private float GetValueOculus()
        {
            return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _oculusController);
        }

        public override float GetInputValue()
        {
            return _valueFunc();
        }
    }
}