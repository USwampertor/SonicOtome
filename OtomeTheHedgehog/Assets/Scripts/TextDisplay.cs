using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
  [Tooltip("How much miliseconds per letter")]
  public float defaultSpeed = 5;

  private Dialog currentDialog;
  private int maxCharacters;
  private int dialogSplits;

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
    currentDialog = new Dialog();
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

  }

  // Update is called once per frame
  void Update()
  {
        
  }

  void checkSplits()
  {
    int totalDialogSize = currentDialog.description.Length;
    dialogSplits = (int)Mathf.Ceil(totalDialogSize / maxCharacters);

  }


  public IEnumerator
  Display(int currentSplit)
  {



    yield return null;
  }

}
