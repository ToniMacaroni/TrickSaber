using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace TrickSaber
{
    public struct ButtonMapping
    {
        public OVRInput.Button ThrowButtton;
        public OVRInput.Button RotateButton;
        public InputFeatureUsage<bool> ThrowUsage;

        public ButtonMapping(OVRInput.Button throwButtton, InputFeatureUsage<bool> throwUsage, OVRInput.Button rotateButton)
        {
            ThrowButtton = throwButtton;
            RotateButton = rotateButton;
            ThrowUsage = throwUsage;
        }

        public static ButtonMapping DefaultMapping = new ButtonMapping(OVRInput.Button.PrimaryIndexTrigger, CommonUsages.triggerButton, OVRInput.Button.PrimaryThumbstickLeft);
    }
}
