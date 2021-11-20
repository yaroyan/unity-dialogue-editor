using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System.Linq;

namespace Dialogue.Editor
{
    public class DialogueGraphView : GraphView
    {
        static readonly string s_graphViewStyleSheet = @"USS/Dialogue/GraphView/GraphView";
        DialogueEditorWindow _editorWindow;
        NodeSearchWindow _searchWindow;

        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            this._editorWindow = editorWindow;

            // ズーム機能をセット
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // スタイルシートのロード
            styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: s_graphViewStyleSheet));

            // ドラッグ機能
            this.AddManipulator(new ContentDragger());
            // 選択中のアイテムのドラッグ機能
            this.AddManipulator(new SelectionDragger());
            // 範囲選択機能
            this.AddManipulator(new RectangleSelector());
            // 選択機能
            this.AddManipulator(new FreehandSelector());

            // グリッドの表示
            var grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();

            // AddElement(GenerateEntryPointNode());
            AddSearchWindow();
        }

        /// <summary>
        /// ポートの互換性を検証します。
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                // 同一のノードであるか
                bool isSameNode = startPort.node == port.node;
                // 同一の方向のポートであるか
                bool isSameDirection = startPort.direction == port.direction;
                // 同一の型であるか
                bool isSameType = startPort.portType == port.portType;
                // 同一のポート色であるか
                bool isSameColor = startPort.portColor == port.portColor;
                // いずれかの条件に合致する場合、次のループに移行する
                if (isSameNode || isSameDirection || !isSameType || !isSameColor) continue;

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        // 検索窓を追加します。
        private void AddSearchWindow()
        {
            this._searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>().Initialize(_editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        // 言語設定を再読み込みします。
        public void ReloadLanguage()
        {
            var allNodes = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
            foreach (var node in allNodes) node.ReloadLanguage();
        }
        public T CreateNode<T>(Vector2 position) where T : BaseNode => System.Activator.CreateInstance(typeof(T), new object[] { position, this._editorWindow, this }) as T;
    }
}