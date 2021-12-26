using SiraUtil;
using TrickSaber.Configuration;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace TrickSaber.Installers
{
    class AppInstaller : Installer
    {
        private readonly PluginConfig _config;

        private AppInstaller(PluginConfig config)
        {
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.BindInterfacesAndSelfTo<TrickSaberPlugin>().AsSingle();
        }
    }
}
