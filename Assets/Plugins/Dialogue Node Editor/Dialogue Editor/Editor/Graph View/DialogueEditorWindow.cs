using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

namespace Dialogue.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        // Current open dialogue container in dialogue editor  window
        DialogueContainerSO _currentDialogueContainer;
        DialogueGraphView _graphView;
        DialogueDataOperator _dataOperator;

        // Current selected language in the dialogue editor window
        public LanguageType SelectedLanguage { get; set; } = LanguageType.Japanese;
        // Languages toolbar menu in the top  of dialogue  editor window
        ToolbarMenu _languageDropdownMenu;
        // Name of current open dialogue  container
        Label _nameOfDialogueContainer;
        // Name of the graph view style sheet
        static readonly string s_graphViewStyleSheet = @"USS/Dialogue/EditorWindow/EditorWindow";

        /// <summary>
        /// エディタのツールバーにメニューアイテムを追加します。
        /// </summary>
        [MenuItem("Tools/DialogueWindow")]
        static DialogueEditorWindow ShowWindow()
        {
            var window = GetWindow<DialogueEditorWindow>();
            window.titleContent = new GUIContent("DialogueWindow");
            window.minSize = new Vector2(500, 250);
            window.Show();
            return window;
        }

        /// <summary>
        /// アセットのダブルクリック時にエディタを表示します。
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceID, int line)
        {
            DialogueContainerSO dialogueContainerSO = EditorUtility.InstanceIDToObject(instanceID) as DialogueContainerSO;

            if (dialogueContainerSO is null) return false;

            var window = ShowWindow();
            window._currentDialogueContainer = dialogueContainerSO;
            window.Load();
            return true;
        }

        void OnGUI()
        {

        }

        private void OnEnable()
        {
            InitializeGraphView();
            GenerateToolbar();
            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        /// <summary>
        /// GraphViewを構成します。
        /// </summary>
        void InitializeGraphView()
        {
            this._graphView = new DialogueGraphView(editorWindow: this);
            this._graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
            this._dataOperator = new DialogueDataOperator(this._graphView);

        }

        /// <summary>
        /// ツールバーを生成します。
        /// </summary>
        void GenerateToolbar()
        {
            // スタイルシートの適用
            rootVisualElement.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(s_graphViewStyleSheet));

            // ツールバーの生成
            Toolbar toolbar = new Toolbar();

            // セーブボタンの追加
            toolbar.Add(child: new Button(clickEvent: () => Save()) { text = "Save" });

            // ロードボタンの追加
            toolbar.Add(child: new Button(clickEvent: () => Load()) { text = "Load" });

            // ドロップダウンメニューの追加
            this._languageDropdownMenu = new ToolbarMenu();
            // ()によるキャストの例外処理に要するコストをカットするためasを用いる。
            foreach (var language in Enum.GetValues(typeof(LanguageType)) as LanguageType[])
            {
                this._languageDropdownMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language)));
            }
            toolbar.Add(this._languageDropdownMenu);

            // ラベルの追加
            this._nameOfDialogueContainer = new Label();
            this._nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");
            toolbar.Add(_nameOfDialogueContainer);

            // MiniMapの追加
            toolbar.Add(child: new Button(clickEvent: () => ToggleVisibilityForMiniMap()) { text = "MiniMap" });

            // ツールバーの追加
            rootVisualElement.Add(toolbar);
        }

        /// <summary>
        /// ミニマップを生成します。
        /// </summary>
        void GenerateMiniMap()
        {
            var miniMap = new MiniMap();
            // var coords = this._graphView.contentViewContainer.WorldToLocal(new Vector2(x: this.maxSize.x - 10.0f, y: 30.0f));
            // miniMap.SetPosition(newPos: new Rect(x: coords.x, y: coords.y, width: 200.0f, height: 140.0f));
            // 代替配置
            miniMap.SetPosition(newPos: new Rect(x: 10.0f, y: 30.0f, width: 200.0f, height: 140.0f));
            miniMap.name = "MiniMap";
            this._graphView.Add(miniMap);
        }

        /// <summary>
        /// MiniMapの表示・非表示を切り替えます。
        /// </summary>
        void ToggleVisibilityForMiniMap()
        {
            var miniMap = this._graphView.Q<MiniMap>("MiniMap");
            if (miniMap is null) GenerateMiniMap(); else this._graphView.Remove(miniMap);
        }

        /// <summary>
        /// セーブします。
        /// </summary>
        void Save()
        {
            if (this._currentDialogueContainer is null) return;
            this._dataOperator.Save(this._currentDialogueContainer);
        }

        /// <summary>
        /// ロードします。
        /// </summary>
        void Load()
        {
            if (this._currentDialogueContainer is null) return;
            Language(LanguageType.Japanese);
            this._nameOfDialogueContainer.text = "Name: " + _currentDialogueContainer.name;
            this._dataOperator.Load(this._currentDialogueContainer);
        }

        /// <summary>
        /// 言語設定を変更します。
        /// </summary>
        /// <param name="language"></param>
        void Language(LanguageType language)
        {
            this._languageDropdownMenu.text = "Language: " + language.ToString();
            SelectedLanguage = language;
            _graphView.ReloadLanguage();
        }
    }
}