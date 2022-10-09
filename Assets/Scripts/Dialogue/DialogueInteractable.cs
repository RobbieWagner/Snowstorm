using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : Interactable
{

    [SerializeField]
    private TextAsset textJSON;

    //Dialogue class stores a list of sentences and possible choices
    [System.Serializable]
    public class Dialogue
    {
        public int dialogueID;
        public Sentence[] sentences;
        public StrongChoice[] strongChoice;
    }

    //Sentence class holds properties for the person speaking, text, and choices
    [System.Serializable]
    public class Sentence
    {
        public int textID;
        public string text;
        public WeakChoice[] weakChoice;
        public string speaker;
        public Impact[] gameImpact;
    }

    //WeakChoice class, stores potential outcomes to choices made
    [System.Serializable]
    public class WeakChoice
    {
        public string choiceText;
        public int nextTextID;
    }

    //StrongChoice class, stores potential lasting impacts to choices made
    [System.Serializable]
    public class StrongChoice
    {
        public string choiceText;
        public int nextDialogueID;
        public string lastingImpact;
    }

    //Impact class. Keeps track of possible branching paths.
    [System.Serializable]
    public class Impact
    {
        public string impact;
        public int nextTextID;
    }

    DialogueManager dialogueM;
    Dialogue dialogue;

    // Start is called before the first frame update
    protected void Start()
    {
        playerCanInteract = false;

        dialogue = JsonUtility.FromJson<Dialogue>(textJSON.text);

        dialogueM = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();
    }

    protected override void Interact()
    {
        dialogueM.StartDialogue(dialogue);
    }
}
