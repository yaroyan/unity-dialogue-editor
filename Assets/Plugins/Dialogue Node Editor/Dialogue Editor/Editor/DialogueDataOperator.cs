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
        List<Edge> edges => graphView.edges.ToList();
        List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

        DialogueGraphView graphView;

        public DialogueDataOperator(DialogueGraphView graphView)
        {
            this.graphView = graphView;
        }

        public void Save(DialogueContainerSO dialogueContainerSO)
        {
            SaveEdges(dialogueContainerSO);
            SaveNodes(dialogueContainerSO);

            EditorUtility.SetDirty(dialogueContainerSO);
            AssetDatabase.SaveAssets();
        }

        public void Load(DialogueContainerSO dialogueContainerSO)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSO);
            ConnectNodes(dialogueContainerSO);
        }

        void SaveEdges(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.NodeLinkDatas.Clear();

            var connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
            foreach (var connectedEdge in connectedEdges)
            {
                var outputNode = connectedEdge.output.node as BaseNode;
                var inputNode = connectedEdge.input.node as BaseNode;

                dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    BasePortName = connectedEdge.output.portName,
                    TargetNodeGUID = inputNode.GUID,
                    TargetPortName = connectedEdge.input.portName,
                });
            }
        }

        void SaveNodes(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.EventDatas.Clear();
            dialogueContainerSO.EndDatas.Clear();
            dialogueContainerSO.StartDatas.Clear();
            dialogueContainerSO.BranchDatas.Clear();
            dialogueContainerSO.DialogueDatas.Clear();
            dialogueContainerSO.ChoiceDatas.Clear();

            nodes.ForEach(node =>
            {
                switch (node)
                {
                    case DialogueNode dialogueNode:
                        dialogueContainerSO.DialogueDatas.Add(SaveNodeData(dialogueNode));
                        break;
                    case StartNode startNode:
                        dialogueContainerSO.StartDatas.Add(SaveNodeData(startNode));
                        break;
                    case EndNode endNode:
                        dialogueContainerSO.EndDatas.Add(SaveNodeData(endNode));
                        break;
                    case EventNode eventNode:
                        dialogueContainerSO.EventDatas.Add(SaveNodeData(eventNode));
                        break;
                    case BranchNode branchNode:
                        dialogueContainerSO.BranchDatas.Add(SaveNodeData(branchNode));
                        break;
                    case ChoiceNode choiceNode:
                        dialogueContainerSO.ChoiceDatas.Add(SaveNodeData(choiceNode));
                        break;
                    default:
                        break;
                }
            });
        }

        DialogueData SaveNodeData(DialogueNode node)
        {
            var dialogueData = new DialogueData
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
            };

            // Set Order
            for (int i = 0; i < node.DialogueData.BaseDialogueDataContainers.Count; i++)
                node.DialogueData.BaseDialogueDataContainers[i].Order.Value = i;

            foreach (var baseContainer in node.DialogueData.BaseDialogueDataContainers)
            {
                // Name
                if (baseContainer is DialogueDataName)
                {
                    var tmp = (baseContainer as DialogueDataName);
                    var tmpData = new DialogueDataName();

                    tmpData.Order.Value = tmp.Order.Value;
                    tmpData.CharacterName.Value = tmp.CharacterName.Value;

                    dialogueData.DialogueDataNames.Add(tmpData);
                }

                // Text
                if (baseContainer is DialogueDataText)
                {
                    var tmp = (baseContainer as DialogueDataText);
                    var tmpData = new DialogueDataText();

                    tmpData.Order = tmp.Order;
                    tmpData.GUID = tmp.GUID;
                    tmpData.Texts = tmp.Texts;
                    tmpData.AudioClips = tmp.AudioClips;

                    dialogueData.DialogueDataTexts.Add(tmpData);
                }

                // Images
                if (baseContainer is DialogueDataImage)
                {
                    var tmp = (baseContainer as DialogueDataImage);
                    var tmpData = new DialogueDataImage();

                    tmpData.Order.Value = tmp.Order.Value;
                    tmpData.SpriteLeft.Value = tmp.SpriteLeft.Value;
                    tmpData.SpriteRight.Value = tmp.SpriteRight.Value;

                    dialogueData.DialogueDataImages.Add(tmpData);
                }
            }

            // Port
            foreach (DialogueDataPort port in node.DialogueData.DialogueDataPorts)
            {
                var portData = new DialogueDataPort();

                portData.OutputGUID = string.Empty;
                portData.InputGUID = string.Empty;
                portData.PortGUID = port.PortGUID;

                foreach (var edge in edges)
                {
                    if (edge.output.portName == port.PortGUID)
                    {
                        portData.OutputGUID = (edge.output.node as BaseNode).GUID;
                        portData.InputGUID = (edge.input.node as BaseNode).GUID;
                    }
                }

                dialogueData.DialogueDataPorts.Add(portData);
            }

            return dialogueData;
        }

        StartData SaveNodeData(StartNode node)
        {
            var nodeData = new StartData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
            };

            return nodeData;
        }

        EndData SaveNodeData(EndNode node)
        {
            var nodeData = new EndData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
            };
            nodeData.EnumType.Value = node.EndData.EnumType.Value;

            return nodeData;
        }

        EventData SaveNodeData(EventNode node)
        {
            var nodeData = new EventData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
            };

            // Save Dialogue Event
            foreach (var dialogueEvent in node.EventData.DialogueEventSOs)
                nodeData.DialogueEventSOs.Add(dialogueEvent);

            // Save String Event
            foreach (var stringEvents in node.EventData.EventDataModifiers)
            {
                var tmp = new EventData<EventModifierType>();
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.EventText.Value = stringEvents.EventText.Value;
                tmp.EnumType.Value = stringEvents.EnumType.Value;
                nodeData.EventDataModifiers.Add(tmp);
            }

            return nodeData;
        }

        BranchData SaveNodeData(BranchNode node)
        {
            var tmpEdges = edges.Where(x => x.output.node == node).Cast<Edge>().ToList();

            var trueOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == BranchNode.s_TruePortName);
            var falseOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == BranchNode.s_FalsePortName);

            var nodeData = new BranchData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
                TrueGUIDNode = (trueOutput != null ? (trueOutput.input.node as BaseNode).GUID : string.Empty),
                FalseGUIDNode = (falseOutput != null ? (falseOutput.input.node as BaseNode).GUID : string.Empty),
            };

            foreach (var stringEvents in node.BranchData.EventDataConditions)
            {
                var tmp = new EventData<EventConditionType>();
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.EventText.Value = stringEvents.EventText.Value;
                tmp.EnumType.Value = stringEvents.EnumType.Value;
                nodeData.EventDataConditions.Add(tmp);
            }

            return nodeData;
        }

        ChoiceData SaveNodeData(ChoiceNode node)
        {
            var nodeData = new ChoiceData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,

                Texts = node.ChoiceData.Texts,
                AudioClips = node.ChoiceData.AudioClips,
            };
            nodeData.ChoiceStateType.Value = node.ChoiceData.ChoiceStateType.Value;

            foreach (var stringEvents in node.ChoiceData.EventDataConditions)
            {
                var tmp = new EventData<EventConditionType>();
                tmp.EventText.Value = stringEvents.EventText.Value;
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.EnumType.Value = stringEvents.EnumType.Value;
                nodeData.EventDataConditions.Add(tmp);
            }

            return nodeData;
        }

        void ClearGraph()
        {
            edges.ForEach(edge => graphView.RemoveElement(edge));
            foreach (var node in nodes) graphView.RemoveElement(node);
        }

        void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            // Start
            foreach (var node in dialogueContainer.StartDatas)
            {
                var tempNode = graphView.CreateNode<StartNode>(node.Position);
                tempNode.GUID = node.GUID;
                graphView.AddElement(tempNode);
            }

            // End Node 
            foreach (var node in dialogueContainer.EndDatas)
            {
                var tempNode = graphView.CreateNode<EndNode>(node.Position);
                tempNode.GUID = node.GUID;
                tempNode.EndData.EnumType.Value = node.EnumType.Value;

                tempNode.LoadValueIntoField();
                graphView.AddElement(tempNode);
            }

            // Event Node
            foreach (var node in dialogueContainer.EventDatas)
            {
                var tempNode = graphView.CreateNode<EventNode>(node.Position);
                tempNode.GUID = node.GUID;

                foreach (var item in node.DialogueEventSOs) tempNode.AddScriptableEvent(item);
                foreach (var item in node.EventDataModifiers) tempNode.AddEvent(item);

                tempNode.LoadValueIntoField();
                graphView.AddElement(tempNode);
            }

            // Breach Node
            foreach (BranchData node in dialogueContainer.BranchDatas)
            {
                var tempNode = graphView.CreateNode<BranchNode>(node.Position);
                tempNode.GUID = node.GUID;

                foreach (var item in node.EventDataConditions) tempNode.AddCondition(item);

                tempNode.LoadValueIntoField();
                tempNode.ReloadLanguage();
                graphView.AddElement(tempNode);
            }

            // Choice Node
            foreach (var node in dialogueContainer.ChoiceDatas)
            {
                var tempNode = graphView.CreateNode<ChoiceNode>(node.Position);
                tempNode.GUID = node.GUID;

                tempNode.ChoiceData.ChoiceStateType.Value = node.ChoiceStateType.Value;

                foreach (var dataText in node.Texts)
                    foreach (var editorText in tempNode.ChoiceData.Texts)
                        if (editorText.LanguageType == dataText.LanguageType)
                            editorText.LanguageGenericType = dataText.LanguageGenericType;

                foreach (var dataAudioClip in node.AudioClips)
                    foreach (var editorAudioClip in tempNode.ChoiceData.AudioClips)
                        if (editorAudioClip.LanguageType == dataAudioClip.LanguageType)
                            editorAudioClip.LanguageGenericType = dataAudioClip.LanguageGenericType;

                foreach (var item in node.EventDataConditions) tempNode.AddCondition(item);

                tempNode.LoadValueIntoField();
                tempNode.ReloadLanguage();
                graphView.AddElement(tempNode);
            }

            // Dialogue Node
            foreach (DialogueData node in dialogueContainer.DialogueDatas)
            {
                var tempNode = graphView.CreateNode<DialogueNode>(node.Position);
                tempNode.GUID = node.GUID;

                var data_BaseContainer = new List<BaseDialogueDataContainer>();

                data_BaseContainer.AddRange(node.DialogueDataImages);
                data_BaseContainer.AddRange(node.DialogueDataTexts);
                data_BaseContainer.AddRange(node.DialogueDataNames);

                data_BaseContainer.Sort(delegate (BaseDialogueDataContainer x, BaseDialogueDataContainer y)
                {
                    return x.Order.Value.CompareTo(y.Order.Value);
                });

                foreach (var data in data_BaseContainer)
                {
                    switch (data)
                    {
                        case DialogueDataName Name:
                            tempNode.CharacterName(Name);
                            break;
                        case DialogueDataText Text:
                            tempNode.TextLine(Text);
                            break;
                        case DialogueDataImage image:
                            tempNode.ImagePicture(image);
                            break;
                        default:
                            break;
                    }
                }

                foreach (var port in node.DialogueDataPorts) tempNode.AddChoicePort(tempNode, port);

                tempNode.LoadValueIntoField();
                tempNode.ReloadLanguage();
                graphView.AddElement(tempNode);
            }
        }

        void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            // Make connection for all node.
            foreach (var node in nodes)
            {
                var connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGUID == node.GUID).ToList();

                var allOutputPorts = node.outputContainer.Children().Where(x => x is Port).Cast<Port>().ToList();

                foreach (var connection in connections)
                {
                    var targetNodeGUID = connection.TargetNodeGUID;
                    var targetNode = nodes.First(node => node.GUID == targetNodeGUID);

                    if (targetNode == null) continue;

                    foreach (var port in allOutputPorts)
                        if (port.portName == connection.BasePortName)
                            LinkNodesTogether(port, (Port)targetNode.inputContainer[0]);
                }
            }
        }

        void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            var tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            graphView.Add(tempEdge);
        }
    }
}