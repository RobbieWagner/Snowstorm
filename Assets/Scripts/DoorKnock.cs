using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnock : MonoBehaviour
{
    [SerializeField]
    private bool canEnter = false;
    private bool isAtDoor;

    private Player player;

    [HideInInspector]
    public Canvas interactableTutorial;

    [SerializeField]
    private AudioSource knockingSound;

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private RoomController cabinDoor;

    void Start()
    {
        interactableTutorial = GameObject.Find("InteractableTutorial").GetComponent<Canvas>();
        player = GameObject.Find("Player").GetComponent<Player>();

        cabinDoor.canEnter = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            if(!player.hasSeenInteractableTutorial)
            {
                player.hasSeenInteractableTutorial = true;
                ToggleCanvas(interactableTutorial);
            }

            isAtDoor = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            isAtDoor = false;
            StartCoroutine(TimeTutorialDisplay(interactableTutorial));
        }
    }

    void OnGUI()
    {
        if(isAtDoor && Input.GetKeyDown(KeyCode.K))
        {
            if(!cabinDoor.canEnter)
            { 
                knockingSound.Play();
                StartCoroutine(OpenDoor());
            }
        }
    }

    void ToggleCanvas(Canvas tutorialCanvas)
    {
        if(!tutorialCanvas.isActiveAndEnabled) tutorialCanvas.enabled = true;
        else tutorialCanvas.enabled = false;
    }

    public IEnumerator TimeTutorialDisplay(Canvas tutorialCanvas)
    {
        yield return new WaitForSeconds(5f);
        ToggleCanvas(tutorialCanvas);

        StopCoroutine(TimeTutorialDisplay(tutorialCanvas));
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2f);
        cabinDoor.canEnter = true;
        door.GetComponent<SpriteRenderer>().enabled = false;
        StopCoroutine(OpenDoor());
    }
}