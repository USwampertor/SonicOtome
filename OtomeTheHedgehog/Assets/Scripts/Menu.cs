using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
  // Start is called before the first frame update
  void AutoRegister()
  {
    if (MenuController.instance != null)
    {
      MenuController.instance.RegisterMenu(this.gameObject);
    }
  }
  
  void Start()
  {
    AutoRegister();
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
