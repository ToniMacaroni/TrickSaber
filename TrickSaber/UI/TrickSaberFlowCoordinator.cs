using System;
using BeatSaberMarkupLanguage;
using HMUI;
using TrickSaber.ViewControllers;

namespace TrickSaber.UI
{
    internal class TrickSaberFlowCoordinator : FlowCoordinator
    {
        private BindingsViewController _bindingsViewController;
        private MiscViewController _miscViewController;
        private ThresholdViewController _thresholdViewController;

        public void Awake()
        {
            if (!_bindingsViewController)
                _bindingsViewController = BeatSaberUI.CreateViewController<BindingsViewController>();
            if (!_thresholdViewController)
                _thresholdViewController = BeatSaberUI.CreateViewController<ThresholdViewController>();
            if (!_miscViewController) _miscViewController = BeatSaberUI.CreateViewController<MiscViewController>();
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    title = "Trick Settings";
                    showBackButton = true;
                    ProvideInitialViewControllers(_bindingsViewController, _miscViewController, _thresholdViewController);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}