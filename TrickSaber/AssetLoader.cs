using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TrickSaber
{
    //For future use
    public class AssetLoader : MonoBehaviour
    {
        public static AssetBundle Bundle;

        public static void Load()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "UserData\\TrickSaber\\prefabs");
            Bundle = AssetBundle.LoadFromFile(path);
        }

        public static Object GetAsset(string name)
        {
            if(Bundle==null)Load();
            return Bundle.LoadAsset(name);
        }

        public static Object GetHandLeftPrefab()
        {
            return GetAsset("HandLeft");
        }

        public static Object GetHandRightPrefab()
        {
            return GetAsset("HandRight");
        }
    }
}