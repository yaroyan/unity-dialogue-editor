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
        string _fileName = "DialogueCSV_Load.csv";
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
        /// CSVからデータをロードします。
        /// </summary>
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
                                LoadIntoDialogueNodeText(record, header, text);

                        foreach (var nodeData in dialogueContainer.ChoiceDatas)
                            LoadIntoChoiceNode(record, header, nodeData);

                        EditorUtility.SetDirty(dialogueContainer);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        void LoadIntoDialogueNodeText(IReadOnlyList<string> record, IReadOnlyList<string> header, DialogueDataText text)
        {
            if (record[2] != text.GUID.Value) return;
            for (var i = 0; i < record.Count; i++)
                foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
                {
                    if (header[i] != languageType.ToString()) continue;
                    text.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                }
        }

        void LoadIntoChoiceNode(IReadOnlyList<string> record, IReadOnlyList<string> header, ChoiceData nodeData)
        {
            if (record[1] != nodeData.GUID) return;
            for (var i = 0; i < record.Count; i++)
                foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
                {
                    if (header[i] != languageType.ToString()) continue;
                    nodeData.Texts.Find(x => x.LanguageType == languageType).LanguageGenericType = record[i];
                }
        }
    }
}