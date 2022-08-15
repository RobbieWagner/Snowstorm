using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnock : MonoBehaviour
{
    [SerializeField]
    private bool canGoIn = false;
    private bool isAtDoor;

    private Player player;

    Canvas interactableTutorial;

    [SerializeField]
    private AudioSource knockingSound;

    void Start()
    {
        canGoIn = false;
        interactableTutorial = GameObject.Find("InteractableTutorial").GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            if(!player.hasSeenInteractableTutorial)
            {
                player.hasSeenInteractableTutorial = true;
                StartCoroutine(TimeTutorialDisplay(interactableTutorial));
            }

            isAtDoor = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            isAtDoor = false;
        }
    }

    void OnGUI()
    {
        if(isAtDoor && Input.GetKeyDown(KeyCode.K))
        {
            if(!canGoIn) knockingSound.Play();
        }
    }

    void ToggleCanvas(Canvas tutorialCanvas)
    {
        if(!tutorialCanvas.isActiveAndEnabled) tutorialCanvas.enabled = true;
        else tutorialCanvas.enabled = false;
    }

    IEnumerator TimeTutorialDisplay(Canvas tutorialCanvas)
    {
        ToggleCanvas(tutorialCanvas);
        yield return new WaitForSeconds(5f);
        ToggleCanvas(tutorialCanvas);

        StopCoroutine(TimeTutorialDisplay(tutorialCanvas));
    }
}