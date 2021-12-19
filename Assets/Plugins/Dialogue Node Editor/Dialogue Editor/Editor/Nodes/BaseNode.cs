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

        protected void AddToClassLists(VisualElement element, params string[] classLists) => classLists.Where(e => !string.IsNullOrWhiteSpace(e)).ToList().ForEach(e => element.AddToClassList(e));

        /// <summary>
        /// 新規ラベルを生成します。
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Label GetNewLabel(string labelName, params string[] USSClasses)
        {
            var label = new Label(labelName);
            AddToClassLists(label, USSClasses);
            return label;
        }

        /// <summary>
        /// 新規ボタンを生成します。
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Button GetNewButton(string buttonText, params string[] USSClasses)
        {
            var button = new Button() { text = buttonText };
            AddToClassLists(button, USSClasses);
            return button;
        }

        /// <summary>
        /// 新規ボタンを生成します。
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="USS01"></param>
        /// <param name="USS02"></param>
        /// <returns></returns>
        protected Button GetNewButton(string buttonText, System.Action action, params string[] USSClasses)
        {
            var button = GetNewButton(buttonText, USSClasses);
            button.clicked += action;
            AddToClassLists(button, USSClasses);
            return button;
        }

        protected T1 GetNewField<T1, T2>(ContainerValue<T2> inputValue, params string[] USSClasses) where T1 : BaseField<T2>, new()
        {
            var field = GetNewField<T1>(USSClasses);
            field.RegisterValueChangedCallback(value => inputValue.Value = value.newValue);
            field.SetValueWithoutNotify(inputValue.Value);
            return field;
        }

        protected T GetNewField<T>(params string[] USSClasses) where T : VisualElement, new()
        {
            var field = new T();
            AddToClassLists(field, USSClasses);
            return field;
        }

        protected TextField GetNewTextFieldTextsLanguage(List<LanguageGeneric<string>> texts, string placeHolderText = "", params string[] USSClasses)
        {
            foreach (var language in System.Enum.GetValues(typeof(LanguageType)) as LanguageType[])
            {
                texts.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });
            }
            var field = new TextField("");
            this._languageGenericHolderTexts.Add(new LanguageGenericHolderText(texts, field, placeHolderText));
            field.RegisterValueChangedCallback(value => texts.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue);
            field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType);
            field.multiline = true;
            AddToClassLists(field, USSClasses);
            return field;
        }

        protected ObjectField GetNewObjectFieldAudioClipsLanguage(List<LanguageGeneric<AudioClip>> audioClips, params string[] USSClasses)
        {
            foreach (var language in System.Enum.GetValues(typeof(LanguageType)) as LanguageType[])
            {
                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }
            var field = new ObjectField
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType,
            };
            this._languageGenericHolderAudioClips.Add(new LanguageGenericHolderAudioClip(audioClips, field));
            field.RegisterValueChangedCallback(value => audioClips.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip);
            field.SetValueWithoutNotify(audioClips.Find(text => text.LanguageType == this.EditorWindow.SelectedLanguage).LanguageGenericType);
            AddToClassLists(field, USSClasses);
            return field;
        }

        protected ObjectField GetNewObjectFieldSprite(ContainerValue<Sprite> inputSprite, Image imagePreview, params string[] USSClasses)
        {
            ObjectField field = new ObjectField
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = inputSprite.Value
            };
            field.RegisterValueChangedCallback(value =>
            {
                inputSprite.Value = value.newValue as Sprite;
                imagePreview.image = inputSprite.Value != null ? inputSprite.Value.texture : null;
            });
            imagePreview.image = inputSprite.Value != null ? inputSprite.Value.texture : null;
            AddToClassLists(field, USSClasses);
            return field;
        }

        protected ObjectField GetNewObjectField<T>(ContainerValue<T> containerValue, params string[] USSClasses) where T : Object
        {
            ObjectField field = new ObjectField
            {
                objectType = typeof(T),
                allowSceneObjects = false,
                value = containerValue.Value
            };
            field.RegisterValueChangedCallback(value => containerValue.Value = value.newValue as T);
            field.SetValueWithoutNotify(containerValue.Value);
            AddToClassLists(field, USSClasses);
            return field;
        }

        protected EnumField GetNewEnumField<T>(ContainerEnumType<T> enumType, params string[] USSClasses) where T : System.Enum
        {
            var field = new EnumField
            {
                value = enumType.Value
            };
            field.Init(enumType.Value);
            field.RegisterValueChangedCallback(value => enumType.Value = (T)value.newValue);
            field.SetValueWithoutNotify(enumType.Value);
            AddToClassLists(field, USSClasses);
            enumType.EnumField = field;
            return field;
        }

        protected EnumField GetNewEnumField<T>(ContainerEnumType<T> enumType, System.Action action, params string[] USSClasses) where T : System.Enum
        {
            var field = new EnumField
            {
                value = enumType.Value
            };
            field.Init(enumType.Value);
            field.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (T)value.newValue;
                action?.Invoke();
            });
            field.SetValueWithoutNotify(enumType.Value);
            AddToClassLists(field, USSClasses);
            enumType.EnumField = field;
            return field;
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
            textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
            textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

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