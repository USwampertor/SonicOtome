using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

public class OtomeSettingsWindow : EditorWindow
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public static void ResetCharacterDataPath()
  {
    GetWindow<CharacterGraph>().characterDataPath =  Application.persistentDataPath + "/characters/";

  }

  public static void WriteToEnum<T>(string path, string name, ICollection<T> data)
  {

  }

  void UpdateeMOOD(string newValue)
  {
    if (newValue == null) { return; }
  }
}
