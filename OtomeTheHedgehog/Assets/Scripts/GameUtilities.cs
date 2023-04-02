using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUtilities : MonoBehaviour
{

  private Coroutine blackFadeCoroutine = null;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
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
    Debug.Log("Fading to " + (toSolid ? "black" : "transparent"));
    blackFadeCoroutine = StartCoroutine(Fade(blackBackground, toSolid, seconds));
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
  }

}
