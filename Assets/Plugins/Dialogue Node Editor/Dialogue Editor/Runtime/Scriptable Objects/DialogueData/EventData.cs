using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class EventData : BaseData
    {
        public List<EventData<EventModifierType>> EventDataModifiers = new List<EventData<EventModifierType>>();
        public List<ContainerValue<DialogueEventSO>> DialogueEventSOs = new List<ContainerValue<DialogueEventSO>>();
    }
}