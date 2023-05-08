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

  public SerializableSprite(JSON json)
  {
    FromJSON(json);
  }

  public SerializableSprite() 
  { 
  
  }

  public JSON ToJSON()
  {
    JSON json = new JSON();
    json.Add("x", sprite ? sprite.texture.width : 0);
    json.Add("y", sprite ? sprite.texture.height : 0);
    if (sprite != null)
    {
      json.Add("data", ImageConversion.EncodeToPNG(sprite.texture));
    }
    return json;
  }

  public void FromJSON(JSON data)
  {
    int x = data.GetInt("x");
    int y = data.GetInt("y");
    byte[] bytes = data.GetJArray("data").AsByteArray();
    Texture2D texture = new Texture2D(x, y);
    ImageConversion.LoadImage(texture, bytes);
    sprite = Sprite.Create(texture, new Rect(0, 0, x, y), Vector2.one);
  }

}

[Serializable, SerializeField]
public class CharacterData
{
  public CharacterData(string newName, string newDescription = "")
  {
    name = newName;
    profile = new SerializableSprite();
    emotions = new List<Tuple<eMOOD, SerializableSprite>>();
    FillDictionary();
  }

  public CharacterData(JSON data)
  {
    FromJSON(data);
  }

  public void FillDictionary()
  {
    if (null == emotions)
    {
      Debug.LogWarning("The emotions list is not initialized. Did you load incorrectly?");
      return;
    }

    foreach(var mood in (eMOOD[]) Enum.GetValues(typeof(eMOOD)))
    {
      if (null == emotions.Find(x => x.Item1 == mood))
      {
        emotions.Add(new Tuple<eMOOD, SerializableSprite>(mood, new SerializableSprite()));
      }
    }
  }

  public JSON ToJSON()
  {
    JSON toJSON = new JSON();

    toJSON.Add("name", name);
    toJSON.Add("profile", profile.ToJSON());
    toJSON.Add("emotions", new JSON());
    if (emotions != null)
    {
      foreach (var emotion in emotions)
      {
        JArray spriteArray = new JArray();
        
        // foreach(var sprite in emotion.Item2)
        // {
        //   spriteArray.Add(sprite.ToJSON());
        // }
        toJSON.GetJSON("emotions").Add(emotion.ToString(), emotion.Item2.ToJSON());
      }
    }


    return toJSON;
  }

  public void FromJSON(JSON data)
  {
    name = data.GetString("name");
    profile = new SerializableSprite();
    profile.FromJSON(data.GetJSON("profile"));

    emotions = new List<Tuple<eMOOD, SerializableSprite>>();

    foreach(var emotionJSON in data.GetJSON("emotions").Keys)
    {
      eMOOD emotionEnum = (eMOOD)Enum.Parse(typeof(eMOOD), emotionJSON);
      List<SerializableSprite> sprites = new List<SerializableSprite>();

      JSON currentEmotion = data.GetJSON("emotions").GetJSON(emotionJSON);

      // foreach(var spriteData in currentEmotion.Values)
      // {
      //   SerializableSprite sprite = new SerializableSprite();
      //   sprite.FromJSON(((JSON)spriteData));
      //   sprites.Add(sprite);
      // }

      emotions.Add(new Tuple<eMOOD, SerializableSprite>(emotionEnum, new SerializableSprite(currentEmotion)));
    }  
  }

  [SerializeField]
  public string name;
  [SerializeField]
  public SerializableSprite profile = null;
  [SerializeField]
  public List<Tuple<eMOOD, SerializableSprite>> emotions;
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
