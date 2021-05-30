using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using HMUI;
using SiraUtil;
using TrickSaber.UI;
using TrickSaber.ViewControllers;
using UnityEngine;
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