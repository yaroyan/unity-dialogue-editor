using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public abstract class DialogueEventSO : ScriptableObject
    {
        public virtual void RunEvent()
        {
            Debug.Log("Event was Called");
        }
    }
}