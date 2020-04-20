using System;
using DynamicOpenVR.IO;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.DOVR
{
    internal class ThumbstickHandler : InputHandler
    {
        private readonly Func<float> _valueFunc;
        private readonly Vector2Input _input;

        public ThumbstickHandler(XRNode node, float threshold, ThumstickDir thumstickDir) : base(node, threshold)
        {
            if(node==XRNode.LeftHand) _input = new Vector2Input("/actions/main/in/leftthumbstickvalue");
            else _input = new Vector2Input("/actions/main/in/rightthumbstickvalue");
            if (thumstickDir == ThumstickDir.Horizontal) _valueFunc = GetValueHorizontal;
            else _valueFunc = GetValueVertical;
        }

        private float GetValueHorizontal()
        {
            return _input.vector.x;
        }

        private float GetValueVertical()
        {
            return _input.vector.y;
        }

        public override float GetValue()
        {
            return _valueFunc();
        }

        public override bool Pressed()
        {
            if (Math.Abs(GetValue()) > _threshold)
            {
                _isUpTriggered = false;
                return true;
            }

            return false;
        }

        public override bool Up()
        {
            if (Math.Abs(GetValue()) < _threshold && !_isUpTriggered)
            {
                _isUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}