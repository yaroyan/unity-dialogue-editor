using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

namespace Dialogue.Editor
{
    public abstract class BaseNode : Node
    {
        /// <summary>
        /// 一意識別子
        /// </summary>
        /// <value></value>
        public string GUID { get; set; }
        protected DialogueGraphView GraphView;
        protected DialogueEditorWindow EditorWindow;
        /// <summary>
        /// サイズ
        /// </summary>
        /// <returns></returns>
        protected Vector2 DefaultNodeSize = new Vector2(200, 250);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseNode()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("USS/Dialogue/Node"));
            this.GUID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        public BaseNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : this()
        {
            this.EditorWindow = editorWindow;
            this.GraphView = graphView;
            SetPosition(new Rect(position, DefaultNodeSize));
        }

        /// <summary>
        /// 出力ポートを追加します。
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="capacity"></param>
        public void AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
        {
            var outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
        }

        /// <summary>
        /// 入力ポートを追加します。
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="capacity"></param>
        public void AddInputPort(string portName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            var inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
        }

        /// <summary>
        /// ポートのインスタンスを取得します。
        /// </summary>
        /// <param name="nodeDirection"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity)
        {
            return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        /// <summary>
        /// フィールドにデータをロードします。
        /// </summary>
        public virtual void LoadValueIntoField() { }

        /// <summary>
        /// 言語を再読み込みします。
        /// </summary>
        public virtual void ReloadLanguage()
        {

        }
    }
}