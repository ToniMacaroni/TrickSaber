using UnityEngine.XR;

namespace TrickSaber.Index
{
    public abstract class InputHandler
    {
        protected bool _isUpTriggered = true;
        protected float _threshold;
        protected XRNode _node;

        protected InputHandler(XRNode node, float threshold)
        {
            _node = node;
            _threshold = threshold;
        }

        public abstract float GetValue();
        public abstract bool Pressed();
        public abstract bool Up();
    }
}