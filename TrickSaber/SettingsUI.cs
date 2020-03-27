using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace TrickSaber
{
    internal class SettingsUI
    {
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
            {
                TrickSaberFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<TrickSaberFlowCoordinator>();
            }
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(TrickSaberFlowCoordinator);
        }

        public static TrickSaberFlowCoordinator TrickSaberFlowCoordinator;
        public static bool Created;
    }
}
