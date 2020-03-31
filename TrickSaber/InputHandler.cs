using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace TrickSaber
{
    public abstract class InputHandler
    {
        protected bool _isUpTriggered;
        protected float _threshold;

        protected InputHandler(float threshold)
        {
            _threshold = threshold;
        }

        public abstract bool Pressed();
        public abstract bool Up();
    }
}
