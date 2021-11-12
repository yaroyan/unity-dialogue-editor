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

        // Old way  of find Dialogue Containers.
        // This work  with generic.
        // may work in run time need to check.
        // TODO: check if work in runtime at  some point.
        public static List<T> FindAllObjectFromResources<T>()
        {
            var list = new List<T>();
            // パスの取得
            string resourcesPath = $"{Application.dataPath}/Resources";
            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);

            var directories = Directory.GetDirectories(resourcesPath, "*", SearchOption.AllDirectories);

            foreach (var directory in directories)
            {
                var directoryPath = directory.Substring(resourcesPath.Length + 1);
                var results = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>().ToArray();
                foreach (var result in results) if (!list.Contains(result)) list.Add(result);
            }
            return list;
        }

        /// <summary>
        /// Find all Dialogue Container in Assets
        /// </summary>
        /// <returns>List of Dialogue Containers</returns>
        public static List<DialogueContainerSO> FindAllDialogueContainerSO()
        {
            // Find all the DialogueContainerSO in Assets and get it GUID.
            string[] GUIDs = AssetDatabase.FindAssets("DialogueContainerSO");
            // Make a Array  as long as we found DialogueContainerSO.
            var items = new DialogueContainerSO[GUIDs.Length];
            for (int i = 0; i < GUIDs.Length; i++)
            {
                // Use the GUID to find the Asset path.
                var path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                // Use path to find and load DialogueContainerSO.
                items[i] = AssetDatabase.LoadAssetAtPath<DialogueContainerSO>(path);
            }
            return items.ToList();
        }
    }
}
