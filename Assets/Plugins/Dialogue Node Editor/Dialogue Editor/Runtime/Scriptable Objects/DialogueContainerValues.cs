using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
namespace Dialogue
{
    public class DialogueContainerValues { }

    [System.Serializable]
    public class ContainerDialogueEventSO
    {
        public DialogueEventSO DialogueEventSO;
    }

    [System.Serializable]
    public class LanguageGeneric<T>
    {
        public LanguageType LanguageType;
        public T LanguageGenericType;
    }

    [System.Serializable]
    public class ContainerValue<T>
    {
        public T Value;
    }

    [System.Serializable]
    public class ContainerString
    {
        public string Value;
    }

    [System.Serializable]
    public class ContainerInt
    {
        public int Value;
    }

    [System.Serializable]
    public class ContainerFloat
    {
        public float Value;
    }

    [System.Serializable]
    public class ContainerSprite
    {
        public Sprite Value;
    }

    [System.Serializable]
    public class ContainerEnumType<T> where T : System.Enum
    {
        public ContainerEnumType(int value = 1)
        {
            this.value = (T)System.Enum.ToObject(typeof(T), value);
        }

#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public T value;
    }

    [System.Serializable]
    public class ContainerChoiceStateType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public ChoiceStateType value = ChoiceStateType.Hidden;
    }

    [System.Serializable]
    public class ContainerEndNodeType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public EndNodeType value = EndNodeType.End;
    }

    [System.Serializable]
    public class ContainerEventConditionType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public EventConditionType value = EventConditionType.True;
    }

    [System.Serializable]
    public class ContainerEventModifierType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public EventModifierType value = EventModifierType.SetTrue;
    }

    [System.Serializable]
    public class BaseEventData
    {
        public ContainerValue<string> EventText = new ContainerValue<string>();
        public ContainerValue<float> Number = new ContainerValue<float>();
    }

    [System.Serializable]
    public class EventData<T> where T : System.Enum
    {
        public ContainerValue<string> EventText = new ContainerValue<string>();
        public ContainerValue<float> Number = new ContainerValue<float>();
        public ContainerEnumType<T> EnumType = new ContainerEnumType<T>();
    }

    [System.Serializable]
    public class EventDataModifier : BaseEventData
    {
        // public ContainerValue<string> EventText = new ContainerValue<string>();
        // public ContainerValue<float> Number = new ContainerValue<float>();
        public ContainerEnumType<EventModifierType> EnumType = new ContainerEnumType<EventModifierType>();
    }

    [System.Serializable]
    public class EventDataCondition : BaseEventData
    {
        // public ContainerValue<string> EventText = new ContainerValue<string>();
        // public ContainerValue<float> Number = new ContainerValue<float>();
        public ContainerEnumType<EventConditionType> EnumType = new ContainerEnumType<EventConditionType>();

    }
}