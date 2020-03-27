using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace TrickSaber
{
    internal class TrickSaberSettingsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "TrickSaber.TrickSaberSettings.bsml";

        [UIValue("UseTrigger-value")]
        public bool UseTrigger {
            get => PluginConfig.Instance.UseTrigger;
            set => PluginConfig.Instance.UseTrigger = value;
        }

        [UIValue("UseGrip-value")]
        public bool UseGrip
        {
            get => PluginConfig.Instance.UseGrip;
            set => PluginConfig.Instance.UseGrip = value;
        }

        [UIValue("ThumbDir-value")]
        public string ThumbstickDir
        {
            get => PluginConfig.Instance.ThumstickDirection;
            set => PluginConfig.Instance.ThumstickDirection = value;
        }

        [UIValue("TriggerThresh-value")]
        public float TriggerThresh
        {
            get => PluginConfig.Instance.TriggerThreshold;
            set => PluginConfig.Instance.TriggerThreshold = value;
        }

        [UIValue("ThumbThresh-value")]
        public float ThumbThresh
        {
            get => PluginConfig.Instance.ThumbstickThreshold;
            set => PluginConfig.Instance.ThumbstickThreshold = value;
        }

        [UIValue("GripThresh-value")]
        public float GripThresh
        {
            get => PluginConfig.Instance.GripThreshold;
            set => PluginConfig.Instance.GripThreshold = value;
        }

        [UIValue("ControllerSnapThresh-value")]
        public float ControllerSnapThresh
        {
            get => PluginConfig.Instance.ControllerSnapThreshold;
            set => PluginConfig.Instance.ControllerSnapThreshold = value;
        }

        [UIValue("DirEnum-list")]
        public List<object> trailType = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();
    }
}