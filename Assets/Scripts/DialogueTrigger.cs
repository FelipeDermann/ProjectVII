using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueBeforeShop;
    public Dialogue dialogueAfterShop;

    public void TriggerDialogue(Dialogue _dialogueToTrigger)
    {
        DialogueManager.Instance.StartDialogue(_dialogueToTrigger);
    }
}
