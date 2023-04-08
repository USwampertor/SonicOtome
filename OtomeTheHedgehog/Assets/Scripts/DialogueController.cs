using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Dialogue
{
  public string name;
  public string description;
  public eMOOD mood;

  public List<Dialogue> options;
}


public class DialogueController : MonoBehaviour
{
  public static DialogueController instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this; 
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Debug.LogWarning("Trying to create previously created module");
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
}
