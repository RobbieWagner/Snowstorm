using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Class keeps track of variables called by multiple classes
    
    [HideInInspector]
    public bool reading;

    [HideInInspector]
    public bool hasSeenFireLightingTutorial;
    [HideInInspector]
    public bool hasSeenWarmthTutorial;

    [HideInInspector]
    public bool hasSeenInteractableTutorial;

    [SerializeField]
    GameObject playerGraphic;

    public int tilesGenerated;

    [HideInInspector]
    public bool playerIsInside;

    void Start()
    {
        tilesGenerated = 0;
        reading = false;

        hasSeenFireLightingTutorial = false;
        hasSeenWarmthTutorial = false;

        hasSeenInteractableTutorial = false;

        playerIsInside = false;
    }
}
