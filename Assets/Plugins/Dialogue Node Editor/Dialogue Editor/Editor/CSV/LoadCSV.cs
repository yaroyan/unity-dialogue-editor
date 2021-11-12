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
        // static readonly string _directoryPath = "Resources/Dialogue Editor/CSV";
        // string _fileName = "DialogueCSV_Load.csv";
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
            // var path = $"{Application.dataPath}/{_directoryPath}/{_fileName}";
            // using (var cr = new CsvReader(new StreamReader(path), this._config))
            // {
            //     // ヘッダーの読み込み
            //     cr.Read();
            //     cr.ReadHeader();
            //     var header = cr.HeaderRecord;

            //     // アセットのロード
            //     var dialogueContainers = Helper.FindAllDialogueContainerSO();

            //     // レコードの読み込み
            //     while (cr.Read())
            //     {
            //         var records = cr.Parser.Record;
            //         foreach (var dialogueContainer in dialogueContainers)
            //         {
            //             foreach (var nodeData in dialogueContainer.DialogueNodeDatas)
            //             {
            //                 LoadToNode(records, header, nodeData);
            //                 foreach (var nodePort in nodeData.DialogueNodePorts)
            //                 {
            //                     LoadToNodePort(records, header, nodePort);
            //                 }
            //             }
            //             EditorUtility.SetDirty(dialogueContainer);
            //             AssetDatabase.SaveAssets();
            //         }
            //     }
            // }
        }

        // /// <summary>
        // /// ノードにデータをロードします。
        // /// </summary>
        // /// <param name="records"></param>
        // /// <param name="header"></param>
        // /// <param name="nodeData"></param>
        // void LoadToNode(IReadOnlyList<string> records, IReadOnlyList<string> header, DialogueNodeData nodeData)
        // {
        // if (records[0] != nodeData.NodeGUID) return;
        // for (int i = 1; i < records.Count; i++) foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
        //     {
        //         if (header[i] != languageType.ToString()) continue;
        //         nodeData.TextLanguages.Find(x => x.LanguageType == languageType).LanguageGenericType = records[i];
        //     }
        // }

        // /// <summary>
        // /// ポートにデータをロードします。
        // /// </summary>
        // /// <param name="records"></param>
        // /// <param name="header"></param>
        // /// <param name="nodePort"></param>
        // void LoadToNodePort(IReadOnlyList<string> records, IReadOnlyList<string> header, DialogueNodePort nodePort)
        // {
        // if (records[0] != nodePort.PortGUID) return;
        // for (int i = 1; i < records.Count; i++) foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
        //     {
        //         if (header[i] != languageType.ToString()) continue;
        //         nodePort.TextLangueages.Find(language => language.LanguageType == languageType).LanguageGenericType = records[i];
        //     }
        // }
    }
}