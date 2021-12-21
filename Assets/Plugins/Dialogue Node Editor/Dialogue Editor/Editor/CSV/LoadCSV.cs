using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Dialogue.Editor
{
    public class LoadCSV
    {
        string _fileName = "DialogueCSV_Save.csv";
        CsvConfiguration _config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            // ヘッダーの有無の指定
            HasHeaderRecord = true,
            // 区切り文字の指定
            Delimiter = ",",
            // 空行の無視
            IgnoreBlankLines = true,
            // コメント行の指定
            Comment = '#',
            // コメントの許可の指定
            AllowComments = true,
        };

        /// <summary>
        /// Load data from CSV
        /// </summary>
        /// <remarks>
        /// Structure of Header of CSV <br/>
        /// | Dialogue Name | Node GUID | Text GUID | Languages... |
        /// </remarks>
        public void Load()
        {
            var path = $"{Helper.GetResourcesPath()}/{_fileName}";
            using (var cr = new CsvReader(new StreamReader(path), this._config))
            {
                // ヘッダーの読み込み
                cr.Read();
                cr.ReadHeader();
                var header = cr.HeaderRecord;

                // アセットのロード
                var dialogueContainers = Helper.FindAllSOResources<DialogueContainerSO>();

                // レコードの読み込み
                while (cr.Read())
                {
                    var record = cr.Parser.Record;
                    foreach (var dialogueContainer in dialogueContainers)
                    {
                        foreach (var nodeData in dialogueContainer.DialogueDatas)
                            foreach (var text in nodeData.DialogueDataTexts)
                            {
                                if (record[2] != text.GUID.Value) continue;
                                for (var i = 0; i < record.Length; i++)
                                    foreach (LanguageType languageType in (Enum.GetValues(typeof(LanguageType)) as LanguageType[]).Where(e => e.ToString() == header[i]))
                                        text.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                            }

                        foreach (var nodeData in dialogueContainer.ChoiceDatas)
                        {
                            if (record[1] != nodeData.GUID) continue;
                            for (var i = 0; i < record.Length; i++)
                                foreach (LanguageType languageType in (Enum.GetValues(typeof(LanguageType)) as LanguageType[]).Where(e => e.ToString() == header[i]))
                                    nodeData.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                        }

                        EditorUtility.SetDirty(dialogueContainer);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        /// <summary>
        /// Load data from CSV
        /// </summary>
        /// <remarks>
        /// Structure of Header of CSV <br/>
        /// | Node GUID | Text GUID | Languages... |
        /// </remarks>
        public void LoadByFile()
        {
            var languageTypes = Enum.GetValues(typeof(LanguageType)) as LanguageType[];
            foreach (var dialogueContainer in Helper.FindAllSOResources<DialogueContainerSO>())
                using (var cr = new CsvReader(new StreamReader($"{Helper.GetResourcesPath()}/{dialogueContainer.name}"), this._config))
                {
                    // ヘッダーの読み込み
                    cr.Read();
                    cr.ReadHeader();
                    var header = cr.HeaderRecord;

                    // レコードの読み込み
                    while (cr.Read())
                    {
                        var record = cr.Parser.Record;

                        foreach (var nodeData in dialogueContainer.DialogueDatas)
                            foreach (var text in nodeData.DialogueDataTexts)
                            {
                                if (record[1] != text.GUID.Value) continue;
                                for (var i = 0; i < record.Length; i++)
                                {
                                    foreach (LanguageType languageType in (Enum.GetValues(typeof(LanguageType)) as LanguageType[]).Where(e => e.ToString() == header[i]))
                                        text.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                                }
                            }

                        foreach (var nodeData in dialogueContainer.ChoiceDatas)
                        {
                            if (record[0] != nodeData.GUID) return;
                            for (var i = 0; i < record.Length; i++)
                                foreach (LanguageType languageType in (Enum.GetValues(typeof(LanguageType)) as LanguageType[]).Where(e => e.ToString() == header[i]))
                                    nodeData.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                        }

                        EditorUtility.SetDirty(dialogueContainer);
                        AssetDatabase.SaveAssets();
                    }
                }
        }
    }
}