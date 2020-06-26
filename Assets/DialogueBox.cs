using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueBox : MonoBehaviour
{
    public static event Action DialogueStart;
    public static event Action DialogueEnd;

    public void StartDialogueWriting()
    {
        DialogueStart?.Invoke();
    }

    public void EndDialogue()
    {
        DialogueEnd?.Invoke();
    }
}
