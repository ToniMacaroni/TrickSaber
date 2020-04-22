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

        private float GetActivationValue(float val)
        {
            float value = Math.Abs(val);
            if (IsReversed) return 1f - value;
            return value;
        }

        public bool Activated(out float val)
        {
            var value = GetInputValue();
            var activationValue = GetActivationValue(value);
            val = 0;

            if (activationValue > Threshold)
            {
                val = IsReversed ? activationValue : value;
                IsUpTriggered = false;
                return true;
            }

            return false;
        }

        public bool Deactivated()
        {
            if (GetActivationValue(GetInputValue()) < Threshold && !IsUpTriggered)
            {
                IsUpTriggered = true;
                return true;
            }

            return false;
        }
    }
}