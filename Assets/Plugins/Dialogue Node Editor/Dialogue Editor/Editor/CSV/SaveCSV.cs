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
        public void SaveByFile()
        {
            var dialogueContainers = Helper.FindAllSOResources<DialogueContainerSO>();
            var languageTypes = Enum.GetValues(typeof(LanguageType)) as LanguageType[];

            foreach (var dialogueContainer in dialogueContainers)
            {
                using (var cw = new CsvWriter(new StreamWriter(path: GetFilePath(dialogueContainer.name)), System.Globalization.CultureInfo.InvariantCulture))
                {
                    // ヘッダの書き込み
                    var header = new List<string>() { "Node GUID", "Text GUID" };
                    header.AddRange(languageTypes.Select(language => language.ToString()));
                    cw.WriteField(header);
                    cw.NextRecord();

                    foreach (var nodeData in dialogueContainer.DialogueDatas)
                        foreach (var text in nodeData.DialogueDataTexts)
                        {
                            var texts = new List<string>() { nodeData.GUID, text.GUID.Value };
                            foreach (var languageType in languageTypes)
                                texts.Add(text.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                            cw.WriteField(texts);
                            cw.NextRecord();
                        }

                    foreach (var nodeData in dialogueContainer.ChoiceDatas)
                    {
                        var texts = new List<string>() { nodeData.GUID, "Choice Node doesn't have TEXT GUID" };
                        foreach (var languageType in languageTypes)
                            texts.Add(nodeData.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                        cw.WriteField(texts);
                        cw.NextRecord();
                    }
                }
                Debug.Log(GetFilePath(dialogueContainer.name));
            }
        }

        public void Save()
        {
            var dialogueContainers = Helper.FindAllSOResources<DialogueContainerSO>();

            using (var cw = new CsvWriter(new StreamWriter(path: GetFilePath("DialogueCSV_Save.csv")), System.Globalization.CultureInfo.InvariantCulture))
            {

                var header = GetHeader();
                cw.WriteField(header);
                cw.NextRecord();

                foreach (var dialogueContainer in dialogueContainers)
                {
                    foreach (var nodeData in dialogueContainer.DialogueDatas)
                        foreach (var text in nodeData.DialogueDataTexts)
                        {
                            var texts = new List<string>();

                            texts.Add(dialogueContainer.name);
                            texts.Add(nodeData.GUID);
                            texts.Add(text.GUID.Value);

                            foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                                texts.Add(text.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                            cw.WriteField(texts);
                            cw.NextRecord();

                        }
                    foreach (var nodeData in dialogueContainer.ChoiceDatas)
                    {
                        var texts = new List<string>();
                        texts.Add(dialogueContainer.name);
                        texts.Add(nodeData.GUID);
                        texts.Add("Choice Node doesn't have TEXT GUID");
                        foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                            texts.Add(nodeData.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                        cw.WriteField(texts);
                        cw.NextRecord();
                    }
                }
            }
        }

        List<string> GetHeader()
        {
            var headerTexts = new List<string>() { "Dialogue Name", "Node GUID", "Text GUID" };
            foreach (var language in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                headerTexts.Add(language.ToString());
            return headerTexts;
        }

        public string GetFilePath(string fileName)
        {
            var path = $"{Helper.GetResourcesPath()}/Dialogue Editor/CSV";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return $"{path}/{fileName}.csv";
        }
    }
}