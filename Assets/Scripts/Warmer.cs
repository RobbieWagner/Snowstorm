using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmer : MonoBehaviour
{
    public float warmRate;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Warmth");
    }
}
