using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMOOD
{
  NEUTRAL = 0,
  HAPPY = 1,
  SAD = 2,
  SHOCKED = 3,
  ANGRY = 4,
  EXCITED = 5,
  OBVLIVIOUS = 6,
  FLIRTY = 7,
  CONFESSION = 8,
}

public struct Dialog
{
  public string name;
  public string description;
  public eMOOD mood;

  public List<Dialog> options;
}


public class DialogController : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
