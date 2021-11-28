using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Dialogue.Editor
{
    public static class Helper
    {

        public static readonly string s_resourcesPath = $"{Application.dataPath}/Resources";

        public static string GetResourcesPath()
        {
            if (!Directory.Exists(s_resourcesPath)) Directory.CreateDirectory(s_resourcesPath);
            return s_resourcesPath;
        }

        public static List<T> FindAllObjectFromResources<T>()
        {
            var list = new List<T>();
            var directories = Directory.GetDirectories(GetResourcesPath(), "*", SearchOption.AllDirectories);

            foreach (var directory in directories)
            {
                var directoryPath = directory.Substring(s_resourcesPath.Length + 1);
                var results = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>();
                foreach (var result in results) if (!list.Contains(result)) list.Add(result);
            }
            return list;
        }

        public static IEnumerable<T> FindAllSOResources<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(GUID => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(GUID)));
        }
    }

    public enum CsvMode
    {
        ALL = 1,
        DIRECTORY = 2,
        FILE = 3
    }
}
