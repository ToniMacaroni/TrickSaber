using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    internal class TriggerHandler : InputHandler
    {
        private readonly string _inputString;

        public TriggerHandler(XRNode node, float threshold, bool isReversed = false) : base(threshold, isReversed)
        {
            _inputString = node == XRNode.LeftHand ? "TriggerLeftHand" : "TriggerRightHand";
        }

        public override float GetInputValue()
        {
            return Input.GetAxis(_inputString);
        }
    }
}