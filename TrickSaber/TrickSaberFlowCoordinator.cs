using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using HMUI;

namespace TrickSaber
{
    internal class TrickSaberFlowCoordinator : FlowCoordinator
    {
        public void Awake()
        {
            if (!settingsViewController)
            {
                this.settingsViewController = BeatSaberUI.CreateViewController<TrickSaberSettingsViewController>();
            }
        }

        protected override void DidActivate(bool firstActivation, FlowCoordinator.ActivationType activationType)
        {
            try
            {
                if (firstActivation)
                {
                    base.title = "Trick Saber";
                    base.showBackButton = true;
                    base.ProvideInitialViewControllers(this.settingsViewController, null, null, null, null);
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

        private TrickSaberSettingsViewController settingsViewController;
    }
}
