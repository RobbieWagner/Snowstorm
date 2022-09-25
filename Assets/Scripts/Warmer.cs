using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmer : MonoBehaviour
{
    //sets the layer of a warm object, and find the detectWarmth component on the player
    public float warmRate;

    [SerializeField]
    private DetectWarmth playerWarmer;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Warmth");
        playerWarmer = GameObject.Find("Player").GetComponent<DetectWarmth>();
    }
}
