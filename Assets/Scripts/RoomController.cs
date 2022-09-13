using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [HideInInspector]
    public bool canEnter;
    private bool isAtDoor;

    private bool isRoomOn;

    [SerializeField]
    private GameObject cabinInterior;

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

    void Start()
    {
        canEnter = true;
        isAtDoor = false;

        playerT = GameObject.Find("Player").GetComponent<Transform>();
        playerMovement = playerT.gameObject.GetComponent<Movement>();
        playerDW = playerT.gameObject.GetComponent<DetectWarmth>();

        isUsingDoor = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
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
        if(isAtDoor && canEnter && Input.GetKeyDown(KeyCode.K) && !isUsingDoor)
        {
            if(isRoomOn) ExitRoom();
            else EnterRoom();
            StartCoroutine(CoolDownDoorUsage());
        }
    }

    void EnterRoom()
    {
        cabinInterior.SetActive(true);
        isRoomOn = true;
        playerMovement.MoveCharacter(enterRoomT.position);
        Debug.Log(playerT.position.ToString());

        playerDW.replenishing = true;
        playerDW.depleting = false;
    }

    void ExitRoom()
    {
        cabinInterior.SetActive(false);
        isRoomOn = false;
        playerMovement.MoveCharacter(exitRoomT.position);
        Debug.Log(playerT.position.ToString());

        playerDW.replenishing = false;
        playerDW.depleting = true;
    }

    IEnumerator CoolDownDoorUsage()
    {
        isUsingDoor = true;
        yield return new WaitForSeconds(.5f);
        isUsingDoor = false;

        Debug.Log(playerT.position.ToString());
        StopCoroutine(CoolDownDoorUsage());
    }
}
