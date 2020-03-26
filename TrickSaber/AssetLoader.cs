using System;
using System.IO;
using UnityEngine;

namespace TrickSaber
{
    //for future use
    public class AssetLoader : MonoBehaviour
    {
        public static AssetBundle Bundle;

        public static void Load()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "UserData\\TrickSaber\\prefabs");
            Bundle = AssetBundle.LoadFromFile(path);
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