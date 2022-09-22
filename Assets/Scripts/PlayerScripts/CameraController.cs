using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float tilt;

    void Start()
    {
        transform.rotation = Quaternion.Euler(tilt, 0, 0);
    }
}

