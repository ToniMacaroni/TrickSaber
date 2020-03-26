using HarmonyLib;
using UnityEngine;
using Logger = IPA.Logging.Logger;

namespace TrickSaber
{
    [HarmonyPatch(typeof(Saber))]
    [HarmonyPatch("Start")]
    public class SaberPatch
    {
        public static void Prefix(Saber __instance, VRController ____vrController)
        {
            Plugin.Log.Debug("Hooked Saber Start");
            TrickManager trickManager = new GameObject("TrickManager_"+__instance.saberType).AddComponent<TrickManager>();
            trickManager.Saber = __instance;
            trickManager.Controller = ____vrController;
        }
    }
}