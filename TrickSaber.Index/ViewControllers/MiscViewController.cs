using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace TrickSaber.Index.ViewControllers
{
    internal class MiscViewController : BSMLResourceViewController
    {
        [UIValue("SpinDirEnum-list")]
        public List<object> SpinDirectionsList = Enum.GetNames(typeof(SpinDir)).ToList<object>();

        [UIValue("DirEnum-list")]
        public List<object> ThumbstickDirectionsList = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();

        public override string ResourceName => "TrickSaber.Index.Views.MiscView.bsml";

        [UIValue("ThumbDir-value")]
        public string ThumbstickDir
        {
            get => PluginConfig.Instance.ThumstickDirection;
            set => PluginConfig.Instance.ThumstickDirection = value;
        }

        [UIValue("IsSpeedVelocityDependent-value")]
        public bool IsSpeedVelocityDependent
        {
            get => PluginConfig.Instance.IsSpeedVelocityDependent;
            set => PluginConfig.Instance.IsSpeedVelocityDependent = value;
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

        [UIValue("ReturnSpeed-value")]
        public float ReturnSpeed
        {
            get => PluginConfig.Instance.ReturnSpeed;
            set => PluginConfig.Instance.ReturnSpeed = value;
        }

        [UIValue("SlowmoDuringThrow-value")]
        public bool SlowmoDuringThrow
        {
            get => PluginConfig.Instance.SlowmoDuringThrow;
            set => PluginConfig.Instance.SlowmoDuringThrow = value;
        }

        [UIValue("EnableCutting-value")]
        public bool EnableCutting
        {
            get => PluginConfig.Instance.EnableCuttingDuringTrick;
            set => PluginConfig.Instance.EnableCuttingDuringTrick = value;
        }
    }
}