using System;

namespace TrickSaber.InputHandling
{
    public abstract class InputHandler
    {
        protected bool IsUpTriggered = true;
        protected float Threshold;
        protected bool IsReversed;

        protected InputHandler(float threshold)
        {
            Threshold = threshold;
        }

        public abstract float GetInputValue();

        private float GetValue()
        {
            if (IsReversed) return 1f - GetInputValue();
            return GetInputValue();
        }

        public bool Pressed()
        {
            if (Math.Abs(GetValue()) > Threshold)
            {
                IsUpTriggered = false;
                return true;
            }

            return false;
        }

        public bool Up()
        {
            if (Math.Abs(GetValue()) < Threshold && !IsUpTriggered)
            {
                IsUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}