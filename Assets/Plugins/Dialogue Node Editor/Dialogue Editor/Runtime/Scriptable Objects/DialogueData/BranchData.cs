using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class BranchData : BaseData
    {
        public string TrueGUIDNode;
        public string FalseGUIDNode;
        public List<EventData<EventConditionType>> EventDataConditions = new List<EventData<EventConditionType>>();
    }
}
