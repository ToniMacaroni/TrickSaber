using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.Index
{
    internal class TriggerHandler : InputHandler
    {
        private readonly string _inputString;

        public TriggerHandler(XRNode node, float threshold) : base(threshold)
        {
            _inputString = node == XRNode.LeftHand ? "TriggerLeftHand" : "TriggerRightHand";
        }

        public override float GetValue()
        {
            return Input.GetAxis(_inputString);
        }

        public override bool Pressed()
        {
            if (Input.GetAxis(_inputString) > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (Input.GetAxis(_inputString) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}