using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace Dialogue.Editor
{
    public class DialogueNode : BaseNode
    {
        public DialogueData DialogueData { get; set; } = new DialogueData();
        List<Box> _boxes = new List<Box>();
        public DialogueNode() { }
        public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView)
        {
            title = "Dialogue";
            styleSheets.Add(Resources.Load<StyleSheet>(@"USS/Dialogue/Node/DialogueNode"));

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Continue");
            TopContainer();
        }

        void TopContainer()
        {
            AddPortButton();
            AddDropdownMenu();
        }


        void AddPortButton()
        {
            var button = new Button(() => AddChoicePort(this)) { text = "Add Choice" };
            button.AddToClassList("TopButton");
            titleButtonContainer.Add(button);
        }

        void AddDropdownMenu()
        {
            var Menu = new ToolbarMenu();
            Menu.text = "Add Content";

            Menu.menu.AppendAction("Text", new Action<DropdownMenuAction>(x => TextLine()));
            Menu.menu.AppendAction("Image", new Action<DropdownMenuAction>(x => ImagePicture()));
            Menu.menu.AppendAction("Name", new Action<DropdownMenuAction>(x => CharacterName()));

            titleButtonContainer.Add(Menu);
        }

        // Port ---------------------------------------------------------------------------------------

        public Port AddChoicePort(BaseNode baseNode, DialogueDataPort dialogueDataPort = null)
        {
            var port = GetPortInstance(Direction.Output);
            var newDialoguePort = new DialogueDataPort();

            // Check if we load it in with values
            if (dialogueDataPort != null)
            {
                newDialoguePort.InputGUID = dialogueDataPort.InputGUID;
                newDialoguePort.OutputGUID = dialogueDataPort.OutputGUID;
                newDialoguePort.PortGUID = dialogueDataPort.PortGUID;
            }
            else
            {
                newDialoguePort.PortGUID = Guid.NewGuid().ToString();
            }

            // Delete button
            port.contentContainer.Add(new Button(() => DeletePort(baseNode, port)) { text = "X" });

            port.portName = newDialoguePort.PortGUID;
            var portNameLabel = port.contentContainer.Q<Label>("type");
            portNameLabel.AddToClassList("PortName");

            // Set color of the port.
            port.portColor = Color.yellow;

            DialogueData.DialogueDataPorts.Add(newDialoguePort);

            baseNode.outputContainer.Add(port);

            // Refresh
            baseNode.RefreshPorts();
            baseNode.RefreshExpandedState();

            return port;
        }

        void DeletePort(BaseNode node, Port port)
        {
            var tmp = DialogueData.DialogueDataPorts.Find(findPort => findPort.PortGUID == port.portName);
            DialogueData.DialogueDataPorts.Remove(tmp);

            var portEdge = GraphView.edges.ToList().Where(edge => edge.output == port);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                GraphView.RemoveElement(edge);
            }

            node.outputContainer.Remove(port);

            // Refresh
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        // Menu dropdown --------------------------------------------------------------------------------------

        public void TextLine(DialogueDataText dialogueDataText = null)
        {
            var newDialogueBaseContainerText = new DialogueDataText();
            DialogueData.DialogueDataBaseContainers.Add(newDialogueBaseContainerText);

            // Add Container Box
            var boxContainer = new Box();
            boxContainer.AddToClassList("DialogueBox");

            // Add Fields
            AddLabelAndButton(newDialogueBaseContainerText, boxContainer, "Text", "TextColor");
            AddTextField(newDialogueBaseContainerText, boxContainer);
            AddAudioClips(newDialogueBaseContainerText, boxContainer);

            // Load in data if it got any
            if (dialogueDataText != null)
            {
                // Guid ID
                newDialogueBaseContainerText.GUID = dialogueDataText.GUID;

                // Text
                foreach (var dataText in dialogueDataText.Texts)
                    foreach (var text in newDialogueBaseContainerText.Texts)
                        if (text.LanguageType == dataText.LanguageType)
                            text.LanguageGenericType = dataText.LanguageGenericType;

                // Audio
                foreach (var dataAudioclip in dialogueDataText.AudioClips)
                    foreach (var audioclip in newDialogueBaseContainerText.AudioClips)
                        if (audioclip.LanguageType == dataAudioclip.LanguageType)
                            audioclip.LanguageGenericType = dataAudioclip.LanguageGenericType;
            }
            else
                // Make New Guid ID
                newDialogueBaseContainerText.GUID.Value = Guid.NewGuid().ToString();

            // Reload the current selected language
            ReloadLanguage();

            mainContainer.Add(boxContainer);
        }

        public void ImagePicture(DialogueDataImage dataImage = null)
        {
            var dialogueImage = new DialogueDataImage();
            if (dataImage != null)
            {
                dialogueImage.SpriteLeft.Value = dataImage.SpriteLeft.Value;
                dialogueImage.SpriteRight.Value = dataImage.SpriteRight.Value;
            }
            DialogueData.DialogueDataBaseContainers.Add(dialogueImage);

            var boxContainer = new Box();
            boxContainer.AddToClassList("DialogueBox");

            AddLabelAndButton(dialogueImage, boxContainer, "Image", "ImageColor");
            AddImages(dialogueImage, boxContainer);

            mainContainer.Add(boxContainer);
        }

        public void CharacterName(DialogueDataName dataName = null)
        {
            var dialogueName = new DialogueDataName();
            if (dataName != null)
                dialogueName.CharacterName.Value = dataName.CharacterName.Value;
            DialogueData.DialogueDataBaseContainers.Add(dialogueName);

            var boxContainer = new Box();
            boxContainer.AddToClassList("CharacterNameBox");

            AddLabelAndButton(dialogueName, boxContainer, "Name", "NameColor");
            AddTextFieldCharacterName(dialogueName, boxContainer);

            mainContainer.Add(boxContainer);
        }

        // Fields --------------------------------------------------------------------------------------

        void AddLabelAndButton(DialogueDataBaseContainer container, Box boxContainer, string labelName, string uniqueUSS = "")
        {
            var topBoxContainer = new Box();
            topBoxContainer.AddToClassList("TopBox");

            // Label Name
            var labelTexts = GetNewLabel(labelName, "LabelText", uniqueUSS);

            var buttonsBox = new Box();
            buttonsBox.AddToClassList("ButtonBox");

            // Move Up button.
            var moveUpButton = GetNewButton("", () => MoveBox(container, true), "MoveUpButton");

            // Move Down button.
            var moveDownButton = GetNewButton("", () => MoveBox(container, false), "MoveDownButton");

            // Remove button.
            var removeButton = GetNewButton("X",
            () =>
            {
                DeleteBox(boxContainer);
                _boxes.Remove(boxContainer);
                DialogueData.DialogueDataBaseContainers.Remove(container);
            }, "TextRemoveButton");

            _boxes.Add(boxContainer);

            buttonsBox.Add(moveUpButton);
            buttonsBox.Add(moveDownButton);
            buttonsBox.Add(removeButton);
            topBoxContainer.Add(labelTexts);
            topBoxContainer.Add(buttonsBox);

            boxContainer.Add(topBoxContainer);
        }

        void AddTextFieldCharacterName(DialogueDataName container, Box boxContainer)
        {
            var textField = GetNewTextField(container.CharacterName, "Name", "CharacterName");
            boxContainer.Add(textField);
        }

        void AddTextField(DialogueDataText container, Box boxContainer)
        {
            var textField = GetNewTextFieldTextLanguage(container.Texts, "Text areans", "TextBox");
            container.TextField = textField;
            boxContainer.Add(textField);
        }

        void AddAudioClips(DialogueDataText container, Box boxContainer)
        {
            var objectField = GetNewObjectFieldAudioClipsLanguage(container.AudioClips, "AudioClip");
            container.ObjectField = objectField;
            boxContainer.Add(objectField);
        }

        void AddImages(DialogueDataImage container, Box boxContainer)
        {
            var imagePreviewBox = new Box();
            var imagesBox = new Box();

            imagePreviewBox.AddToClassList("BoxRow");
            imagesBox.AddToClassList("BoxRow");

            // Set up Image Preview.
            var leftImage = GetNewImage("ImagePreview", "ImagePreviewLeft");
            var rightImage = GetNewImage("ImagePreview", "ImagePreviewRight");

            imagePreviewBox.Add(leftImage);
            imagePreviewBox.Add(rightImage);

            // Set up Sprite.
            var objectFieldLeft = GetNewObjectFieldSprite(container.SpriteLeft, leftImage, "SpriteLeft");
            var objectFieldRight = GetNewObjectFieldSprite(container.SpriteRight, rightImage, "SpriteRight");

            imagesBox.Add(objectFieldLeft);
            imagesBox.Add(objectFieldRight);

            // Add to box container.
            boxContainer.Add(imagePreviewBox);
            boxContainer.Add(imagesBox);
        }

        // ------------------------------------------------------------------------------------------

        void MoveBox(DialogueDataBaseContainer container, bool moveUp)
        {
            var tmpDialogueBaseContainers = new List<DialogueDataBaseContainer>();
            tmpDialogueBaseContainers.AddRange(DialogueData.DialogueDataBaseContainers);

            foreach (Box box in _boxes)
                mainContainer.Remove(box);
            _boxes.Clear();

            for (int i = 0; i < tmpDialogueBaseContainers.Count; i++)
                tmpDialogueBaseContainers[i].ID.Value = i;


            if (container.ID.Value > 0 && moveUp)
            {
                DialogueDataBaseContainer tmp01 = tmpDialogueBaseContainers[container.ID.Value];
                DialogueDataBaseContainer tmp02 = tmpDialogueBaseContainers[container.ID.Value - 1];

                tmpDialogueBaseContainers[container.ID.Value] = tmp02;
                tmpDialogueBaseContainers[container.ID.Value - 1] = tmp01;
            }
            else if (container.ID.Value < tmpDialogueBaseContainers.Count - 1 && !moveUp)
            {
                DialogueDataBaseContainer tmp01 = tmpDialogueBaseContainers[container.ID.Value];
                DialogueDataBaseContainer tmp02 = tmpDialogueBaseContainers[container.ID.Value + 1];

                tmpDialogueBaseContainers[container.ID.Value] = tmp02;
                tmpDialogueBaseContainers[container.ID.Value + 1] = tmp01;
            }

            DialogueData.DialogueDataBaseContainers.Clear();

            foreach (DialogueDataBaseContainer dialogueDataBaseContainer in tmpDialogueBaseContainers)
            {
                switch (dialogueDataBaseContainer)
                {
                    case DialogueDataName Name:
                        CharacterName(Name);
                        break;
                    case DialogueDataText Text:
                        TextLine(Text);
                        break;
                    case DialogueDataImage image:
                        ImagePicture(image);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void ReloadLanguage()
        {
            base.ReloadLanguage();
        }

        public override void LoadValueIntoField()
        {

        }
    }
}