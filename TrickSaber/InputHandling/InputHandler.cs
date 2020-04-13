namespace TrickSaber
{
    public abstract class InputHandler
    {
        protected bool _isUpTriggered = true;
        protected float _threshold;

        protected InputHandler(float threshold)
        {
            _threshold = threshold;
        }

        public abstract float GetValue();
        public abstract bool Pressed();
        public abstract bool Up();
    }
}