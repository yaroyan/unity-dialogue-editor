using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dialogue.Editor
{
    public class BranchNode : BaseNode
    {
        // public List<BranchStringIdData> BranchStringDatas { get; set; } = new List<BranchStringIdData>();
        public static readonly string s_TruePortName = "True";
        public static readonly string s_FalsePortName = "False";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BranchNode()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        /// <returns></returns>
        public BranchNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort(s_TruePortName, Port.Capacity.Single);
            AddOutputPort(s_FalsePortName, Port.Capacity.Single);
            // TopButton();
        }

        // /// <summary>
        // /// トップボタンを生成します。
        // /// </summary>
        // public void TopButton()
        // {
        //     var menu = new ToolbarMenu();
        //     menu.text = "Add Condition";

        //     menu.menu.AppendAction("String Condition", new System.Action<DropdownMenuAction>(x => AddCondition()));
        //     titleButtonContainer.Add(menu);
        // }

        // /// <summary>
        // /// 条件を追加します。
        // /// </summary>
        // /// <param name="stringIdData"></param>
        // public void AddCondition(BranchStringIdData stringIdData = null)
        // {

        // }
    }
}
