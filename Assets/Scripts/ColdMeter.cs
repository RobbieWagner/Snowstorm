using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdMeter : MonoBehaviour
{

    [SerializeField]
    private float meterMax;
    public float depletionRate;
    public float replenishingRate;
    [SerializeField]
    private float currentMeterReading;
    
    [SerializeField]
    private Slider meter;

    bool depleting;
    bool replenishing;

    [SerializeField]
    private SceneChanger sceneChanger;

    void Start()
    {
        depleting = false;
        replenishing = false;

        meter.minValue = 0;
        meter.maxValue = meterMax;
        meter.value = currentMeterReading;
    }

    void Update()
    {
        if(currentMeterReading <= 0)
        {
            sceneChanger.RunTransitionScene("MainMenu");
        }

        meter.value = currentMeterReading;
    }

    public IEnumerator DepleteMeter()
    {
        if(!depleting && !replenishing)
        {
            depleting = true;
            currentMeterReading -= depletionRate;
            Debug.Log("Depleted " + depletionRate + " units");
            yield return new WaitForSeconds(1f);
        }
        depleting = false;
        StopCoroutine(DepleteMeter());
    }

    public IEnumerator DepleteMeter(float depleteRate)
    {
        if(!depleting && !replenishing)
        {
            depleting = true;
            currentMeterReading -= depleteRate;
            Debug.Log("Depleted " + depleteRate + " units");
            yield return new WaitForSeconds(1f);
        }
        depleting = false;
        StopCoroutine(DepleteMeter());
    }

    public IEnumerator ReplenishMeter()
    {
        if(!depleting && !replenishing)
        {
            replenishing = true;
            currentMeterReading += replenishingRate;
            Debug.Log("Replenished " + replenishingRate + " units");
            yield return new WaitForSeconds(1f);
        }
        replenishing = false;
        StopCoroutine(ReplenishMeter());
    }

    public IEnumerator ReplenishMeter(float replenishRate)
    {
        if(!depleting && !replenishing)
        {
            replenishing = true;
            currentMeterReading += replenishRate;
            Debug.Log("Replenished " + replenishRate + " units");
            yield return new WaitForSeconds(1f);
        }
        replenishing = false;
        StopCoroutine(ReplenishMeter());
    }
}
