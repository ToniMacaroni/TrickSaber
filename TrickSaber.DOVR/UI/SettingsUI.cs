using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using TrickSaber.DOVR.UI;

namespace TrickSaber.DOVR
{
    internal class SettingsUI
    {
        public static TrickSaberFlowCoordinator TrickSaberFlowCoordinator;
        public static bool Created;

        public static void CreateMenu()
        {
            if (!Created)
            {
                MenuButton menuButton = new MenuButton("Trick Saber", "Change your tricks!", ShowFlow);
                MenuButtons.instance.RegisterButton(menuButton);
                Created = true;
            }
        }

        public static void ShowFlow()
        {
            if (TrickSaberFlowCoordinator == null)
                TrickSaberFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<TrickSaberFlowCoordinator>();
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(TrickSaberFlowCoordinator);
        }
    }
}