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
    private Vector3 enterRoomPositionOffset;
    [SerializeField]
    private Vector3 exitRoomPositionOffset;

    private bool isUsingDoor;

    void Start()
    {
        canEnter = true;
        isAtDoor = false;

        playerT = GameObject.Find("Player").GetComponent<Transform>();
        playerMovement = playerT.gameObject.GetComponent<Movement>();

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
        playerMovement.MoveCharacter(playerT.position + enterRoomPositionOffset);
        Debug.Log(playerT.position.ToString());
        Debug.Log("enter room");
    }

    void ExitRoom()
    {
        cabinInterior.SetActive(false);
        isRoomOn = false;
        playerMovement.MoveCharacter(playerT.position + exitRoomPositionOffset);
        Debug.Log(playerT.position.ToString());
        Debug.Log("exit room");
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
