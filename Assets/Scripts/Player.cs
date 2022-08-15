using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool reading;

    public bool hasSeenFireLightingTutorial;
    public bool hasSeenWarmthTutorial;

    public bool hasSeenInteractableTutorial;

    [SerializeField]
    GameObject playerGraphic;

    void Start()
    {
        reading = false;

        hasSeenFireLightingTutorial = false;
        hasSeenWarmthTutorial = false;

        hasSeenInteractableTutorial = false;
    }
}
