using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using CsvHelper;

namespace Dialogue.Editor
{
    public class SaveCSV
    {
        /// <summary>
        /// Output to CSV
        /// </summary>
        /// <remarks>
        /// Structure of Header of CSV <br/>
        /// | Node GUID | Text GUID | Languages... |
        /// </remarks>
        public void SaveByFile()
        {
            var dialogueContainers = Helper.FindAllSOResources<DialogueContainerSO>();
            var languageTypes = Enum.GetValues(typeof(LanguageType)) as LanguageType[];

            foreach (var dialogueContainer in dialogueContainers)
            {
                using (var cw = new CsvWriter(new StreamWriter(path: GetFilePath(dialogueContainer.name)), System.Globalization.CultureInfo.InvariantCulture))
                {
                    var header = new List<string>() { "Node GUID", "Text GUID" };
                    header.AddRange(languageTypes.Select(language => language.ToString()));
                    cw.WriteField(header);
                    cw.NextRecord();

                    foreach (var nodeData in dialogueContainer.DialogueDatas)
                        foreach (var text in nodeData.DialogueDataTexts)
                        {
                            var texts = new List<string>() { nodeData.GUID, text.GUID.Value };
                            foreach (var languageType in languageTypes) texts.Add(text.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                            cw.WriteField(texts);
                            cw.NextRecord();
                        }

                    foreach (var nodeData in dialogueContainer.ChoiceDatas)
                    {
                        var texts = new List<string>() { nodeData.GUID, "Choice Node doesn't have TEXT GUID" };
                        foreach (var languageType in languageTypes) texts.Add(nodeData.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                        cw.WriteField(texts);
                        cw.NextRecord();
                    }
                }
                Debug.Log(GetFilePath(dialogueContainer.name));
            }
        }

        /// <summary>
        /// Output to CSV
        /// </summary>
        /// <remarks>
        /// Structure of Header of CSV <br/>
        /// | Dialogue Name | Node GUID | Text GUID | Languages... |
        /// </remarks>
        public void Save()
        {
            var dialogueContainers = Helper.FindAllSOResources<DialogueContainerSO>();
            var languageTypes = Enum.GetValues(typeof(LanguageType)) as LanguageType[];

            using (var cw = new CsvWriter(new StreamWriter(path: GetFilePath("DialogueCSV_Save.csv")), System.Globalization.CultureInfo.InvariantCulture))
            {
                var header = new List<string>() { "Dialogue Name", "Node GUID", "Text GUID" };
                header.AddRange(languageTypes.Select(e => e.ToString()));
                cw.WriteField(header);
                cw.NextRecord();

                foreach (var dialogueContainer in dialogueContainers)
                {
                    foreach (var nodeData in dialogueContainer.DialogueDatas)
                        foreach (var text in nodeData.DialogueDataTexts)
                        {
                            var texts = new List<string>() { dialogueContainer.name, nodeData.GUID, text.GUID.Value };
                            foreach (var languageType in languageTypes) texts.Add(text.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                            cw.WriteField(texts);
                            cw.NextRecord();
                        }
                    foreach (var nodeData in dialogueContainer.ChoiceDatas)
                    {
                        var texts = new List<string>() { dialogueContainer.name, nodeData.GUID, "Choice Node doesn't have TEXT GUID" };
                        foreach (var languageType in languageTypes) texts.Add(nodeData.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                        cw.WriteField(texts);
                        cw.NextRecord();
                    }
                }
            }
        }

        /// <summary>
        /// Get path to CSV file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFilePath(string fileName)
        {
            var path = $"{Helper.GetResourcesPath()}/Dialogue Editor/CSV";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return $"{path}/{fileName}.csv";
        }
    }
}