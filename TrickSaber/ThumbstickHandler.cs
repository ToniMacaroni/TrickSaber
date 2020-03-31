using System;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    class ThumbstickHandler : InputHandler
    {
        private readonly string _axisString = "";

        public ThumbstickHandler(VrSystem vrSystem, XRNode node, float threshold, ThumstickDir thumstickDir) : base(threshold)
        {
            _axisString = thumstickDir == ThumstickDir.Horizontal ? "Horizontal" : "Vertical";
            _axisString += node == XRNode.LeftHand ? "LeftHand" : "RightHand";
        }

        public override bool Pressed()
        {
            if (Math.Abs(Input.GetAxis(_axisString)) > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (Math.Abs(Input.GetAxis(_axisString)) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}