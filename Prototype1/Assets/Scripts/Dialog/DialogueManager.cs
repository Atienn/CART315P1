using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    string[] lines;
    short index;

    Text titleBox;
    Text dialogBox;
    Animator transition;

    // Update is called once per frame
    void StartDialogue(Dialogue data) {
        titleBox.text = data.title;
        lines = data.lines;
        index = 0;


        NextLine();
    }

    void NextLine() {
        if (index < lines.Length) {
            dialogBox.text = lines[index++];
        }
        else {
            EndDialogue();
        }
    }

    void EndDialogue() {

    }
}
