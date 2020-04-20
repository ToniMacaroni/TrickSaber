using DynamicOpenVR.IO;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.Index
{
    internal class TriggerHandler : InputHandler
    {
        private readonly VectorInput _input;

        public TriggerHandler(XRNode node, float threshold) : base(node, threshold)
        {
            if (node == XRNode.LeftHand) _input = new VectorInput("/actions/main/in/lefttriggervalue");
            else _input = new VectorInput("/actions/main/in/righttriggervalue");
        }

        public override float GetValue()
        {
            return _input.value;
        }

        public override bool Pressed()
        {
            if (GetValue() > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (GetValue() < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}