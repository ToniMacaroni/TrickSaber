using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS_Utils.Gameplay;

namespace TrickSaber
{
    public class GameplayManager
    {
        public static void DisableScoreSubmissionIfNeeded()
        {
            if(PluginConfig.Instance.EnableCuttingDuringTrick) ScoreSubmission.DisableSubmission("TrickSaber");
        }

        public static void OnGameSceneLoaded()
        {

            DisableScoreSubmissionIfNeeded();
        }

        public static void OnMenuSceneLoadedFresh()
        {
            Globals.TransformOffset = UnityEngine.Object.FindObjectOfType<VRControllerTransformOffset>();
        }
    }
}
