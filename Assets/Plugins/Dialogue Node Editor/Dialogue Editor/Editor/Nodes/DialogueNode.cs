using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace Dialogue.Editor
{
    public class DialogueNode : BaseNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DialogueNode() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        /// <returns></returns>
        public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Dialogue";

            AddInputPort("Input", Port.Capacity.Multi);
        }
    }
}