using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// using UnityEditor.Experimental.GraphView;

namespace Dialogue
{
    /// <summary>
    /// ダイアログコンテナ
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
    [System.Serializable]
    public class DialogueContainerSO : ScriptableObject
    {
        public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();
        public List<EndData> EndDatas = new List<EndData>();
        public List<StartData> StartDatas = new List<StartData>();
        public List<EventData> EventDatas = new List<EventData>();
        public List<BranchData> BranchDatas = new List<BranchData>();
        public List<DialogueData> DialogueDatas = new List<DialogueData>();
        public List<ChoiceData> ChoiceDatas = new List<ChoiceData>();
        public List<BaseData> AllDatas
        {
            get
            {
                var data = new List<BaseData>();
                data.AddRange(this.EndDatas);
                data.AddRange(this.StartDatas);
                data.AddRange(this.EventDatas);
                data.AddRange(this.BranchDatas);
                data.AddRange(this.DialogueDatas);
                data.AddRange(this.ChoiceDatas);
                return data;
            }
        }
    }

    [System.Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGUID;
        public string BasePortName;
        public string TargetNodeGUID;
        public string TargetPortName;
    }
}