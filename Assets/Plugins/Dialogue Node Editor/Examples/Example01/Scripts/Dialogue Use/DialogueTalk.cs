using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue.Example01
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] DialogueController _dialogueController;
        [SerializeField] AudioSource _audioSource;

        // DialogueNodeData _currentDialogueNodeData;
        // DialogueNodeData _lastDialogueNodedata;

        void Awake()
        {
            _dialogueController = FindObjectOfType<DialogueController>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void StartDialogue()
        {
            // CheckNodeType(GetNextNode(base.dialogueContainerSO.StartNodeDatas[0]));
            // _dialogueController.ShowDialogueUI(true);
        }

        // public void CheckNodeType(BaseNodeData baseNodeData)
        // {
        // switch (baseNodeData)
        // {
        //     case StartNodeData nodeData: RunNode(nodeData); break;
        //     case DialogueNodeData nodeData: RunNode(nodeData); break;
        //     case EventNodeData nodeData: RunNode(nodeData); break;
        //     case EndNodeData nodeData: RunNode(nodeData); break;
        //     default: break;
        // }
        // }

        // void RunNode(StartNodeData nodeData)
        // {
        //     CheckNodeType(GetNextNode(base.dialogueContainerSO.StartNodeDatas[0]));
        // }

        // void RunNode(DialogueNodeData nodeData)
        // {
        //     if (this._currentDialogueNodeData != nodeData)
        //     {
        //         this._lastDialogueNodedata = _currentDialogueNodeData;
        //         this._currentDialogueNodeData = nodeData;
        //     }

        //     this._dialogueController.SetText(nodeData.CharacterName, nodeData.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
        //     this._dialogueController.SetImage(nodeData.Sprite, nodeData.DialogueFaceImageType);

        //     MakeButtons(nodeData.DialogueNodePorts);

        //     this._audioSource.clip = nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
        //     this._audioSource.Play();
        // }

        // void RunNode(EventNodeData nodeData)
        // {
        //     foreach (var data in nodeData.EventScriptableObjectDatas)
        //         data.DialogueEventSO?.RunEvent();
        //     CheckNodeType(GetNextNode(nodeData));
        // }

        // void RunNode(EndNodeData nodeData)
        // {
        //     switch (nodeData.EndNodeType)
        //     {
        //         case EndNodeType.End:
        //             this._dialogueController.ShowDialogueUI(false);
        //             break;
        //         case EndNodeType.Repeat:
        //             CheckNodeType(GetNodeByGUID(this._currentDialogueNodeData.NodeGUID));
        //             break;
        //         case EndNodeType.GoBack:
        //             CheckNodeType(GetNodeByGUID(this._lastDialogueNodedata.NodeGUID));
        //             break;
        //         case EndNodeType.ReturnToStart:
        //             CheckNodeType(GetNextNode(dialogueContainerSO.StartNodeDatas[0]));
        //             break;
        //         default:
        //             break;
        //     }
        // }

        // void MakeButtons(IReadOnlyList<DialogueNodePort> nodePorts)
        // {
        //     var texts = new List<string>();
        //     var unityActions = new List<UnityAction>();
        //     foreach (var nodePort in nodePorts)
        //     {
        //         texts.Add(nodePort.TextLangueages.Find(text =>
        //             text.LanguageType == LanguageController.Instance.Language).LanguageGenericType
        //         );
        //         unityActions.Add(new UnityAction(() =>
        //         {
        //             CheckNodeType(GetNodeByGUID(nodePort.InputGUID));
        //             _audioSource.Stop();
        //         }));
        //     }
        //     _dialogueController.SetButttons(texts, unityActions);
        // }
    }
}