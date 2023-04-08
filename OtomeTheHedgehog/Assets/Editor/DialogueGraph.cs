using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
  private DialogueGraphView _graphView;
  private string _fileName = "New Narrative";

  private void ConstructGraphView()
  { 
    _graphView = new DialogueGraphView 
    { 
      name = "Dialogue Graph"
    };

    _graphView.StretchToParentSize();
    rootVisualElement.Add(_graphView);
  }

  private void GenerateToolbar()
  {
    var toolbar = new Toolbar();

    var nameField = new TextField("File Name: ");
    nameField.SetValueWithoutNotify(_fileName);
    nameField.MarkDirtyRepaint();
    nameField.RegisterValueChangedCallback(e => {
      _fileName = e.newValue;
    });
    toolbar.Add(nameField);

    toolbar.Add(new Button(() => SaveData()) { text = "Save data"});
    toolbar.Add(new Button(() => LoadData()) { text = "Load data"});


    var nodeCreateButton = new Button(() => {
      _graphView.CreateNode("New Dialogue Node");
    });
    nodeCreateButton.text = "Add New Node";
    toolbar.Add(nodeCreateButton);


    rootVisualElement.Add(toolbar);

  }

  private void SaveData()
  {
    Debug.Log("Saving Data");
  }

  private void LoadData()
  {
    Debug.Log("Load Data");
  }


  private void OnEnable()
  {
    ConstructGraphView();
    GenerateToolbar();
  }

  private void OnDisable()
  {
    rootVisualElement.Remove(_graphView);
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
