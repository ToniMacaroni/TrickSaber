using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrickSaber
{
    public struct ButtonMapping
    {
        public OVRInput.Button ThrowButtton;
        public OVRInput.Button RotateButton;

        public ButtonMapping(OVRInput.Button throwButtton, OVRInput.Button rotateButton)
        {
            ThrowButtton = throwButtton;
            RotateButton = rotateButton;
        }

        public static ButtonMapping LeftButtons = new ButtonMapping(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Button.PrimaryThumbstickLeft);
        public static ButtonMapping RightButtons = new ButtonMapping(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Button.SecondaryThumbstickRight);
    }
}
