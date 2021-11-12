using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Dialogue.Example01
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] GameObject _dialogueUI;
        [Header("Text")]
        [SerializeField] Text _textName;
        [SerializeField] Text _textBox;
        [Header("Image")]
        [SerializeField] Image _leftImage;
        [SerializeField] GameObject _leftImageGO;
        [SerializeField] Image _rightImage;
        [SerializeField] GameObject _rightImageGO;
        [Header("Button")]
        [SerializeField] Button button01;
        [SerializeField] Text buttonText01;
        [Space]
        [SerializeField] Button button02;
        [SerializeField] Text buttonText02;
        [Space]
        [SerializeField] Button button03;
        [SerializeField] Text buttonText03;
        [Space]
        [SerializeField] Button button04;
        [SerializeField] Text buttonText04;

        List<Button> _buttons = new List<Button>();
        List<Text> _buttonTexts = new List<Text>();

        void Awake()
        {
            // ShowDialogueUI(false);

            // this._buttons.Add(button01);
            // this._buttons.Add(button02);
            // this._buttons.Add(button03);
            // this._buttons.Add(button04);

            // this._buttonTexts.Add(buttonText01);
            // this._buttonTexts.Add(buttonText02);
            // this._buttonTexts.Add(buttonText03);
            // this._buttonTexts.Add(buttonText04);
        }

        // public void ShowDialogueUI(bool isShow) => this._dialogueUI.SetActive(isShow);

        // public void SetText(string name, string textBox)
        // {
        //     this._textName.text = name;
        //     this._textBox.text = textBox;
        // }

        // public void SetImage(Sprite image, DialogueFaceImageType dialogueFaceImageType)
        // {
        //     this._leftImageGO.SetActive(false);
        //     this._rightImageGO.SetActive(false);
        //     switch (dialogueFaceImageType)
        //     {
        //         case DialogueFaceImageType.Right:
        //             this._leftImage.sprite = image;
        //             this._leftImageGO.SetActive(true);
        //             break;
        //         case DialogueFaceImageType.Left:
        //             this._rightImage.sprite = image;
        //             this._rightImageGO.SetActive(true);
        //             break;
        //         default:
        //             break;
        //     }
        // }

        // public void SetButttons(IReadOnlyList<string> texts, IReadOnlyList<UnityAction> unityActions)
        // {
        //     this._buttons.ForEach(button => button.gameObject.SetActive(false));

        //     for (var i = 0; i < texts.Count; i++)
        //     {
        //         this._buttonTexts[i].text = texts[i];
        //         this._buttons[i].gameObject.SetActive(true);
        //         this._buttons[i].onClick = new Button.ButtonClickedEvent();
        //         this._buttons[i].onClick.AddListener(unityActions[i]);
        //     }
        // }
    }
}