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

        [UIValue("TriggerAction-value")]
        public string UseTrigger {
            get => PluginConfig.Instance.TriggerAction;
            set => PluginConfig.Instance.TriggerAction = value;
        }

        [UIValue("GripAction-value")]
        public string GripAction
        {
            get => PluginConfig.Instance.GripAction;
            set => PluginConfig.Instance.GripAction = value;
        }

        [UIValue("ThumbAction-value")]
        public string ThumbAction
        {
            get => PluginConfig.Instance.ThumbstickAction;
            set => PluginConfig.Instance.ThumbstickAction = value;
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

        [UIValue("SpinSpeed-value")]
        public float SpinSpeed
        {
            get => PluginConfig.Instance.SpinSpeed;
            set => PluginConfig.Instance.SpinSpeed = value;
        }

        [UIValue("SpinDir-value")]
        public string SpinDir
        {
            get => PluginConfig.Instance.SpinDirection;
            set => PluginConfig.Instance.SpinDirection = value;
        }

        [UIValue("ThrowVelocity-value")]
        public float ThrowVelocity
        {
            get => PluginConfig.Instance.ThrowVelocity;
            set => PluginConfig.Instance.ThrowVelocity = value;
        }

        [UIValue("DirEnum-list")]
        public List<object> ThumbstickDirectionsList = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();

        [UIValue("SpinDirEnum-list")]
        public List<object> SpinDirectionsList = Enum.GetNames(typeof(SpinDir)).ToList<object>();

        [UIValue("TrickActionEnum-list")]
        public List<object> TrickActionList = Enum.GetNames(typeof(TrickAction)).ToList<object>();
    }
}