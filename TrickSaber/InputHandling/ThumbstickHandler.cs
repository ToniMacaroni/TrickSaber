using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    internal class ThumbstickHandler : InputHandler
    {
        private readonly string _axisString;

        public ThumbstickHandler(XRNode node, float threshold, ThumstickDir thumstickDir) : base(threshold)
        {
            _axisString = thumstickDir == ThumstickDir.Horizontal ? "Horizontal" : "Vertical";
            _axisString += node == XRNode.LeftHand ? "LeftHand" : "RightHand";
            IsReversed = PluginConfig.Instance.ReverseThumbstick;
        }

        public override float GetInputValue()
        {
            return Input.GetAxis(_axisString);
        }
    }
}