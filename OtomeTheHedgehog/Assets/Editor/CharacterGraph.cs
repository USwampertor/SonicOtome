using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Leguar.TotalJSON;
using UnityEditor.ShaderGraph.Serialization;
using System.IO;
using System;

public class CharacterGraph : EditorWindow
{
  // Visual items

  Toolbar toolbar = null;

  TwoPaneSplitView splitView = null;
  ListView leftPane =  null;
  VisualElement rightPane =  null;

  TextField currentCharacterName = null;
  ObjectField currentCharacterSprite = null;
  Image tex2D = null;

  List<CharacterData> characterNames = new List<CharacterData>();

  public string characterDataPath;

  private static string characterExtension = "cdp";

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  private void OnEnable()
  {
    GenerateToolbar();
    GeneratePanel();
    UpdateLeftPane();
    OtomeSettingsWindow.ResetCharacterDataPath();
  }

  private void OnDisable()
  {
    
  }

  private void GenerateToolbar()
  {
    if (toolbar != null) { return; }

    toolbar = new Toolbar();
    Button fileButton = new Button();
    fileButton.text = "File";
    fileButton.clicked += () =>
    {
      GenericMenu menu = new GenericMenu();
      menu.AddItem(new GUIContent("New Character..."), false, () => {
        Debug.Log("Creating Character");
        CreateCharacter();
      });
      menu.AddItem(new GUIContent("Load Character..."), false, () => {
        OpenCharacter();
      });
      menu.AddItem(new GUIContent("Recent Character"), false, () => {
        
      });
      menu.AddItem(new GUIContent("Load Folder..."), false, () => { 
        OpenAllCharactersFromFolder();
      });
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("Save Character..."), false, () => {
        SaveCharacter(leftPane.selectedIndex);
      });
      menu.AddItem(new GUIContent("Save All Characters"), false, () => {
        SaveAllCharacters();
      });
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("Remove Character..."), false, () => {
        Debug.Log("Removing Character");
        RemoveCharacter();
      });
      menu.AddItem(new GUIContent("Delete Character..."), false, () => {
        DeleteCharacter();
      });

      menu.DropDown(new Rect(fileButton.contentRect.xMin - 5,
                              fileButton.contentRect.yMax + 10,
                              fileButton.contentRect.width,
                              fileButton.contentRect.height));
    };

    toolbar.Add(fileButton);



    toolbar.Add(new Button(() => {
      characterNames.Clear();
      UpdateLeftPane();
    }) { text = "Delete all" });
    // toolbar.Add(new Button(() => {}) { text = "Load All Characters" });
    // toolbar.Add(new Button(() => {}) { text = "Create new Character" });
    // toolbar.Add(new Button(() => {}) { text = "Save Character" });
    // toolbar.Add(new Button(() => {}) { text = "Save Character as..." });
    // toolbar.Add(new Button(() => {}) { text = "Save All Characters" });

    var nodeCreateButton = new Button(() => {
     
    });

    rootVisualElement.Add(toolbar);
  }

  private void GeneratePanel()
  {
    if (splitView != null) { return; }
    splitView = new TwoPaneSplitView(0, 150, TwoPaneSplitViewOrientation.Horizontal);

    // Add the view to the visual tree by adding it as a child to the root element
    rootVisualElement.Add(splitView);

    // A TwoPaneSplitView always needs exactly two child elements
    leftPane = new ListView();
    leftPane.onSelectionChange += (obj) =>
    {
      UpdateRightPane(leftPane.selectedIndex);
    };

    splitView.Add(leftPane);
    
    rightPane = new VisualElement();

    currentCharacterName = new TextField("Name: ");
    currentCharacterName.RegisterValueChangedCallback((s) => { 
      characterNames[leftPane.selectedIndex].name = s.newValue;
      UpdateLeftPane();
    } );
    rightPane.Add(currentCharacterName);

    // terrainTexture = (Texture)EditorGUILayout.ObjectField("texture ", terrainTexture, typeof(Texture), true);
    
    currentCharacterSprite = new ObjectField();
    currentCharacterSprite.label = "Avatar";
    currentCharacterSprite.objectType = typeof(Sprite);
    currentCharacterSprite.allowSceneObjects = true;

    tex2D = new Image();
    tex2D.sourceRect = new Rect(0, 0, 100, 100);

    currentCharacterSprite.RegisterValueChangedCallback((e) => {
      tex2D.sprite = (Sprite)e.newValue;
      characterNames[leftPane.selectedIndex].profile.sprite = (Sprite)e.newValue;
    });


    rightPane.Add(currentCharacterSprite);
    rightPane.Add(tex2D);

    splitView.Add(rightPane);

    currentCharacterName.SetEnabled(leftPane.selectedIndex > -1);
    currentCharacterSprite.SetEnabled(leftPane.selectedIndex > -1);

    rightPane.Add(new Image());
  }

  private void CreateCharacter()
  {
    characterNames.Add(new CharacterData("New Character Name" + characterNames.Count));
    UpdateLeftPane();
  }

  private void OpenCharacter()
  {
    string[] extensions = { characterExtension, "json" };
    string path = EditorUtility.OpenFilePanelWithFilters("Load Character", 
                                                         characterDataPath, 
                                                         extensions);
    Debug.Log(path);
    if (path != null)
    {
      LoadCharacter(path);
    }
  }



  private void LoadCharacter(string characterPath)
  {
    StreamReader sr = new StreamReader(characterPath);
    JSON characterJSON = JSON.ParseString(sr.ReadToEnd());

    if (characterJSON == null) { return; }
    if (characterJSON.GetString("name") == null) { return; }

    // Check if the character is already loaded
    if(characterNames.Find(x => x.name == characterJSON.GetString("name")) != null) { return; }

    CharacterData newCharacter = new CharacterData(characterJSON);

    characterNames.Add(newCharacter);
    UpdateLeftPane();
  }

  private void OpenAllCharactersFromFolder()
  {
    string pathToSearch = EditorUtility.OpenFolderPanel("Load Folder", 
                                                        characterDataPath, 
                                                        characterExtension);
    if (Directory.Exists(pathToSearch))
    {
      var files = Directory.GetFiles(pathToSearch);
      foreach (var file in files)
      {
        LoadCharacter(file);
      }
    }
  }

  private void SaveCharacter(int selectedIndex)
  {
    if (selectedIndex < 0 || characterNames.Count <= 0) { return; }
    var activeCharacter = characterNames[selectedIndex];
    JSON jsonCharacter = activeCharacter.ToJSON(); 


    string jsonString = jsonCharacter.CreateString();


    string characterPath = characterDataPath + activeCharacter.name + "." + characterExtension;
    Debug.Log(characterPath);
    
    if (!Directory.Exists(characterDataPath))
    {
      Directory.CreateDirectory(characterDataPath);
    }

    StreamWriter file = File.CreateText(characterDataPath + activeCharacter.name + "." + characterExtension);
    
    file.Write(jsonString);
    file.Close();
  }

  private void SaveAllCharacters()
  {
    for(int i = 0; i < characterNames.Count; ++i)
    {
      SaveCharacter(i);
    }
  }

  private void DeleteCharacter()
  {
  }

  private void RemoveCharacter()
  {
    characterNames.RemoveAt(leftPane.selectedIndex);
    UpdateLeftPane();
    UpdateRightPane(-1);
  }

  private void UpdateLeftPane()
  {
    leftPane.makeItem = () => new Label();
    leftPane.bindItem = (item, index) => { (item as Label).text = characterNames[index].name; };
    leftPane.itemsSource = characterNames;
  }

  private void UpdateRightPane(int index)
  {
    Debug.Log("Updating");
    currentCharacterName.SetEnabled(leftPane.selectedIndex > -1);
    currentCharacterSprite.SetEnabled(leftPane.selectedIndex > -1);

    if (index < 0)
    {
      tex2D.sprite = null;
    }
    else
    {
      currentCharacterName.value = characterNames[index].name;
      currentCharacterSprite.value = characterNames[index].profile.sprite;
    }
  }
}
