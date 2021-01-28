using System;
using IPA.Utilities;
using SiraUtil;
using SiraUtil.Tools;
using TrickSaber.InputHandling;
using TrickSaber.Tricks;
using UnityEngine;
using Zenject;

namespace TrickSaber.Installers
{
    internal class GameInstaller : Installer
    {
        private readonly SiraLog _logger;

        private GameInstaller(SiraLog logger)
        {
            _logger = logger;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplayManager>().AsSingle();
            Container.Bind<GlobalTrickManager>().AsSingle();

            Container.Bind<MovementController>().FromNewComponentSibling().AsTransient();
            Container.Bind<InputManager>().AsTransient();

            Container.Bind<SpinTrick>().FromNewComponentSibling().AsTransient();
            Container.Bind<ThrowTrick>().FromNewComponentSibling().AsTransient();

            if (!Container.HasBinding<SaberManager>() || !Container.HasBinding<PlayerVRControllersManager>())
            {
                _logger.Error("Saber or Cotroller not bound");
                return;
            }

            var saberManager = Container.Resolve<SaberManager>();
            var controllers = Container.Resolve<PlayerVRControllersManager>();

            var leftController =
                controllers.GetField<VRController, PlayerVRControllersManager>("_leftHandVRController");
            var rightController =
                controllers.GetField<VRController, PlayerVRControllersManager>("_rightHandVRController");

            if (!leftController || !rightController)
            {
                _logger.Error("controllers not assigned");
                return;
            }

            Container
                .BindInterfacesAndSelfTo<SaberTrickManager>()
                .FromNewComponentOn(saberManager.leftSaber.gameObject).AsCached()
                .WithConcreteId(SaberType.SaberA)
                .WithArguments(saberManager.leftSaber, leftController);

            Container
                .BindInterfacesAndSelfTo<SaberTrickManager>()
                .FromNewComponentOn(saberManager.rightSaber.gameObject).AsCached()
                .WithConcreteId(SaberType.SaberB)
                .WithArguments(saberManager.rightSaber, rightController);
        }
    }
}