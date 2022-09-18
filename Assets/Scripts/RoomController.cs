using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : Interactable
{
    [HideInInspector]
    public bool canEnter;
    private bool isAtDoor;

    private bool isRoomOn;

    [SerializeField]
    private GameObject interior;

    private Transform playerT;
    private Movement playerMovement;

    [SerializeField]
    private Transform enterRoomT;
    [SerializeField]
    private Transform exitRoomT;

    private bool isUsingDoor;

    [SerializeField]
    private bool roomWarms;

    private DetectWarmth playerDW;

    [SerializeField]
    private DoorKnock doorKnock;

    protected void Start()
    {
        base.Start();

        if(doorKnock == null)
        canEnter = true;
        isAtDoor = false;

        playerT = GameObject.Find("Player").GetComponent<Transform>();
        playerMovement = playerT.gameObject.GetComponent<Movement>();
        playerDW = playerT.gameObject.GetComponent<DetectWarmth>();

        interior.SetActive(false);

        isUsingDoor = false;
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            isAtDoor = true;
        }
    }

    protected override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            isAtDoor = false;
        }
    }

    protected override void OnGUI()
    {
        if(isAtDoor && canEnter && Input.GetKeyDown(KeyCode.K) && !isUsingDoor)
        {
            if(isRoomOn) ExitRoom();
            else EnterRoom();
            StartCoroutine(CoolDownDoorUsage());
        }
    }

    void EnterRoom()
    {
        interior.SetActive(true);
        isRoomOn = true;
        playerMovement.MoveCharacter(enterRoomT.position);

        if(roomWarms)
        {
            playerDW.replenishing = true;
            playerDW.depleting = false;
        }

        if(doorKnock != null)
        {
            if(doorKnock.interactableTutorial != null)
            StartCoroutine(doorKnock.TimeTutorialDisplay(doorKnock.interactableTutorial));
        }
    }

    void ExitRoom()
    {
        interior.SetActive(false);
        isRoomOn = false;
        playerMovement.MoveCharacter(exitRoomT.position);

        playerDW.replenishing = false;
        playerDW.depleting = true;
    }

    IEnumerator CoolDownDoorUsage()
    {
        isUsingDoor = true;
        yield return new WaitForSeconds(.5f);
        isUsingDoor = false;

        StopCoroutine(CoolDownDoorUsage());
    }
}
