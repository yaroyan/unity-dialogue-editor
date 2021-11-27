using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Example01
{
    public class DialogueGetData : MonoBehaviour
    {
        [SerializeField] protected DialogueContainerSO dialogueContainerSO;

        // protected BaseNodeData GetNodeByGUID(string targetNodeGUID) =>
        //     dialogueContainerSO.AllNodes.Find(node => node.NodeGUID == targetNodeGUID);

        // protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort) =>
        //     dialogueContainerSO.AllNodes.Find(node => node.NodeGUID == nodePort.InputGUID);

        // protected BaseNodeData GetNextNode(BaseNodeData baseNodeData) =>
        //     this.GetNodeByGUID(dialogueContainerSO.NodeLinkDatas.Find(edge => edge.BaseNodeGUID == baseNodeData.NodeGUID).TargetNodeGUID);
    }
}