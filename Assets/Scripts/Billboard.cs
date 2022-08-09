using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //makes billboarding objects look at the player

    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = camera.transform.rotation;

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}