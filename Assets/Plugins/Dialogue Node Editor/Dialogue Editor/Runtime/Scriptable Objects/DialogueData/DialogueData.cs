using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueData : BaseData
    {
        public List<BaseDialogueDataContainer> BaseDialogueDataContainers { get; set; } = new List<BaseDialogueDataContainer>();
        public List<DialogueDataName> DialogueDataNames = new List<DialogueDataName>();
        public List<DialogueDataText> DialogueDataTexts = new List<DialogueDataText>();
        public List<DialogueDataImage> DialogueDataImages = new List<DialogueDataImage>();
        public List<DialogueDataPort> DialogueDataPorts = new List<DialogueDataPort>();
    }

    [System.Serializable]
    public class BaseDialogueDataContainer
    {
        /// <summary>
        /// 並び順
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        public ContainerValue<int> Order = new ContainerValue<int>();
    }

    [System.Serializable]
    public class DialogueDataValue<T> : BaseDialogueDataContainer
    {
        public ContainerValue<T> Value = new ContainerValue<T>();
    }

    [System.Serializable]
    public class DialogueDataImage : BaseDialogueDataContainer
    {
        public ContainerValue<Sprite> SpriteLeft = new ContainerValue<Sprite>();
        public ContainerValue<Sprite> SpriteRight = new ContainerValue<Sprite>();
    }

    [System.Serializable]
    public class DialogueDataText : BaseDialogueDataContainer
    {
#if UNITY_EDITOR
        public TextField TextField { get; set; }
        public ObjectField ObjectField { get; set; }
#endif
        public ContainerValue<string> GUID = new ContainerValue<string>();
        public List<LanguageGeneric<string>> Texts = new List<LanguageGeneric<string>>();
        public List<LanguageGeneric<AudioClip>> AudioClips = new List<LanguageGeneric<AudioClip>>();
    }

    [System.Serializable]
    public class DialogueDataName : BaseDialogueDataContainer
    {
        public ContainerValue<string> CharacterName = new ContainerValue<string>();
    }

    [System.Serializable]
    public class DialogueDataPort
    {
        public string PortGUID;
        public string InputGUID;
        public string OutputGUID;
    }
}
