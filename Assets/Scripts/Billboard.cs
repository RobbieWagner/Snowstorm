using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //makes billboarding objects look at the player

    private Camera camera;

    [SerializeField]
    private bool isPlane;
    
    void Start()
    {
        camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.rotation = camera.transform.rotation;

        if(isPlane)
        {
            transform.rotation = Quaternion.Euler(90f, 0f, -transform.rotation.eulerAngles.y);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}