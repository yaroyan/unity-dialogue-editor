namespace Dialogue
{
    /// <summary>
    /// 終了ノードタイプ
    /// </summary>
    public enum EndNodeType
    {
        /// <summary>
        /// 終了
        /// </summary>
        End = 1,
        /// <summary>
        /// 繰り返し
        /// </summary>
        Repeat = 2,
        /// <summary>
        /// 戻る
        /// </summary>
        GoBack = 3,
        /// <summary>
        /// 最初に戻る
        /// </summary>
        ReturnToStart = 4,
    }
}