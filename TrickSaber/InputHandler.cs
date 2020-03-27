using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace TrickSaber
{
    public abstract class InputHandler
    {
        protected bool _isUpTriggered;
        protected float _threshold;
        protected VrSystem _vrSystem;
        protected XRNode _node;

        protected OVRInput.Controller _oculusController;
        protected InputDevice _controllerInputDevice;

        protected InputHandler(VrSystem vrSystem, OVRInput.Controller oculusController, InputDevice controllerInputDevice, float threshold)
        {
            _vrSystem = vrSystem;
            _oculusController = oculusController;
            _controllerInputDevice = controllerInputDevice;
            _threshold = threshold;
        }

        public abstract bool Pressed();
        public abstract bool Up();
    }
}
