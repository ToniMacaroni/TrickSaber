using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using IPA;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        public static Logger Log { get; set; }
        public static Harmony Harmony { get; set; }

        [Init]
        public Plugin(Logger logger)
        {
            Log = logger;

            Harmony = new Harmony("tricksaber.toni.com");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnStart]
        public void OnStart()
        {
            Log.Debug("TrickSaber Started");
        }

        [OnExit]
        public void OnExit()
        {
        }
    }
}
