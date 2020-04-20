using System;
using DynamicOpenVR.IO;
using UnityEngine.XR;

namespace TrickSaber.Index
{
    internal class GripHandler : InputHandler
    {
        private readonly VectorInput _input;

        public GripHandler(XRNode node, float threshold) : base(node, threshold)
        {
            if(node==XRNode.LeftHand) _input = new VectorInput("/actions/main/in/leftgripvalue");
            else _input = new VectorInput("/actions/main/in/rightgripvalue");
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