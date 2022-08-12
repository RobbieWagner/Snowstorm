using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWarmth : MonoBehaviour
{
    [SerializeField]
    private ColdMeter coldMeter;

    bool depleting;
    bool runningDepletionFunction;
    bool replenishing;
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

    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Warmth"))
        {
            depleting = false;
            replenishing = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Warmth"))
        {
            depleting = true;
            replenishing = false;
        }
    }

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