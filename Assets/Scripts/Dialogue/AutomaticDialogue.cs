using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDialogue : Interactable
{

    [SerializeField]
    private TextAsset[] textJSON;

    DialogueInteractable dialogueI;

    DialogueManager dialogueM;
    [SerializeField]
    List<DialogueInteractable.Dialogue> dialogues;

    private bool hasInteracted;

    [SerializeField]
    private bool dialogueOnStart;

    [SerializeField]
    private Collider[] triggers;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        playerCanInteract = false;

        for(int i = 0; i < textJSON.Length; i++)
        {
            dialogues.Add(JsonUtility.FromJson<DialogueInteractable.DialogueD>(textJSON[i].text).dialogue);
        }

        dialogueM = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();

        hasInteracted = false;


    }

    protected override void OnTriggerEnter(Collider collision)
    {
        if(!hasInteracted)
        {
            hasInteracted = true;
            Interact();
            foreach(Collider collider in triggers)
            {
                collider.enabled = false;
            }
        }
    }

    protected override void OnTriggerExit(Collider collision)
    {

    }

    protected override void Interact()
    {
        base.Interact();
        StartCoroutine(ReadDialogueList());
    }

    IEnumerator ReadDialogueList()
    {
        foreach(DialogueInteractable.Dialogue dialogue in dialogues)
        {
            Debug.Log("New Dialogue Started");
            dialogueM.StartDialogue(dialogue);
            yield return new WaitForSeconds(.25f);
            while(player.isReadingDialogue)
            {
                yield return null;
            }
            yield return new WaitForSeconds(.5f);
        }

        StopCoroutine(ReadDialogueList());
    }

    protected override void Update()
    {
    }
}
