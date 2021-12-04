using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.Json;

namespace Dialogue.Editor.Logger
{
    /// <summary>
    /// タイムスタンプを記録したログファイルを生成するクラス
    /// </summary>
    public class TimeStampLogger
    {
        // ログファイルの出力先パス
        string _logPath;
        // タイムスタンプを格納する辞書
        Dictionary<string, Dictionary<string, DateTime>> _log;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logPath">ログファイルの出力先のパス</param>
        public TimeStampLogger(string logPath)
        {
            this._logPath = logPath;
            this._log = File.Exists(logPath) ? JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, DateTime>>>(File.ReadAllText(logPath)) : new Dictionary<string, Dictionary<string, DateTime>>();
        }

        /// <summary>
        /// タイムスタンプを生成します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Dictionary<string, DateTime> CreateTimeStamp(string filePath)
        {
            // タイムスタンプを生成
            var timeStamp = new Dictionary<string, DateTime>()
            {
                // 作成日時
                {"CreationTime", File.GetCreationTime(filePath)},
                // 最終更新日時
                {"LastWriteTime", File.GetLastWriteTime(filePath)},
                // 最終アクセス日時
                {"LastAccessTime", File.GetLastAccessTime(filePath)},
            };
            return timeStamp;
        }

        /// <summary>
        /// タイムスタンプをログに追加します
        /// </summary>
        /// <param name="filePath"></param>
        public void Add(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var timeStamp = CreateTimeStamp(filePath);
            this._log[fileName] = timeStamp;
        }

        public bool CanAddLog(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var canInsert = !this._log.ContainsKey(fileName);
            var canUpdate = !canInsert && _log[fileName]["LastWriteTime"] < File.GetLastWriteTime(filePath);
            return canInsert || canUpdate;
        }

        /// <summary>
        /// ログファイルをJSON形式で出力します
        /// </summary>
        /// <param name="fileName">対象ファイルの名前</param>
        public void Generate()
        {
            File.WriteAllText(this._logPath, JsonSerializer.Serialize(this._log));
        }
    }
}
