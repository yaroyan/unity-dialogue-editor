using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class EventData : BaseData
    {
        public List<EventData<EventModifierType>> EventDataModifiers = new List<EventData<EventModifierType>>();
        public List<ContainerDialogueEventSO> DialogueEventSOs = new List<ContainerDialogueEventSO>();
    }
}