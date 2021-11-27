using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue.Editor
{
    public class CustomTools
    {
        [MenuItem("Tools/Dialogue Editor/Save to CSV")]
        public static void SaveToCSV()
        {
            SaveCSV saveCSV = new SaveCSV();
            saveCSV.Save();

            EditorApplication.Beep();
            Debug.Log("<color=green> Save to CSV file Successfully </color>");
        }

        [MenuItem("Tools/Dialogue Editor/Load from CSV")]
        public static void LoadFromCSV()
        {
            LoadCSV loadCSV = new LoadCSV();
            loadCSV.Load();

            EditorApplication.Beep();
            Debug.Log("<color=green> Load from CSV file Successfully </color>");
        }
    }
}