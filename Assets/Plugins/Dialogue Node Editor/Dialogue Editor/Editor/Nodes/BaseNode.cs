using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

namespace Dialogue.Editor
{
    public abstract class BaseNode : Node
    {
        public string GUID { get; set; }
        protected DialogueGraphView GraphView;
        protected DialogueEditorWindow EditorWindow;
        protected Vector2 DefaultNodeSize = new Vector2(200, 250);
        List<LanguageGenericHolderText> _languageGenericHolderTexts = new List<LanguageGenericHolderText>();
        List<LanguageGenericHolderAudioClip> _languageGenericHolderAudioClips = new List<LanguageGenericHolderAudioClip>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseNode()
        {
            this.GUID = System.Guid.NewGuid().ToString();
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/BaseNode"));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        public BaseNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : this()
        {
            this.EditorWindow = editorWindow;
            this.GraphView = graphView;
            SetPosition(new Rect(position, DefaultNodeSize));
        }

        /// <summary>
        /// 新規ラベルを生成します。
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Label GetNewLabel(string labelName, string USS01 = "", string USS02 = "")
        {
            var label = new Label(labelName);
            label.AddToClassList(USS01);
            label.AddToClassList(USS02);
            return label;
        }

        /// <summary>
        /// 新規ボタンを生成します。
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Button GetNewButton(string buttonText, string USS01 = "", string USS02 = "")
        {
            var button = new Button() { text = buttonText };
            button.AddToClassList(USS01);
            button.AddToClassList(USS02);
            return button;
        }

        /// <summary>
        /// 新規ボタンを生成します。
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Button GetNewButton(string buttonText, System.Action action, string USS01 = "", string USS02 = "")
        {
            var button = GetNewButton(buttonText, USS01, USS02);
            button.clicked += action;
            return button;
        }

        protected T1 GetNewField<T1, T2>(ContainerValue<T2> inputValue, string USS01 = "", string USS02 = "") where T1 : BaseField<T2>, new()
        {
            var field = new T1();
            field.RegisterValueChangedCallback(value => inputValue.Value = value.newValue);
            field.SetValueWithoutNotify(inputValue.Value);
            field.AddToClassList(USS01);
            field.AddToClassList(USS02);
            return field;
        }

        protected T GetNewField<T>(string USS01 = "", string USS02 = "") where T : VisualElement, new()
        {
            var field = new T();
            field.AddToClassList(USS01);
            field.AddToClassList(USS02);
            return field;
        }

        // protected TextField GetNewTextField(ContainerValue<string> inputValue, string placeHolderText, string USS01 = "", string USS02 = "")
        // {
        //     var textField = new TextField();
        //     textField.RegisterValueChangedCallback(value => inputValue.Value = value.newValue);
        //     textField.SetValueWithoutNotify(inputValue.Value);
        //     textField.AddToClassList(USS01);
        //     textField.AddToClassList(USS02);
        //     SetPlaceHolderText(textField, placeHolderText);
        //     return textField;
        // }

        protected TextField GetNewTextField(ContainerValue<string> inputValue, string placeHolderText, string USS01 = "", string USS02 = "")
        {
            var textField = GetNewField<TextField, string>(inputValue, USS01, USS02);
            SetPlaceHolderText(textField, placeHolderText);
            return textField;
        }

        /// <summary>
        /// Get a new TextField that use a List<LanguageGeneric<string>> text.
        /// </summary>
        /// <param name="Text">List of LanguageGeneric<string> Text</param>
        /// <param name="placeholderText">The text that will be displayed if the text field is empty</param>
        /// <param name="USS01">USS class add to the UI element</param>
        /// <param name="USS02">USS class add to the UI element</param>
        /// <returns></returns>
        protected TextField GetNewTextFieldTextLanguage(List<LanguageGeneric<string>> Text, string placeholderText = "", string USS01 = "", string USS02 = "")
        {
            // Add languages
            foreach (LanguageType language in (LanguageType[])System.Enum.GetValues(typeof(LanguageType)))
            {
                Text.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });
            }

            var textField = GetNewField<TextField>(USS01, USS02);

            // Add it to the reaload current language list.
            _languageGenericHolderTexts.Add(new LanguageGenericHolderText(Text, textField, placeholderText));

            // When we change the variable from graph view.
            textField.RegisterValueChangedCallback(value =>
            {
                Text.Find(text => text.LanguageType == EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });
            textField.SetValueWithoutNotify(Text.Find(text => text.LanguageType == EditorWindow.SelectedLanguage).LanguageGenericType);

            // Text field is set to be multiline.
            textField.multiline = true;

            return textField;
        }

        /// <summary>
        /// Get a new ObjectField that use List<LanguageGeneric<AudioClip>>.
        /// </summary>
        /// <param name="audioClips"></param>
        /// <param name="USS01">USS class add to the UI element</param>
        /// <param name="USS02">USS class add to the UI element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectFieldAudioClipsLanguage(List<LanguageGeneric<AudioClip>> audioClips, string USS01 = "", string USS02 = "")
        {
            // Add languages.
            foreach (LanguageType language in (LanguageType[])System.Enum.GetValues(typeof(LanguageType)))
            {
                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }

            // Make ObjectField.
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.LanguageType == EditorWindow.SelectedLanguage).LanguageGenericType,
            };

            // Add it to the reaload current language list.
            _languageGenericHolderAudioClips.Add(new LanguageGenericHolderAudioClip(audioClips, objectField));

            // When we change the variable from graph view.
            objectField.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.LanguageType == EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip;
            });
            objectField.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == EditorWindow.SelectedLanguage).LanguageGenericType);

            // Set uss class for stylesheet.
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);

            return objectField;
        }

        protected IntegerField GetNewIntegerField(ContainerValue<int> inputValue, string USS01 = "", string USS02 = "")
        {
            var integerField = new IntegerField();
            integerField.RegisterValueChangedCallback(value => inputValue.Value = value.newValue);
            integerField.SetValueWithoutNotify(inputValue.Value);
            integerField.AddToClassList(USS01);
            integerField.AddToClassList(USS02);
            return integerField;
        }

        protected FloatField GetNewFloatField(ContainerValue<float> inputValue, string USS01 = "", string USS02 = "")
        {
            var floatFiled = new FloatField();
            floatFiled.RegisterValueChangedCallback(value => inputValue.Value = value.newValue);
            floatFiled.SetValueWithoutNotify(inputValue.Value);
            floatFiled.AddToClassList(USS01);
            floatFiled.AddToClassList(USS02);
            return floatFiled;
        }

        protected Image GetNewImage(string USS01 = "", string USS02 = "")
        {
            var image = new Image();
            image.AddToClassList(USS01);
            image.AddToClassList(USS02);
            return image;
        }

        protected ObjectField GetNewObjectFieldSprite(ContainerValue<Sprite> inputSprite, Image imagePreview, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new ObjectField
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = inputSprite.Value
            };
            objectField.RegisterValueChangedCallback(value =>
            {
                inputSprite.Value = value.newValue as Sprite;
                imagePreview.image = inputSprite.Value != null ? inputSprite.Value.texture : null;
            });
            imagePreview.image = inputSprite.Value != null ? inputSprite.Value.texture : null;
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            return objectField;
        }

        protected ObjectField GetNewObjectField<T>(ContainerValue<T> containerValue, string USS01 = "", string USS02 = "") where T : Object
        {
            ObjectField objectField = new ObjectField
            {
                objectType = typeof(T),
                allowSceneObjects = false,
                value = containerValue.Value
            };
            objectField.RegisterValueChangedCallback(value => containerValue.Value = value.newValue as T);
            objectField.SetValueWithoutNotify(containerValue.Value);
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            return objectField;
        }

        protected ObjectField GetNewObjectFieldDialogueEvent(ContainerValue<DialogueEventSO> inputDialogueEventSO, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new ObjectField
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = inputDialogueEventSO.Value
            };
            objectField.RegisterValueChangedCallback(value => inputDialogueEventSO.Value = value.newValue as DialogueEventSO);
            objectField.SetValueWithoutNotify(inputDialogueEventSO.Value);
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            return objectField;
        }

        protected EnumField GetNewEnumField<T>(ContainerEnumType<T> enumType, string USS01 = "", string USS02 = "") where T : System.Enum
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value => enumType.Value = (T)value.newValue);
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected EnumField GetNewEnumField<T>(ContainerEnumType<T> enumType, System.Action action, string USS01 = "", string USS02 = "") where T : System.Enum
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (T)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected EnumField GetNewEnumFieldChoiceStateType(ContainerEnumType<ChoiceStateType> enumType, string USS01 = "", string USS02 = "")
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value => enumType.Value = (ChoiceStateType)value.newValue);
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected EnumField GetNewEnumFieldEndNodeType(ContainerEnumType<EndNodeType> enumType, string USS01 = "", string USS02 = "")
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value => enumType.Value = (EndNodeType)value.newValue);
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected EnumField GetNewEnumFieldEventModifierType(ContainerEnumType<EventModifierType> enumType, System.Action action, string USS01 = "", string USS02 = "")
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (EventModifierType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected EnumField GetNewEnumFieldEventConditionType(ContainerEnumType<EventConditionType> enumType, System.Action action, string USS01 = "", string USS02 = "")
        {
            var enumField = new EnumField
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);
            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (EventConditionType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.Value);
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);
            enumType.EnumField = enumField;
            return enumField;
        }

        protected TextField GenNewTextFieldTextsLanguage(List<LanguageGeneric<string>> texts, string placeHolderText = "", string USS01 = "", string USS02 = "")
        {
            foreach (var language in System.Enum.GetValues(typeof(LanguageType)) as LanguageType[])
            {
                texts.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });
            }
            TextField textField = new TextField("");
            this._languageGenericHolderTexts.Add(new LanguageGenericHolderText(texts, textField, placeHolderText));
            textField.RegisterValueChangedCallback(value => texts.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue);
            textField.SetValueWithoutNotify(texts.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType);
            textField.multiline = true;
            textField.AddToClassList(USS01);
            textField.AddToClassList(USS02);
            return textField;
        }

        protected ObjectField GenNewTextFieldAudioClipsLanguage(List<LanguageGeneric<AudioClip>> audioClips, string USS01 = "", string USS02 = "")
        {
            foreach (var language in System.Enum.GetValues(typeof(LanguageType)) as LanguageType[])
            {
                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }
            var objectField = new ObjectField
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType,
            };
            this._languageGenericHolderAudioClips.Add(new LanguageGenericHolderAudioClip(audioClips, objectField));
            objectField.RegisterValueChangedCallback(value => audioClips.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip);
            objectField.SetValueWithoutNotify(audioClips.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType);
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            return objectField;
        }

        protected void AddModifierEventBuild(List<EventData<EventModifierType>> eventDatas, EventData<EventModifierType> eventData = null)
        {
            var eventModifier = new EventData<EventModifierType>();
            if (eventData != null)
            {
                eventModifier.EventText.Value = eventData.EventText.Value;
                eventModifier.Number.Value = eventData.Number.Value;
                eventModifier.EnumType.Value = eventData.EnumType.Value;
            }
            eventDatas.Add(eventModifier);

            var boxContainer = new Box();
            var boxFloatField = new Box();
            boxContainer.AddToClassList("EventBox");
            boxFloatField.AddToClassList("EventBoxFloatField");

            var textField = GetNewField<TextField, string>(eventModifier.EventText, "Event", "EventText");
            var floatField = GetNewField<FloatField, float>(eventModifier.Number, "EventFloat");

            System.Action action = () => SwitchVisibilityEventModifierType(eventModifier.EnumType.Value, boxFloatField);
            EnumField enumField = GetNewEnumField<EventModifierType>(eventModifier.EnumType, action, "EventEnum");
            SwitchVisibilityEventModifierType(eventModifier.EnumType.Value, boxFloatField);

            var button = GetNewButton("X", () =>
            {
                eventDatas.Remove(eventModifier);
                DeleteBox(boxContainer);
            },
            "RemoveButton");

            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxFloatField.Add(floatField);
            boxContainer.Add(boxFloatField);
            boxContainer.Add(button);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        protected void AddConditionEventBuild(List<EventData<EventConditionType>> eventDatas, EventData<EventConditionType> eventData = null)
        {
            var eventCondition = new EventData<EventConditionType>();
            if (eventData != null)
            {
                eventCondition.EventText.Value = eventData.EventText.Value;
                eventCondition.Number.Value = eventData.Number.Value;
                eventCondition.EnumType.Value = eventData.EnumType.Value;
            }
            eventDatas.Add(eventCondition);

            var boxContainer = new Box();
            var boxFloatField = new Box();
            boxContainer.AddToClassList("EventBox");
            boxFloatField.AddToClassList("EventBoxFloatField");

            var textField = GetNewField<TextField, string>(eventCondition.EventText, "Event", "EventText");
            var floatField = GetNewField<FloatField, float>(eventCondition.Number, "EventFloat");

            System.Action action = () => SwitchVisibilityEventConditionType(eventCondition.EnumType.Value, boxFloatField);
            EnumField enumField = GetNewEnumField<EventConditionType>(eventCondition.EnumType, action, "EventEnum");
            SwitchVisibilityEventConditionType(eventCondition.EnumType.Value, boxFloatField);

            var button = GetNewButton("X", () =>
            {
                eventDatas.Remove(eventCondition);
                DeleteBox(boxContainer);
            },
            "RemoveButton");

            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxFloatField.Add(floatField);
            boxContainer.Add(boxFloatField);
            boxContainer.Add(button);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        void SwitchVisibilityEvent<T>(T value, HashSet<T> invisibles, Box boxContainer) where T : System.Enum
        {
            bool isVisible = invisibles.Contains(value);
            SwitchVisibility(!isVisible, boxContainer);
        }

        void SwitchVisibilityEventConditionType(EventConditionType value, Box boxContainer)
        {
            bool isVisible = value == EventConditionType.True
            || value == EventConditionType.False;
            SwitchVisibility(!isVisible, boxContainer);
        }

        void SwitchVisibilityEventModifierType(EventModifierType value, Box boxContainer)
        {
            bool isVisible = value == EventModifierType.SetTrue || value == EventModifierType.SetFalse;
            SwitchVisibility(!isVisible, boxContainer);
        }

        protected void SwitchVisibility(bool isVisible, Box boxContainer)
        {
            string invisible = "invisible";
            if (isVisible)
                boxContainer.RemoveFromClassList(invisible);
            else
                boxContainer.AddToClassList(invisible);
        }

        protected virtual void DeleteBox(Box boxContainer)
        {
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// 出力ポートを追加します。
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public Port AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
        {
            var outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
            return outputPort;
        }

        /// <summary>
        /// 入力ポートを追加します。
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="capacity"></param>
        /// <returns><returns>
        public Port AddInputPort(string portName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            var inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
            return inputPort;
        }

        /// <summary>
        /// ポートのインスタンスを取得します。
        /// </summary>
        /// <param name="nodeDirection"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        /// <summary>
        /// フィールドにデータをロードします。
        /// </summary>
        public virtual void LoadValueIntoField() { }

        /// <summary>
        /// 言語を再読み込みします。
        /// </summary>
        public virtual void ReloadLanguage()
        {
            foreach (var textHolder in this._languageGenericHolderTexts)
                ReloadTextLangueage(textHolder.InputTexts, textHolder.TextField, textHolder.PlaceHolderText);
            foreach (var audioHolder in this._languageGenericHolderAudioClips)
                ReloadAudioClipLanguage(audioHolder.InputAudioClips, audioHolder.ObjectField);
        }

        protected void ReloadTextLangueage(List<LanguageGeneric<string>> inputTexts, TextField textField, string placeHolderText)
        {
            var languageType = inputTexts.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType;
            textField.RegisterValueChangedCallback(value => { languageType = value.newValue; });
            textField.SetValueWithoutNotify(languageType);
            SetPlaceHolderText(textField, placeHolderText);
        }

        protected void SetPlaceHolderText(TextField textField, string placeHolderText)
        {
            var placeHolderClass = TextField.ussClassName + "__placeholder";

            CheckText();
            onFocusOut();
            textField.RegisterCallback<FocusEvent>(evt => onFocusIn());
            textField.RegisterCallback<FocusEvent>(evt => onFocusOut());

            void onFocusIn()
            {
                if (!textField.ClassListContains(placeHolderClass)) return;
                textField.value = string.Empty;
                textField.RemoveFromClassList(placeHolderClass);
            }

            void onFocusOut()
            {
                if (!string.IsNullOrEmpty(textField.text)) return;
                textField.SetValueWithoutNotify(placeHolderText);
                textField.AddToClassList(placeHolderClass);
            }

            void CheckText()
            {
                if (string.IsNullOrEmpty(textField.text)) return;
                textField.RemoveFromClassList(placeHolderClass);
            }
        }

        protected void ReloadAudioClipLanguage(List<LanguageGeneric<AudioClip>> inputAudioClips, ObjectField objectField)
        {
            var languageType = inputAudioClips.Find(audioClip => audioClip.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType;
            objectField.RegisterValueChangedCallback(value => { languageType = value.newValue as AudioClip; });
            objectField.SetValueWithoutNotify(languageType);
        }
    }

    class LanguageGenericHolderText
    {
        public List<LanguageGeneric<string>> InputTexts;
        public TextField TextField;
        public string PlaceHolderText;
        public LanguageGenericHolderText(List<LanguageGeneric<string>> inputTexts, TextField textField, string placeHolderText = "placeHolderText")
        {
            this.InputTexts = inputTexts;
            this.TextField = textField;
            this.PlaceHolderText = placeHolderText;
        }
    }

    class LanguageGenericHolderAudioClip
    {
        public List<LanguageGeneric<AudioClip>> InputAudioClips;
        public ObjectField ObjectField;
        public LanguageGenericHolderAudioClip(List<LanguageGeneric<AudioClip>> inputAudioClips, ObjectField objectField)
        {
            this.InputAudioClips = inputAudioClips;
            this.ObjectField = objectField;
        }
    }
}