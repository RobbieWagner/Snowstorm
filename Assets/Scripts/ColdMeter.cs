using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdMeter : MonoBehaviour
{
    // Class handling the cold meter. Main functionality of the game

    [SerializeField]
    private float meterMax;
    public float depletionRate;
    public float replenishingRate;
    [SerializeField]
    private float currentMeterReading;
    
    [SerializeField]
    private Slider meter;

    [HideInInspector]
    public bool depleting;
    [HideInInspector]
    public bool replenishing;

    [SerializeField]
    private SceneChanger sceneChanger;

    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Movement playerMovement;

    bool transitioningScene;

    void Start()
    {
        transitioningScene = false;
        depleting = false;
        replenishing = false;

        meter.minValue = 0;
        meter.maxValue = meterMax;
        meter.value = currentMeterReading;
    }

    void Update()
    {
        if(currentMeterReading <= 0 && !transitioningScene)
        {
            transitioningScene = true;
            playerMovement.canMove = false;
            playerAnimator.SetBool("Dying", true);
            sceneChanger.RunTransitionScene("MainMenu");
        }
        else if(currentMeterReading > meterMax) currentMeterReading = meterMax;

        meter.value = currentMeterReading;
    }

    // Lowers the current reading on the meter
    public IEnumerator DepleteMeter()
    {
        if(!depleting && !replenishing)
        {
            depleting = true;
            currentMeterReading -= depletionRate;
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
            yield return new WaitForSeconds(1f);
        }
        depleting = false;
        StopCoroutine(DepleteMeter());
    }

    // Raises the reading on the meter
    public IEnumerator ReplenishMeter()
    {
        if(!depleting && !replenishing)
        {
            replenishing = true;
            currentMeterReading += replenishingRate;
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
            yield return new WaitForSeconds(1f);
        }
        replenishing = false;
        StopCoroutine(ReplenishMeter());
    }
}
