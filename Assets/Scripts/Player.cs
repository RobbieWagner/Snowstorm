using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    void Start()
    {
        tilesGenerated = 0;
        reading = false;

        hasSeenFireLightingTutorial = false;
        hasSeenWarmthTutorial = false;

        hasSeenInteractableTutorial = false;
    }
}
