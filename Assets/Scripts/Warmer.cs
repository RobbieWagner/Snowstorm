using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmer : MonoBehaviour
{
    public float warmRate;

    [SerializeField]
    private DetectWarmth playerWarmer;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Warmth");
        playerWarmer = GameObject.Find("Player").GetComponent<DetectWarmth>();
    }
}
