using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUtilities : MonoBehaviour
{
  public static GameUtilities instance;

  private IEnumerator blackFadeCoroutine = null;

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
    
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void CloseGame()
  {
    Application.Quit();
  }


  public void setVolume()
  {

  }


  public void FadeIn(float seconds)
  {
    FadeBlackBackground(true, seconds);
  }

  public void FadeOut(float seconds)
  {
    FadeBlackBackground(false, seconds);
  }

  public void FadeBlackBackground(bool toSolid, float seconds)
  {
    var blackBackground = GameObject.Find("InBetween").GetComponent<Image>();
    if (blackFadeCoroutine == null )
    {
      Debug.Log("Fading to " + (toSolid ? "black" : "transparent"));
      blackFadeCoroutine = Fade(blackBackground, toSolid, seconds);
      StartCoroutine(blackFadeCoroutine);
    }
  }

  public IEnumerator
  Fade(Image image, bool toSolid, float seconds)
  {

    for (float actualTime = 0.0f; actualTime < seconds; actualTime += Time.deltaTime)
    {
      image.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(toSolid ? 0.0f : 1.0f, toSolid ? 1.0f : 0.0f, actualTime));
      yield return null;  
    }

    yield return new WaitForEndOfFrame();
    blackFadeCoroutine = null;
    yield return new WaitForEndOfFrame();
  }

}
