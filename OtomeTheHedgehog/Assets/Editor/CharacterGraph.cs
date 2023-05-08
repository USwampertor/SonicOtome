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
using UnityEditorInternal;
using System.Linq;
using Unity.VisualScripting;

public class CharacterGraph : EditorWindow
{
  // Visual items

  Toolbar toolbar = null;

  TwoPaneSplitView splitView = null;
  ListView leftPane = null;
  ListView moodList = null;
  VisualElement rightPane = null;

  List<ObjectField> emotionsViewer = null;

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
      UpdateRightPane(-1);
    }) { text = "Delete all" });

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
    leftPane.headerTitle = "Characters";
    leftPane.onSelectionChange += (obj) =>
    {
      UpdateRightPane(leftPane.selectedIndex);
    };

    splitView.Add(leftPane);

    emotionsViewer = new List<ObjectField>();

    rightPane = new VisualElement();

    currentCharacterName = new TextField("Name: ");
    currentCharacterName.RegisterValueChangedCallback((s) => {
      characterNames[leftPane.selectedIndex].name = s.newValue;
      UpdateLeftPane();
    });
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

    splitView.Add(rightPane);

    currentCharacterName.SetEnabled(leftPane.selectedIndex > -1);
    currentCharacterSprite.SetEnabled(leftPane.selectedIndex > -1);



    emotionsViewer.Clear();
    foreach (var mood in (eMOOD[]) Enum.GetValues(typeof(eMOOD)))
    {

      var spriteObject = new ObjectField();
      spriteObject.objectType = typeof(Sprite);
      // list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
      // list.showFoldoutHeader = true;
      // list.headerTitle = mood.ToString() + " animation list";
      // list.showAddRemoveFooter = true;
      // list.reorderMode = ListViewReorderMode.Animated;

      emotionsViewer.Add(spriteObject);

      spriteObject.name = mood.ToString() + " sprite";
      spriteObject.label = mood.ToString() + " sprite";
      spriteObject.RegisterValueChangedCallback((e) => {
        // tex2D.sprite = (Sprite)e.newValue;
        characterNames[leftPane.selectedIndex].emotions.Find(x => x.Item1 == mood).Item2.sprite = (Sprite)e.newValue;
      });
      spriteObject.SetEnabled(leftPane.selectedIndex > -1);
      rightPane.Add(spriteObject);
    }
    rightPane.Add(tex2D);
    


    // moodList = new ReorderableList(new IList(), typeof(ObjectField));
  }

  private void CreateCharacter()
  {
    characterNames.Add(new CharacterData("New Character Name" + characterNames.Count));
    UpdateLeftPane();
  }

  private void OpenCharacter()
  {
    string[] extensions = { "cdp", "json" };
    string path = EditorUtility.OpenFilePanel("Load Character", characterDataPath, "cdp,json");
    Debug.Log(path);
    if (path != null)
    {
      if (File.Exists(path) &&
          (path.Substring(path.LastIndexOf(".")).Contains(extensions[0]) ||
           path.Substring(path.LastIndexOf(".")).Contains(extensions[1])))
      {
        LoadCharacter(path);
      }
    }
  }



  private void LoadCharacter(string characterPath)
  {
    StreamReader sr = new StreamReader(characterPath);
    JSON characterJSON = JSON.ParseString(sr.ReadToEnd());

    if (characterJSON == null) { return; }
    if (characterJSON.GetString("name") == null) { return; }

    // Check if the character is already loaded
    if (characterNames.Find(x => x.name == characterJSON.GetString("name")) != null) { return; }

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
    for (int i = 0; i < characterNames.Count; ++i)
    {
      SaveCharacter(i);
    }
  }

  private void DeleteCharacter()
  {
    RemoveCharacter();
  }

  private void RemoveCharacter()
  {
    characterNames.RemoveAt(leftPane.selectedIndex);
    UpdateLeftPane();
    UpdateRightPane(-1);
  }

  public void UpdateEmotionList(int index)
  {
    for (int i = 0; i < emotionsViewer.Count; ++i)
    {
      var sprite = emotionsViewer[i];
      sprite.value = index < 0 ? null : characterNames[index].emotions[i].Item2.sprite;

      sprite.SetEnabled(index < 0 ? false : true);
    }

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
      var activeChar = characterNames[index];

      currentCharacterName.value = activeChar.name;
      if (activeChar.profile != null)
      {
        currentCharacterSprite.value = activeChar.profile.sprite ? activeChar.profile.sprite : null;
      }
    }
    UpdateEmotionList(index);
  }
}
