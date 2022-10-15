using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Class keeps track of variables called by multiple classes
    
    //[HideInInspector]
    //public bool reading;

    [HideInInspector]
    public bool hasSeenFireLightingTutorial;
    [HideInInspector]
    public bool hasSeenWarmthTutorial;

    [HideInInspector]
    public bool hasSeenInteractableTutorial;

    [HideInInspector]
    public bool canInteractWithObjects;

    [SerializeField]
    GameObject playerGraphic;

    public int tilesGenerated;

    [HideInInspector]
    public bool playerIsInside;

    [HideInInspector]
    public bool playerHasEneteredCabin;
    [HideInInspector]
    public bool playerhasEnteredTown;

    [SerializeField]
    public GameObject k;
    [SerializeField]
    public GameObject j;

    void Start()
    {
        tilesGenerated = 0;
        

        hasSeenFireLightingTutorial = false;
        hasSeenWarmthTutorial = false;

        hasSeenInteractableTutorial = false;

        playerIsInside = false;
        playerHasEneteredCabin = false;
        playerhasEnteredTown = false;

        k.SetActive(false);
    }
}
