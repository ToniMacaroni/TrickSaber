using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatmapEditor3D;

namespace TrickSaber
{
    public class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public bool UseTrigger { get; set; } = true;

        public bool UseGrip { get; set; } = false;

        public string ThumstickDirection { get; set; } = ThumstickDir.Horizontal.ToString();

        public float TriggerThreshold { get; set; } = 0.8f;

        public float GripThreshold { get; set; } = 0.8f;

        public float ThumbstickThreshold { get; set; } = 0.8f;

        public float ControllerSnapThreshold { get; set; } = 0.3f;
    }
}
