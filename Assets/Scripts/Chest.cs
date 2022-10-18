using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{

    [SerializeField]
    private int matchsticksToGive;

    bool openedChest;

    DialogueManager dialogueM;

    DialogueInteractable.Dialogue dialogueBefore;
    [SerializeField]
    TextAsset dbJSON;

    DialogueInteractable.Dialogue dialogueAfter;
    [SerializeField]
    TextAsset daJSON;

    GameMatchsticks gameMatchsticks;

    // Start is called before the first frame update
    void Start()
    {
        dialogueM = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();

        dialogueBefore = JsonUtility.FromJson<DialogueInteractable.DialogueD>(dbJSON.text).dialogue;
        dialogueAfter = JsonUtility.FromJson<DialogueInteractable.DialogueD>(daJSON.text).dialogue;
        
        openedChest = false;

        gameMatchsticks = player.gameObject.GetComponent<GameMatchsticks>();
    }

    protected override void Update()
    {
        base.Update();

        if(player.canInteractWithObjects && playerCanInteract && Input.GetKeyDown(KeyCode.K))
        {
            playerCanInteract = false;
            isInteracting = true;
            Interact();
        }

        if(isInteracting && !runningCooldown)
        {
            StartCoroutine(CoolDownInteraction());
        }

        if(isInteracting && !dialogueM.textBoxC.enabled)
        {
            isInteracting = false;
        }
    }
    
    protected override void Interact()
    {
        if(!openedChest)
        {
            openedChest = true;
            dialogueM.StartDialogue(dialogueBefore);
            gameMatchsticks.matchsticksCount += matchsticksToGive;
        }

        else dialogueM.StartDialogue(dialogueAfter);
       
    }
}
