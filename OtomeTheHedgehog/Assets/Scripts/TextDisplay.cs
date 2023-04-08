using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
  [Tooltip("How much miliseconds per letter")]
  public float defaultSpeed = 5;

  private Dialogue currentDialog;
  private List<string> textList = new List<string>();
  private int maxCharacters;
  private int dialogSplits;
  private int currentSplit;
  private IEnumerator displayCoroutine = null;

  public TMPro.TextMeshProUGUI textDisplay;

  private void Awake()
  {
    string loremIpsum = "Lorem ipsum dolor sit amet, " +
                        "consectetur adipiscing elit, " +
                        "sed do eiusmod tempor incididunt" +
                        "ut labore et dolore magna aliqua. " +
                        "Ut enim ad minim veniam, quis " +
                        "nostrud exercitation ullamco laboris " +
                        "nisi ut aliquip ex ea commodo consequat. ";
    maxCharacters = loremIpsum.Length;
  }


  // Start is called before the first frame update
  void Start()
  {
    // Testing Data
    currentDialog = new Dialogue();
    currentDialog.name = "test";
    currentDialog.description = "Lorem ipsum dolor sit amet, " +
                                "consectetur adipiscing elit, " +
                                "sed do eiusmod tempor incididunt" +
                                "ut labore et dolore magna aliqua. " +
                                "Ut enim ad minim veniam, quis "+
                                "nostrud exercitation ullamco laboris "+
                                "nisi ut aliquip ex ea commodo consequat. " +
                                "Duis aute irure dolor in reprehenderit " +
                                "in voluptate velit esse cillum dolore " +
                                "eu fugiat nulla pariatur. Excepteur " +
                                "sint occaecat cupidatat non " +
                                "proident, sunt in culpa qui " +
                                "officia deserunt mollit anim " +
                                "id est laborum.";

    SplitDialogue();
    textDisplay.text = "";
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  //TODO: Make this split into complete words and not just characters
  void CheckSplits()
  {
    int totalDialogSize = currentDialog.description.Length;
    float floatSplit = (float)totalDialogSize / (float)maxCharacters;
    Debug.Log(Mathf.RoundToInt(floatSplit));
    dialogSplits = Mathf.RoundToInt(floatSplit);

  }
  void SplitDialogue()
  {
    textList.Clear();

    CheckSplits();
    int startIndex = 0;
    int chunkSize = currentDialog.description.Length / dialogSplits;
    for (int i = 0; i < dialogSplits; i++)
    {
      textList.Add(
        currentDialog.description.Substring(startIndex, 
                                            startIndex + chunkSize < 
                                            currentDialog.description.Length ? 
                                            chunkSize : 
                                            currentDialog.description.Length - startIndex));
      startIndex += chunkSize;
    }
  }

  public void Continue()
  {
    // We are still throwing text but user clicked as the desesperate prick he is
    textDisplay.text = "";
    if (displayCoroutine != null)
    {
      ++currentSplit;
      StopCoroutine(displayCoroutine);
    }

    // Are we still spliting the same dialogue?
    if (currentSplit < dialogSplits)
    {
      displayCoroutine = Display();
      StartCoroutine(displayCoroutine);
    }

    // Should continue to the next if any
  }


  public IEnumerator
  Display()
  {
    yield return new WaitForSeconds(1);

    int charIndex = 0;
    while (textDisplay.text != textList[currentSplit]) {
      textDisplay.text += textList[currentSplit][charIndex];
      ++charIndex;
      yield return new WaitForSeconds(Time.deltaTime * defaultSpeed);
    }
    displayCoroutine = null;
    ++currentSplit;
    yield return null;
  }

}
