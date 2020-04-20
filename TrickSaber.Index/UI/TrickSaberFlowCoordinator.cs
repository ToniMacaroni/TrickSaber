using System;
using BeatSaberMarkupLanguage;
using HMUI;
using TrickSaber.Index.ViewControllers;
using TrickSaber.ViewControllers;

namespace TrickSaber.Index.UI
{
    internal class TrickSaberFlowCoordinator : FlowCoordinator
    {
        private BindingsViewController bindingsViewController;
        private MiscViewController miscViewController;
        private ThresholdViewController thresholdViewController;

        public void Awake()
        {
            if (!bindingsViewController)
                bindingsViewController = BeatSaberUI.CreateViewController<BindingsViewController>();
            if (!thresholdViewController)
                thresholdViewController = BeatSaberUI.CreateViewController<ThresholdViewController>();
            if (!miscViewController) miscViewController = BeatSaberUI.CreateViewController<MiscViewController>();
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    title = "Trick Settings";
                    showBackButton = true;
                    ProvideInitialViewControllers(bindingsViewController, miscViewController, thresholdViewController);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, false);
        }
    }
}