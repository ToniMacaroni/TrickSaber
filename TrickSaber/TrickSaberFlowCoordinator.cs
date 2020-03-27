using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using HMUI;
using TrickSaber.ViewControllers;

namespace TrickSaber
{
    internal class TrickSaberFlowCoordinator : FlowCoordinator
    {
        public void Awake()
        {
            if (!bindingsViewController) bindingsViewController = BeatSaberUI.CreateViewController<BindingsViewController>();
            if (!thresholdViewController) thresholdViewController = BeatSaberUI.CreateViewController<ThresholdViewController>();
            if (!miscViewController) miscViewController = BeatSaberUI.CreateViewController<MiscViewController>();
        }

        protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    base.title = "Trick Settings";
                    base.showBackButton = true;
                    base.ProvideInitialViewControllers(bindingsViewController, miscViewController, thresholdViewController, null, null);
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

        private BindingsViewController bindingsViewController;
        private ThresholdViewController thresholdViewController;
        private MiscViewController miscViewController;
    }
}
