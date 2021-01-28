using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TrickSaber.Configuration;
using Zenject;

namespace TrickSaber.ViewControllers
{
    internal class ThresholdViewController : BSMLResourceViewController
    {
        public override string ResourceName => "TrickSaber.Views.ThresholdView.bsml";

        [Inject] private readonly PluginConfig _config = null;

        [UIValue("TriggerThresh-value")]
        public float TriggerThresh
        {
            get => _config.TriggerThreshold;
            set => _config.TriggerThreshold = value;
        }

        [UIValue("ThumbThresh-value")]
        public float ThumbThresh
        {
            get => _config.ThumbstickThreshold;
            set => _config.ThumbstickThreshold = value;
        }

        [UIValue("GripThresh-value")]
        public float GripThresh
        {
            get => _config.GripThreshold;
            set => _config.GripThreshold = value;
        }

        [UIValue("ControllerSnapThresh-value")]
        public float ControllerSnapThresh
        {
            get => _config.ControllerSnapThreshold;
            set => _config.ControllerSnapThreshold = value;
        }
    }
}