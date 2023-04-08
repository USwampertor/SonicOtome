using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
  public static MenuController instance;

  public List<GameObject> registeredMenus;

  public Stack<GameObject> menuStack = new Stack<GameObject>();

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Debug.LogWarning("Trying to recreate MenuController. Destroying object");
      Destroy(gameObject);
    }
  }


  // Start is called before the first frame update
  void Start()
  {
    foreach (var menu in registeredMenus)
    {
      menu.SetActive(false);
    }

    OpenMenu("MainMenuContainer");
    OpenMenu("MainItemsContainer");
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void RegisterMenu(GameObject newContainer)
  {
    if (registeredMenus.Find(x => x == newContainer) == null)
    { 
      registeredMenus.Add(newContainer);
    }
  }

  public void OpenMenu(string menuName)
  {
    if (menuStack.Count > 1)
    {
      menuStack.Peek().SetActive(false);
    }
    var newMenu = registeredMenus.Find(x => x.name == menuName);
    if (newMenu != null)
    {
      menuStack.Push(newMenu);
      menuStack.Peek().SetActive(true);
    }
  }

  public void CloseMenu(bool transition)
  {
    if (menuStack.Count > 1)
    {
      menuStack.Peek().SetActive(false);
      menuStack.Pop();
      if (transition) 
      {
        GameUtilities.instance.FadeIn(3);
      }
    }
  }

  public void ChangeScene()
  {

  }


  public IEnumerator ChangeSceneCoroutine(string sceneName)
  {
    GameUtilities.instance.FadeIn(5);
    yield return new WaitForSeconds(6);

    OpenMenu(sceneName);

    GameUtilities.instance.FadeOut(5);
    yield return new WaitForSeconds(6);

    yield return null;
  }

}
