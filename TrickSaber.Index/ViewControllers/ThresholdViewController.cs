using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace TrickSaber.Index.ViewControllers
{
    internal class ThresholdViewController : BSMLResourceViewController
    {
        public override string ResourceName => "TrickSaber.Index.Views.ThresholdView.bsml";

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
    }
}