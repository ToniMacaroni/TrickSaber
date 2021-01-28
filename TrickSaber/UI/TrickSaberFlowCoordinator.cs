using System;
using BeatSaberMarkupLanguage;
using HMUI;
using TrickSaber.ViewControllers;
using Zenject;

namespace TrickSaber.UI
{
    internal class TrickSaberFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator _mainFlowCoordinator;

        private BindingsViewController _bindingsViewController;
        private MiscViewController _miscViewController;
        private ThresholdViewController _thresholdViewController;

        [Inject]
        public void Construct(
            MainFlowCoordinator mainFlowCoordinator,
            BindingsViewController bindingsViewController,
            MiscViewController miscViewController,
            ThresholdViewController thresholdViewController)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _bindingsViewController = bindingsViewController;
            _miscViewController = miscViewController;
            _thresholdViewController = thresholdViewController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                showBackButton = true;
                SetTitle("Tricksaber");
                ProvideInitialViewControllers(_bindingsViewController, _miscViewController, _thresholdViewController);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}