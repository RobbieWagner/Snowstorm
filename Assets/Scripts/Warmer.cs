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

    void OnTriggerStay(Collider collision)
    {
        Debug.Log("hi");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("flipped");
            playerWarmer.depleting = false;
            playerWarmer.replenishing = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerWarmer.depleting = true;
            playerWarmer.replenishing = false;
        }
    }
}
