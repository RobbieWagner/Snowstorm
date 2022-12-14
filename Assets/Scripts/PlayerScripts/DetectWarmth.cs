using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWarmth : MonoBehaviour
{
    // Class attached to the player
    // Looks for sources of warmth

    [SerializeField]
    private ColdMeter coldMeter;

    [HideInInspector]
    public bool depleting;
    bool runningDepletionFunction;
    [HideInInspector]
    public bool replenishing;
    bool runningReplenishFunction;

    public float depletionRate;
    public float replenishmentRate;

    void Start()
    {
        depleting = true;
        replenishing = false;

        runningDepletionFunction = false;
        runningReplenishFunction = false;
    }

    void Update()
    {
        if(depleting && !runningDepletionFunction)
        {
            runningDepletionFunction = true;
            StartCoroutine(RunDepletion(depletionRate));
        }
        else if(replenishing && !runningReplenishFunction)
        {
            runningReplenishFunction = true;
            StartCoroutine(RunReplenishment(replenishmentRate));
        }
    }

    //When standing near source of warmth, warm player
    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("Warmth"))
        {
            depleting = false;
            replenishing = true;
        }
    }

    //When going away from a source of warmth, stop warming player
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.CompareTag("Warmth"))
        {
            depleting = true;
            replenishing = false;
        }
    }

    //Change cold meter values accordingly
    IEnumerator RunDepletion(float rate)
    {
        yield return StartCoroutine(coldMeter.DepleteMeter(rate));
        runningDepletionFunction = false;
        StopCoroutine(RunDepletion(rate));
    }

    IEnumerator RunReplenishment(float rate)
    {
        yield return StartCoroutine(coldMeter.ReplenishMeter(rate));
        runningReplenishFunction = false;
        StopCoroutine(RunReplenishment(rate));
    }
}