using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPA.Logging;
using SiraUtil;
using TrickSaber.Configuration;
using Zenject;

namespace TrickSaber.Installers
{
    class AppInstaller : Installer
    {
        private readonly PluginConfig _config;
        private readonly Logger _logger;

        private AppInstaller(PluginConfig config, Logger logger)
        {
            _config = config;
            _logger = logger;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.BindLoggerAsSiraLogger(_logger);
            Container.BindInterfacesAndSelfTo<TrickSaberPlugin>().AsSingle();
        }
    }
}
