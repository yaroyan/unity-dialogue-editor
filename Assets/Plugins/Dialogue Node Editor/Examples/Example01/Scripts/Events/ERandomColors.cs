using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Example01
{
    [CreateAssetMenu(fileName = "Dialogue/New Color Event")]
    [System.Serializable]
    public class ERandomColors : DialogueEventSO
    {
        [SerializeField] int number;
        public override void RunEvent()
        {
            GameEvents.Instance.CallRandomColorModel(number);
        }
    }
}