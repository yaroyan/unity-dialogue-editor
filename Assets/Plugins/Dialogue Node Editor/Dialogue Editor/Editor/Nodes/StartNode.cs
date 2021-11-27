using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogue.Editor
{
    public class StartNode : BaseNode
    {
        public StartNode() { }
        public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Start";
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/StartNode"));

            AddOutputPort("Output", Port.Capacity.Single);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}