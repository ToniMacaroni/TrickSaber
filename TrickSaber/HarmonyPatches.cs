using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace TrickSaber
{
    [HarmonyPatch(typeof(Saber))]
    [HarmonyPatch("Start")]
    public class SaberPatch
    {
        public static void Prefix(Saber __instance, VRController ____vrController)
        {
            Plugin.Log.Debug("Hooked Saber Start");
            SaberTrickManager saberTrickManager = new GameObject("TrickManager_"+__instance.saberType).AddComponent<SaberTrickManager>();
            saberTrickManager.Saber = __instance;
            saberTrickManager.Controller = ____vrController;
        }
    }

    [HarmonyPatch(typeof(Cutter))]
    [HarmonyPatch("Cut")]
    public class CutterPatch
    {
        public static bool Prefix(Saber saber)
        {
            if (!PluginConfig.Instance.EnableCuttingDuringTrick)
            {
                if (saber.saberType == SaberType.SaberA && Globals.LeftSaberSaberTrickManager.IsDoingTrick) return false;
                if (saber.saberType == SaberType.SaberB && Globals.RightSaberSaberTrickManager.IsDoingTrick) return false;
            }
            return true;
        }
    }
}