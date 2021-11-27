using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Dialogue
{
    [System.Serializable]
    public class ChoiceData : BaseData
    {
#if UNITY_EDITOR
        public TextField TextField { get; set; }
        public ObjectField ObjectField { get; set; }
#endif
        public ContainerEnumType<ChoiceStateType> ChoiceStateType = new ContainerEnumType<ChoiceStateType>();
        public List<LanguageGeneric<string>> Texts = new List<LanguageGeneric<string>>();
        public List<LanguageGeneric<AudioClip>> AudioClips = new List<LanguageGeneric<AudioClip>>();
        public List<EventData<EventConditionType>> EventDataConditions = new List<EventData<EventConditionType>>();
    }
}
