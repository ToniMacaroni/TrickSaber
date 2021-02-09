using SiraUtil;
using TrickSaber.UI;
using TrickSaber.ViewControllers;
using Zenject;

namespace TrickSaber.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<BindingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<MiscViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<ThresholdViewController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<SettingsUI>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrickSaberFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}