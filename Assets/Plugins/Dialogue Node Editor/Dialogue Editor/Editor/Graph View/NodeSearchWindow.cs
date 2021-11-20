using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements.Experimental;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

namespace Dialogue.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        DialogueEditorWindow _editorWindow;
        DialogueGraphView _graphView;
        Texture2D _iconImage;

        /// <summary>
        /// 初期化用コンストラクタ代替メソッド
        /// </summary>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        /// <returns></returns>
        public NodeSearchWindow Initialize(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            this._editorWindow = editorWindow;
            this._graphView = graphView;

            this._iconImage = new Texture2D(1, 1);
            this._iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
            this._iconImage.Apply();

            return this;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            int level = default;
            var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"), level),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), ++level),
            AddNodeSearchTreeEntry<StartNode>("Start", ++level, new StartNode()),
            AddNodeSearchTreeEntry<DialogueNode>("Dialogue", level, new DialogueNode()),
            AddNodeSearchTreeEntry<BranchNode>("Branch", level, new BranchNode()),
            AddNodeSearchTreeEntry<EventNode>("Event", level, new EventNode()),
            AddNodeSearchTreeEntry<EndNode>("End", level, new EndNode()),
            AddNodeSearchTreeEntry<ChoiceNode>("Choice", level, new ChoiceNode()),
        };
            return tree;

            SearchTreeEntry AddNodeSearchTreeEntry<T>(string name, int level, T node) where T : BaseNode => new SearchTreeEntry(new GUIContent(name, this._iconImage)) { level = level, userData = node };
        }


        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            // GraphView上のマウスカーソルの座標を取得します。
            Vector2 worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(
                _editorWindow.rootVisualElement.parent,
                context.screenMousePosition - _editorWindow.position.position
            );
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            switch (searchTreeEntry.userData)
            {
                case StartNode node:
                    _graphView.AddElement(_graphView.CreateNode<StartNode>(localMousePosition));
                    return true;
                case DialogueNode node:
                    _graphView.AddElement(_graphView.CreateNode<DialogueNode>(localMousePosition));
                    return true;
                case EventNode node:
                    _graphView.AddElement(_graphView.CreateNode<EventNode>(localMousePosition));
                    return true;
                case BranchNode node:
                    _graphView.AddElement(_graphView.CreateNode<BranchNode>(localMousePosition));
                    return true;
                case EndNode node:
                    _graphView.AddElement(_graphView.CreateNode<EndNode>(localMousePosition));
                    return true;
                case ChoiceNode node:
                    _graphView.AddElement(_graphView.CreateNode<ChoiceNode>(localMousePosition));
                    return true;
                default:
                    return false;
            }
        }
    }
}