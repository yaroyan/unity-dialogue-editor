using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

namespace Dialogue.Editor
{

    public class ChoiceNode : BaseNode
    {
        public ChoiceData ChoiceData { get; set; } = new ChoiceData();
        Box _choiceStateEnumBox;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChoiceNode() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        /// <returns></returns>
        public ChoiceNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Choice";
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/ChoiceNode"));

            var inputPort = AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            inputPort.portColor = Color.yellow;

            TopButton();

            TextLine();
            ChoiceStateEnum();
        }

        void TopButton()
        {
            var menu = new ToolbarMenu { text = "Add Condition" };
            menu.menu.AppendAction("Event Condition", new System.Action<DropdownMenuAction>(x => AddCondition()));
            titleButtonContainer.Add(menu);
        }

        public void AddCondition(EventData<EventConditionType> eventData = null)
        {
            AddConditionEventBuild(ChoiceData.EventDataConditions, eventData);
            SwitchVisibilityChoiceEnum();
        }

        public void TextLine()
        {
            var boxContainer = new Box();
            boxContainer.AddToClassList("TextLineBox");

            var textField = GetNewTextFieldTextsLanguage(ChoiceData.Texts, "Text", "TextBox");
            ChoiceData.TextField = textField;
            boxContainer.Add(textField);

            var objectField = GetNewObjectFieldAudioClipsLanguage(ChoiceData.AudioClips, "AudioClip");
            ChoiceData.ObjectField = objectField;
            boxContainer.Add(objectField);

            ReloadLanguage();

            mainContainer.Add(boxContainer);
        }

        void ChoiceStateEnum()
        {
            _choiceStateEnumBox = new Box();
            _choiceStateEnumBox.AddToClassList("BoxRow");
            SwitchVisibilityChoiceEnum();

            var enumLabel = GetNewLabel("If the condition is not met", "ChoiceLabel");
            var choiceStateEnumField = GetNewEnumField<ChoiceStateType>(ChoiceData.ChoiceStateType, "enumHide");
            _choiceStateEnumBox.Add(choiceStateEnumField);
            _choiceStateEnumBox.Add(enumLabel);
            mainContainer.Add(_choiceStateEnumBox);
        }

        protected override void DeleteBox(Box boxContainer)
        {
            base.DeleteBox(boxContainer);
            SwitchVisibilityChoiceEnum();
        }

        void SwitchVisibilityChoiceEnum()
        {
            SwitchVisibility(ChoiceData.EventDataConditions.Count > 0, _choiceStateEnumBox);
        }

        public override void ReloadLanguage()
        {
            base.ReloadLanguage();
        }

        public override void LoadValueIntoField()
        {
            ChoiceData.ChoiceStateType.EnumField?.SetValueWithoutNotify(ChoiceData.ChoiceStateType.Value);
        }
    }
}
