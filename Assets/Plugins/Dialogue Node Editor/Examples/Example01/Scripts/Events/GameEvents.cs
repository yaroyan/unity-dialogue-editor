using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dialogue.Example01
{
    public class GameEvents : MonoBehaviour
    {
        event Action<int> _randomColorModel;
        public static GameEvents Instance { get; private set; }
        public Action<int> RandomColorModel { get => this._randomColorModel; set => this._randomColorModel = value; }

        void Awake()
        {
            Instance ??= this;
            if (Instance is null) Destroy(this.gameObject); else DontDestroyOnLoad(this.gameObject);
        }

        public void CallRandomColorModel(int number) => this._randomColorModel?.Invoke(number);
    }
}