using Leguar.TotalJSON;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class OtomeWindowUtilities
{
  [MenuItem("Otome Toolkit/Dialogue Editor")]
  public static void OpenDialogGraphWindow()
  {
    var window = EditorWindow.GetWindow<DialogueGraph>();
    window.titleContent = new GUIContent("Dialogue Editor Window");
  }
  
  [MenuItem("Otome Toolkit/Character Manager")]
  public static void OpenCharacterManagerWindow()
  {
    var window = EditorWindow.GetWindow<CharacterGraph>();
    window.titleContent = new GUIContent("Character Editor Window");
  }

  [MenuItem("Otome Toolkit/Otome Settings")]
  public static void OpenOtomeSettingsWindow()
  {
    var window = EditorWindow.GetWindow<OtomeSettingsWindow>();
    window.titleContent = new GUIContent("Otome Toolkit Settings");
  }


  

}
public static class OtomeExtensions
{
    

}

