using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpriteLayering : MonoBehaviour
{
    Transform cameraT;
    Transform playerT;

    bool isObjectBeforePlayer;
    bool isLayerAbovePlayer;

    SpriteRenderer goSR;

    float playerCameraDistance;

    void Start()
    {
        cameraT = Camera.allCameras[0].transform;
        playerT = GameObject.Find("Player").transform;

        goSR = gameObject.GetComponent<SpriteRenderer>();

        playerCameraDistance = Vector3.Distance(cameraT.position, playerT.position);

        isLayerAbovePlayer = false;
    }

    void FixedUpdate()
    {
        //Place sprite on higher layer if in front of player, lower otherwise
        //ISSUE: Calculates every frame. Might become expensive when there are a lot of objects.
        //isObjectBeforePlayer = Vector3.Distance(cameraT.position, transform.position) < playerCameraDistance;

        if(isLayerAbovePlayer && !isObjectBeforePlayer) 
        {
            goSR.sortingOrder = -1;
            isLayerAbovePlayer = false;
        }
        else if(!isLayerAbovePlayer && isObjectBeforePlayer) 
        {
            goSR.sortingOrder = 1;
            isLayerAbovePlayer = true;
        }
    }
}
