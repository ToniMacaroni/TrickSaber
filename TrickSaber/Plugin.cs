using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SiraUtil.Zenject;
using TrickSaber.Configuration;
using TrickSaber.Installers;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {

        [Init]
        public Plugin(IPALogger logger, Config conf, Zenjector zenjector)
        {
            var pluginConfig = conf.Generated<PluginConfig>();

            zenjector.OnApp<AppInstaller>().WithParameters(pluginConfig, logger);
            zenjector.OnMenu<Installers.MenuInstaller>();
            zenjector.OnGame<GameInstaller>();
        }

        [OnEnable]
        public void OnEnable()
        {
        }

        [OnDisable]
        public void OnDisable()
        {
        }
    }
}
