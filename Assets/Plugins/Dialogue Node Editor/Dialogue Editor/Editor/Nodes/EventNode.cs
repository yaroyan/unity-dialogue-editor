using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using UnityEditor.Experimental.GraphView;

namespace Dialogue.Editor
{
    public class EventNode : BaseNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventNode() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        /// <returns></returns>
        public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Event";

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
        }
    }
}