using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using CsvHelper;

namespace Dialogue.Editor
{
    public class SaveCSV
    {
        static readonly string s_directoryPath = "Resources/Dialogue Editor/CSV";
        static readonly string s_fileName = "DialogueCSV_Save.csv";
        List<string> _headers;
        static readonly string s_nodeId = "Node GUID";
        static readonly string s_textId = "Text GUID";
        static readonly string s_dialogueName = "Dialogue Name";

        public void Save()
        {
            var dialogueContainers = Helper.FindAllDialogueContainerSO();

            CreateFile();

            using (var cw = new CsvWriter(new StreamWriter(path: GetFilePath(), append: true), System.Globalization.CultureInfo.InvariantCulture))
            {
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
                        texts.Add("Choice Node");
                        foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                            texts.Add(nodeData.Texts.Find(language => language.LanguageType == languageType).LanguageGenericType);
                        cw.WriteField(texts);
                        cw.NextRecord();
                    }
                }
            }
        }

        void CreateFile()
        {
            VerifyDirectory();
            // MakeHeader();
            WriteHeader();
        }

        void MakeHeader()
        {
            var headerTexts = new List<string>();
            headerTexts.Add(s_dialogueName);
            headerTexts.Add(s_nodeId);
            headerTexts.Add(s_textId);

            foreach (var language in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                headerTexts.Add(language.ToString());
            this._headers = headerTexts;
        }

        void WriteHeader()
        {
            var headerTexts = new List<string>();
            headerTexts.Add(s_dialogueName);
            headerTexts.Add(s_nodeId);
            headerTexts.Add(s_textId);

            foreach (var language in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
                headerTexts.Add(language.ToString());

            using (var cw = new CsvWriter(new StreamWriter(GetFilePath()), System.Globalization.CultureInfo.InvariantCulture))
            {
                cw.WriteField(headerTexts);
            }

            this._headers = headerTexts;
        }

        void VerifyDirectory()
        {
            var directory = GetDirectoryPath();
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        public string GetDirectoryPath() => $"{Application.dataPath}/{s_directoryPath}";

        public string GetFilePath() => $"{GetDirectoryPath()}/{s_fileName}";
    }
}