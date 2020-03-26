using UnityEngine;

namespace TrickSaber
{
    public class AssetLoader : MonoBehaviour
    {
        public static AssetBundle Bundle;

        public static void Load()
        {
            Bundle = AssetBundle.LoadFromFile(@"E:\SteamLibrary\steamapps\common\Beat Saber\UserData\TrickSaber\prefabs");
        }

        public static UnityEngine.Object GetAsset(string name)
        {
            if(Bundle==null)Load();
            return Bundle.LoadAsset(name);
        }

        public static UnityEngine.Object GetHandLeftPrefab()
        {
            return GetAsset("HandLeft");
        }

        public static UnityEngine.Object GetHandRightPrefab()
        {
            return GetAsset("HandRight");
        }
    }
}