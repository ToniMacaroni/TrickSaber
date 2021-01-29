using System;
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
            Container.BindInterfacesAndSelfTo<GlobalTrickManager>().AsSingle();

            Container.Bind<MovementController>().FromNewComponentSibling().AsTransient();
            Container.Bind<InputManager>().AsTransient();

            Container.BindFactory<Type, GameObject, Trick, Trick.Factory>().FromFactory<Trick.CustomFactory>();

            Container.Bind<SaberControllerBearer>().AsSingle();

            //TODO: make SaberTrickManagers non-Monobehaviours

            BindTrickManager(SaberType.SaberA);
            BindTrickManager(SaberType.SaberB);

            Container.Bind<SaberTrickModel>().AsTransient();

            _logger.Info("Installed Everything");
        }

        private void BindTrickManager(SaberType saberType)
        {
            Container
                .Bind<SaberTrickManager>()
                .WithId(saberType)
                .FromNewComponentOn(GetSaber).AsCached()
                .WithArguments(saberType);
        }

        private GameObject GetSaber(InjectContext ctx)
        {
            var saberManager = ctx.Container.Resolve<SaberManager>();

            if (!saberManager)
            {
                _logger.Error("Couldn't resolve SaberManager");
                return null;
            }

            var saberType = (SaberType) ctx.Identifier;

            return saberType == SaberType.SaberA
                ? saberManager.leftSaber.gameObject
                : saberManager.rightSaber.gameObject;
        }
    }
}