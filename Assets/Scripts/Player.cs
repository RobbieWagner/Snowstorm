using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canHide;
    public bool reading;
    public bool firstTimeInLocker;
    public bool firstButtonDoor;

    [SerializeField]
    GameObject playerGraphic;

    void Start()
    {
        canHide = true;
        reading = false;
        firstTimeInLocker = true;
        firstButtonDoor = true;

        playerGraphic.SetActive(false);
    }
}
