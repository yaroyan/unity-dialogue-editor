using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogue.Editor
{
    public class EndNode : BaseNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EndNode() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "End";

            AddInputPort("Input", Port.Capacity.Multi);
        }
    }
}