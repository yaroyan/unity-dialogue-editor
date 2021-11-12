using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Example01
{
    public class RandomColors : MonoBehaviour
    {
        [SerializeField] int _myNumber;
        List<Material> _materials = new List<Material>();

        void Awake()
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var smr in skinnedMeshRenderers)
            {
                foreach (var material in smr.materials)
                {
                    this._materials.Add(material);
                }
            }
        }

        void Start()
        {
            GameEvents.Instance.RandomColorModel += DoRandomColorModel;
        }

        void OnDestroy()
        {
            GameEvents.Instance.RandomColorModel -= DoRandomColorModel;
        }

        void DoRandomColorModel(int number)
        {
            if (this._myNumber == number)
            {
                foreach (var material in this._materials)
                {
                    material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                }
            }
        }
    }
}