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
        public EndData EndData { get; set; } = new EndData();
        public EndNode() { }
        public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "End";
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/EndNode"));

            AddInputPort("Input", Port.Capacity.Multi);

            MakeMainContainer();
        }

        void MakeMainContainer()
        {
            mainContainer.Add(GetNewEnumField<EndNodeType>(EndData.EnumType));
        }

        public override void LoadValueIntoField()
        {
            EndData.EnumType.EnumField?.SetValueWithoutNotify(EndData.EnumType.Value);
        }
    }
}