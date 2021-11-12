using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dialogue.Editor
{
    public class DialogueDataOperator
    {
        // DialogueGraphView _graphView;
        // List<Edge> _edges => this._graphView.edges.ToList();
        // List<BaseNode> _nodes => this._graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
        // public DialogueDataOperator(DialogueGraphView graphView)
        // {
        //     _graphView = graphView;
        // }

        // /// <summary>
        // /// セーブします。
        // /// </summary>
        // /// <param name="dialogueContainerSO"></param>
        // public void Save(DialogueContainerSO dialogueContainerSO)
        // {
        //     SaveEdges(dialogueContainerSO);
        //     SaveNodes(dialogueContainerSO);

        //     EditorUtility.SetDirty(dialogueContainerSO);
        //     AssetDatabase.SaveAssets();
        // }

        // /// <summary>
        // /// ロードします。
        // /// </summary>
        // /// <param name="dialogueContainerSO"></param>
        // public void Load(DialogueContainerSO dialogueContainerSO)
        // {
        //     ClearGraph();
        //     GenerateNodes(dialogueContainerSO);
        //     ConnectNodes(dialogueContainerSO);
        // }

        // /// <summary>
        // /// エッジをセーブします。
        // /// </summary>
        // /// <param name="dialogueContainerSO"></param>
        // void SaveEdges(DialogueContainerSO dialogueContainerSO)
        // {
        //     dialogueContainerSO.NodeLinkDatas.Clear();
        //     var connectedEdges = this._edges.Where(edge => edge.input.node != null);
        //     foreach (var edge in connectedEdges)
        //     {
        //         var outputNode = edge.output.node as BaseNode;
        //         var inputNode = edge.input.node as BaseNode;

        //         dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
        //         {
        //             BaseNodeGUID = outputNode.GUID,
        //             BasePortName = edge.output.portName,
        //             TargetNodeGUID = inputNode.GUID,
        //             TargetPortName = edge.input.portName,
        //         });
        //     }
        // }

        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="dialogueContainerSO"></param>
        // void SaveNodes(DialogueContainerSO dialogueContainerSO)
        // {
        //     dialogueContainerSO.StartNodeDatas.Clear();
        //     dialogueContainerSO.DialogueNodeDatas.Clear();
        //     dialogueContainerSO.EventNodeDatas.Clear();
        //     dialogueContainerSO.BranchNodeDatas.Clear();
        //     dialogueContainerSO.EndNodeDatas.Clear();
        //     foreach (var node in this._nodes)
        //     {
        //         switch (node)
        //         {
        //             case StartNode startNode:
        //                 dialogueContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
        //                 break;
        //             case DialogueNode dialogueNode:
        //                 dialogueContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
        //                 break;
        //             case EventNode eventNode:
        //                 dialogueContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
        //                 break;
        //             case BranchNode branchNode:
        //                 dialogueContainerSO.BranchNodeDatas.Add(SaveNodeData(branchNode));
        //                 break;
        //             case EndNode endNode:
        //                 dialogueContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        // }

        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // DialogueNodeData SaveNodeData(DialogueNode node)
        // {
        //     var dialogueNodeData = new DialogueNodeData
        //     {
        //         NodeGUID = node.GUID,
        //         Position = node.GetPosition().position,
        //     };

        //     return dialogueNodeData;
        // }

        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // StartNodeData SaveNodeData(StartNode node)
        // {
        //     var nodeData = new StartNodeData()
        //     {
        //         NodeGUID = node.GUID,
        //         Position = node.GetPosition().position
        //     };
        //     return nodeData;
        // }

        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // EndNodeData SaveNodeData(EndNode node)
        // {
        //     var nodeData = new EndNodeData()
        //     {
        //         NodeGUID = node.GUID,
        //         Position = node.GetPosition().position,
        //     };
        //     return nodeData;
        // }

        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // EventNodeData SaveNodeData(EventNode node)
        // {
        //     var nodeData = new EventNodeData()
        //     {
        //         NodeGUID = node.GUID,
        //         Position = node.GetPosition().position,
        //     };

        //     return nodeData;

        // }
        // /// <summary>
        // /// ノードをセーブします。
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // BranchNodeData SaveNodeData(BranchNode node)
        // {
        //     var edges = this._edges.Where(x => x.output.node == node).Cast<Edge>();
        //     var trueOutput = edges.FirstOrDefault(x => x.output.portName == BranchNode.s_TruePortName);
        //     var falseOutput = edges.FirstOrDefault(x => x.output.portName == BranchNode.s_FalsePortName);

        //     var nodeData = new BranchNodeData()
        //     {
        //         NodeGUID = node.GUID,
        //         Position = node.GetPosition().position,
        //     };
        //     return nodeData;
        // }


        // void ClearGraph()
        // {
        //     foreach (var edge in this._edges)
        //     {
        //         this._graphView.RemoveElement(edge);
        //     }
        //     foreach (var node in this._nodes)
        //     {
        //         this._graphView.RemoveElement(node);
        //     }
        // }

        // void GenerateNodes(DialogueContainerSO dialogueContainerSO)
        // {
        //     // Start Node
        //     foreach (var node in dialogueContainerSO.StartNodeDatas)
        //     {
        //         var startNode = this._graphView.CreateNode<StartNode>(node.Position);
        //         startNode.GUID = node.NodeGUID;
        //         this._graphView.AddElement(startNode);
        //     }

        //     // End Node
        //     foreach (var node in dialogueContainerSO.EndNodeDatas)
        //     {
        //         var endNode = this._graphView.CreateNode<EndNode>(node.Position);
        //         endNode.GUID = node.NodeGUID;
        //         endNode.LoadValueIntoField();
        //         this._graphView.AddElement(endNode);
        //     }

        //     // Event Node
        //     foreach (var node in dialogueContainerSO.EventNodeDatas)
        //     {
        //         var eventNode = this._graphView.CreateNode<EventNode>(node.Position);
        //         eventNode.GUID = node.NodeGUID;

        //         eventNode.LoadValueIntoField();
        //         this._graphView.AddElement(eventNode);
        //     }

        //     // BranchNode
        //     foreach (var node in dialogueContainerSO.BranchNodeDatas)
        //     {
        //         var branchNode = this._graphView.CreateNode<BranchNode>(node.Position);
        //         branchNode.GUID = node.NodeGUID;

        //         foreach (var data in node.BranchStringDatas)
        //             branchNode.AddCondition(data);

        //         branchNode.LoadValueIntoField();
        //         branchNode.ReloadLanguage();
        //         this._graphView.AddElement(branchNode);
        //     }

        //     // Dialogue Node
        //     foreach (var node in dialogueContainerSO.DialogueNodeDatas)
        //     {
        //         var dialogueNode = this._graphView.CreateNode<DialogueNode>(node.Position);
        //         dialogueNode.GUID = node.NodeGUID;

        //         dialogueNode.LoadValueIntoField();
        //         this._graphView.AddElement(dialogueNode);
        //     }
        // }

        // void ConnectNodes(DialogueContainerSO dialogueContainerSO)
        // {
        //     // Make connection for all node.
        //     foreach (var node in this._nodes)
        //     {
        //         var connections = dialogueContainerSO.NodeLinkDatas.Where(edge => edge.BaseNodeGUID == node.GUID);
        //         var allOutputPorts = node.outputContainer.Children().Where(x => x is Port).Cast<Port>();
        //         foreach (var connection in connections)
        //         {
        //             var targetNodeGUID = connection.TargetNodeGUID;
        //             var targetNode = this._nodes.First(x => x.GUID == targetNodeGUID);
        //             if (targetNode == null) continue;
        //             foreach (var port in allOutputPorts)
        //                 if (port.portName == connection.BasePortName)
        //                     LinkNodes(port, targetNode.inputContainer[0] as Port);
        //         }
        //     }
        // }

        // void LinkNodes(Port output, Port input)
        // {
        //     var edge = new Edge
        //     {
        //         output = output,
        //         input = input,
        //     };
        //     edge.input.Connect(edge);
        //     edge.output.Connect(edge);
        //     this._graphView.Add(edge);
        // }
    }
}