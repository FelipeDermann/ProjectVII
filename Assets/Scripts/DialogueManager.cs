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

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if(Input.GetButtonDown("light") && isTalking)
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
        Debug.Log("DIALOGUE IS ON");
        Invoke(nameof(TimeToAllowSkipping), 0.1f);

        anim.SetTrigger("Open");
        DialogueStart?.Invoke();

        nameText.text = dialogue.npcName;
        npcImage.sprite = dialogue.npcImage;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
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
        isTalking = false;
        anim.SetTrigger("Close");
        DialogueEnd?.Invoke();
    }
}
