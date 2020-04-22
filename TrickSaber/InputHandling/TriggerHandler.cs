using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber.InputHandling
{
    internal class TriggerHandler : InputHandler
    {
        private readonly string _inputString;

        public TriggerHandler(XRNode node, float threshold) : base(threshold)
        {
            _inputString = node == XRNode.LeftHand ? "TriggerLeftHand" : "TriggerRightHand";

            IsReversed = PluginConfig.Instance.ReverseTrigger;
        }

        public override float GetInputValue()
        {
            return Input.GetAxis(_inputString);
        }
    }
}