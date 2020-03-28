using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace TrickSaber.ViewControllers
{
    class MiscViewController : BSMLResourceViewController
    {
        public override string ResourceName => "TrickSaber.Views.MiscView.bsml";

        [UIValue("ThumbDir-value")]
        public string ThumbstickDir
        {
            get => PluginConfig.Instance.ThumstickDirection;
            set => PluginConfig.Instance.ThumstickDirection = value;
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

        [UIValue("EnableCutting-value")]
        public bool EnableCutting
        {
            get => PluginConfig.Instance.EnableCuttingDuringTrick;
            set => PluginConfig.Instance.EnableCuttingDuringTrick = value;
        }

        [UIValue("DirEnum-list")]
        public List<object> ThumbstickDirectionsList = Enum.GetNames(typeof(ThumstickDir)).ToList<object>();

        [UIValue("SpinDirEnum-list")]
        public List<object> SpinDirectionsList = Enum.GetNames(typeof(SpinDir)).ToList<object>();
    }
}
