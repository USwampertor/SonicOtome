using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class DialogueGraphView : GraphView
{
  private readonly Vector2 defaultNodeSize = new Vector2(200, 150);

  public DialogueGraphView()
  {
    styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

    SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);


    this.AddManipulator(new ContentDragger());
    this.AddManipulator(new SelectionDragger());
    this.AddManipulator(new SelectionDropper());
    this.AddManipulator(new RectangleSelector());
    
    // this.AddManipulator(new ContentZoomer());

    var gridBackground = new GridBackground();
    Insert(0, gridBackground );
    gridBackground.StretchToParentSize();
    AddElement(GenerateEntryPointNode());

  }

  public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
  {
    var compatiblePorts = new List<Port>();

    foreach(var port in ports)
    {
      if(startPort!=port && startPort.node != port.node)
      {
        compatiblePorts.Add(port);
      }
    }

    return compatiblePorts;
  }

  private Port GeneratePort(DialogueNode node, 
                            Direction portDirection, 
                            Port.Capacity capacity = Port.Capacity.Single)
  {
    return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
  }


  private DialogueNode GenerateEntryPointNode()
  {
    var node = new DialogueNode
    {
      title = "Start",
      GUID = Guid.NewGuid().ToString(),
      DialogueText = "ENTRYPOINT",
      EntryPoint = true
    };

    var generatedPort = GeneratePort(node, Direction.Output);
    generatedPort.portName = "Next";
    node.outputContainer.Add(generatedPort);

    node.RefreshExpandedState();
    node.RefreshPorts();

    node.SetPosition(new Rect(100, 200, 100, 150));
    return node;
  }

  public void CreateNode(string name)
  {
    AddElement(CreateDialogueNode(name));
  }

  public DialogueNode CreateDialogueNode(string nodeName)
  {
    var dialogueNode = new DialogueNode
    {
      title = nodeName,
      GUID = Guid.NewGuid().ToString(),
      DialogueText = "New Description"
    };

    var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
    inputPort.portName = "Input";
    dialogueNode.inputContainer.Add(inputPort);

    var button = new Button(() => 
    {
      AddChoicePort(dialogueNode);
    });
    button.text = "New Choice";
    dialogueNode.titleContainer.Add(button);


    dialogueNode.RefreshExpandedState();
    dialogueNode.RefreshPorts();

    dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));


    return dialogueNode;
  }

  private void AddChoicePort(DialogueNode dialogueNode)
  {
    var generatePort = GeneratePort(dialogueNode, Direction.Output);

    var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
    var outputPortName = $"Choice {outputPortCount}";
    dialogueNode.outputContainer.Add(generatePort);

    dialogueNode.RefreshExpandedState();
    dialogueNode.RefreshPorts();

  }

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
