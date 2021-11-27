using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Dialogue.Editor
{
    public class EventNode : BaseNode
    {
        public EventData EventData { get; set; } = new EventData();

        public EventNode() { }
        public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Event";
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/EventNode"));

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            AddTopButton();
        }

        public void AddTopButton()
        {
            var menu = new ToolbarMenu { text = "Add Event" };
            menu.menu.AppendAction("Event Modifier", new System.Action<DropdownMenuAction>(x => AddEvent()));
            menu.menu.AppendAction("Scriptable Object", new System.Action<DropdownMenuAction>(x => AddScriptableEvent()));
            titleContainer.Add(menu);
        }

        public void AddEvent(EventData<EventModifierType> eventData = null)
        {
            AddModifierEventBuild(EventData.EventDataModifiers, eventData);
        }

        public void AddScriptableEvent(ContainerValue<DialogueEventSO> dialogueEventSO = null)
        {
            var tmpDialogueEventSO = new ContainerValue<DialogueEventSO>();
            if (dialogueEventSO != null) tmpDialogueEventSO.Value = dialogueEventSO.Value;
            this.EventData.DialogueEventSOs.Add(tmpDialogueEventSO);

            var boxContainer = new Box();
            boxContainer.AddToClassList("EventBox");

            var objectField = GetNewObjectField<DialogueEventSO>(tmpDialogueEventSO, "EventObject");

            var button = GetNewButton("X", () =>
            {
                DeleteBox(boxContainer);
                EventData.DialogueEventSOs.Remove(tmpDialogueEventSO);
            },
            "RemoveButton");

            boxContainer.Add(objectField);
            boxContainer.Add(button);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }
    }
}