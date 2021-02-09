using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    internal class ThumbstickHandler : InputHandler
    {
        private readonly string _inputString;

        public ThumbstickHandler(XRNode node, float threshold, ThumstickDir thumstickDir, bool isReversed = false) : base(threshold, isReversed)
        {
            _inputString = thumstickDir == ThumstickDir.Horizontal ? "Horizontal" : "Vertical";
            _inputString += node == XRNode.LeftHand ? "LeftHand" : "RightHand";
        }

        public override float GetInputValue()
        {
            return Input.GetAxis(_inputString);
        }
    }
}