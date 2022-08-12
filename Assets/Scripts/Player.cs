using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canHide;
    public bool reading;

    [SerializeField]
    GameObject playerGraphic;

    void Start()
    {
        canHide = true;
        reading = false;
    }
}
