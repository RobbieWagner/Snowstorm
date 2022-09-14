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
        GameObject interactableTutorialGO = GameObject.Find("InteractableTutorial");
        if(interactableTutorialGO != null)
        interactableTutorial = interactableTutorialGO.GetComponent<Canvas>();
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
                if(interactableTutorial.gameObject != null)
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
            if(interactableTutorial != null)
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
        if(tutorialCanvas != null && !tutorialCanvas.isActiveAndEnabled) tutorialCanvas.enabled = true;
        else if (tutorialCanvas != null) tutorialCanvas.enabled = false;
    }

    public IEnumerator TimeTutorialDisplay(Canvas tutorialCanvas)
    {
        yield return new WaitForSeconds(5f);
        ToggleCanvas(tutorialCanvas);
        if(tutorialCanvas != null)
        GameObject.Destroy(tutorialCanvas.gameObject);

        if(interactableTutorial != null)
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