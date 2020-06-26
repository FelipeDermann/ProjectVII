using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public static event Action DialogueEnd;
    public static event Action DialogueStart;

    [Header("Delay (in seconds) for each chracter on sentence to be drawn")]
    public float characterDrawingDelay;

    public Image npcImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator anim;

    public bool isTalking;
    public bool isWritingSentence;

    private Queue<string> sentences;
    string currentSentence;

    Dialogue currentDialogue;
    bool canEnd;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        DialogueBox.DialogueStart += WriteDialogue;
        DialogueBox.DialogueEnd += End;
    }

    private void OnDestroy()
    {
        DialogueBox.DialogueStart -= WriteDialogue;
        DialogueBox.DialogueEnd -= End;

    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if((Input.GetButtonDown("Interact") || Input.GetButtonDown("light")) && isTalking)
        {
            if (isWritingSentence)
            {
                StopAllCoroutines();
                isWritingSentence = false;
                dialogueText.text = currentSentence;
            }
            else
            {
                if (sentences.Count > 0) DisplayNextSentence();
                else EndDialogue();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        canEnd = true;
        currentDialogue = dialogue;

        DialogueStart?.Invoke();

        sentences.Clear();

        nameText.text = currentDialogue.npcName;
        npcImage.sprite = currentDialogue.npcImage;
        dialogueText.text = "";

        anim.SetTrigger("Open");

    }

    public void WriteDialogue()
    {
        Invoke(nameof(TimeToAllowSkipping), 0.1f);

        foreach (string sentence in currentDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void TimeToAllowSkipping()
    {
        isTalking = true;
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isWritingSentence = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(characterDrawingDelay);
        }

        isWritingSentence = false;
    }

    public void EndDialogue()
    {
        if (!canEnd) return;
        canEnd = false;

        anim.SetTrigger("Close");
    }

    void End()
    {
        isTalking = false;
        DialogueEnd?.Invoke();
    }
}
