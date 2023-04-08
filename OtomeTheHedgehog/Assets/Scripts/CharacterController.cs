using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;
using static UnityEngine.Rendering.DebugUI;

public enum eSCREENSIDE
{
  LEFT = 0, 
  RIGHT = 1, 
  FRONT = 2
}

public enum eMOVEMENTTYPE
{
  NONE = 0,
  SLIDE = 1
}

[Serializable, SerializeField]
public class SerializableSprite
{
  [SerializeField]
  public Sprite sprite = null;

  [SerializeField]
  public int x;
  [SerializeField]
  public int y;
  [SerializeField]
  public byte[] bytes;

  public JSON ToJSON()
  {
    JSON json = new JSON();
    json.Add("x", x);
    json.Add("y", y);
    json.Add("data", bytes);
    return json;
  }

  public void FromJSON(JSON data)
  {
    x = data.GetInt("x");
    y = data.GetInt("y");
    bytes = data.GetJArray("x").AsByteArray();
    Texture2D texture = new Texture2D(x, y);
    
  }

}

[Serializable, SerializeField]
public class CharacterData
{
  public CharacterData(string newName, string newDescription = "")
  {
    name = newName;
    profile = null;
    emotions = new Dictionary<eMOOD, List<SerializableSprite>>();
  }

  public CharacterData(JSON data)
  {
    FromJSON(data);
  }


  public JSON ToJSON()
  {
    JSON toJSON = new JSON();

    toJSON.Add("name", name);
    toJSON.Add("profile", profile.sprite.texture.GetRawTextureData());
    toJSON.Add("emotions", new JSON());
    if (emotions != null)
    {
      foreach (var emotion in emotions)
      {
        List<byte[]> spritebytes = new List<byte[]>();
        foreach(var sprite in emotion.Value)
        {
          spritebytes.Add(sprite.sprite.texture.GetRawTextureData());
        }
        toJSON.GetJSON("emotions").Add(emotion.Key.ToString(), spritebytes.ToArray());
      }
    }


    return toJSON;
  }

  public void FromJSON(JSON data)
  {
    // name = data.GetString("name");
    // Texture2D t = new Texture2D(0, 0);
    // t.LoadRawTextureData();
    // profile.texture.LoadRawTextureData(data.GetJArray("profile").AsByteArray());
    // emotions = new Dictionary<eMOOD, List<Sprite>>();
    // 
    // var emotionsJSON = data.GetJSON("emotions");
    // 
    // foreach(var emotion in emotionsJSON.Keys)
    // {
    //   var arr = emotionsJSON.GetJArray(emotion);
    // 
    //   foreach(var bytearray in arr.Values)
    //   {
    //     emotions.Add((eMOOD)Enum.Parse(typeof(eMOOD), emotion), );
    //   }
    // 
    // }

  }

  [SerializeField]
  public string name;
  [SerializeField]
  public SerializableSprite profile;
  [SerializeField]
  public Dictionary<eMOOD, List<SerializableSprite>> emotions;
}

public class CharacterController : MonoBehaviour
{
  public static CharacterController instance;

  public GameObject leftPivot;

  public GameObject rightPivot;

  public GameObject centerPivot;

  public List<GameObject> customPivots;

  public GameObject characterPrefab;

  [SerializeField]
  public Dictionary<CharacterData, GameObject> characters;

  public List<GameObject> characterFramePool;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  void LoadData()
  {
    Resources.Load("characters");
  }

  public void
  AddCharacter(string name, eSCREENSIDE side, eMOVEMENTTYPE movementtype)
  {
    
  }


}
