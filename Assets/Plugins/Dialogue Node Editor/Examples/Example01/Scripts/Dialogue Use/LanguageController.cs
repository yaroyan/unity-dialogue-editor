using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Example01
{
    public class LanguageController : MonoBehaviour
    {
        [field: SerializeField]
        public LanguageType Language { get; set; }
        public static LanguageController Instance { get; private set; }

        void Awake()
        {
            Instance ??= this;
            if (Instance == null) DontDestroyOnLoad(this.gameObject); else Destroy(this.gameObject);
        }
    }
}