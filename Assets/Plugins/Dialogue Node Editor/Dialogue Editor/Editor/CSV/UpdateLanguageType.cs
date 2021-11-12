using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Dialogue.Editor
{
    public class UpdateLanguageType
    {
        public void UpdateLanguage()
        {
            // foreach (var dialogueContainer in Helper.FindAllDialogueContainerSO())
            //     foreach (var nodeData in dialogueContainer.DialogueNodeDatas)
            //     {
            //         nodeData.TextLanguages = UpdateLanguageGeneric(nodeData.TextLanguages);
            //         nodeData.AudioClips = UpdateLanguageGeneric(nodeData.AudioClips);
            //         foreach (var nodePort in nodeData.DialogueNodePorts)
            //             nodePort.TextLangueages = UpdateLanguageGeneric(nodePort.TextLangueages);
            //     }
        }

        // List<LanguageGeneric<T>> UpdateLanguageGeneric<T>(IReadOnlyList<LanguageGeneric<T>> languageGenerics)
        // {
        // var list = new List<LanguageGeneric<T>>();

        // foreach (var languageType in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
        //     list.Add(new LanguageGeneric<T> { LanguageType = languageType });

        // foreach (var languageGeneric in languageGenerics)
        // {
        //     var tmp = list.Find(language => language.LanguageType == languageGeneric.LanguageType);
        //     if (tmp is null) continue;
        //     tmp.LanguageGenericType = languageGeneric.LanguageGenericType;
        // }

        //     return list;
        // }
    }
}