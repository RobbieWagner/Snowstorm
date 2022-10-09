using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject playButton, mainMenuButton;

    [SerializeField]
    private Canvas mainMenuCanvas;

    private bool currentlySelected;

    void Start()
    {
        Time.timeScale = 1.0f;

        currentlySelected = false;
    }

    private void Update() 
    {
        if(mainMenuCanvas.enabled && !currentlySelected)
        SetupEventSystem();
        else if(!mainMenuCanvas.enabled && currentlySelected)
        SetupControlsMenu();
    }

    void SetupEventSystem()
    {
        currentlySelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    void SetupControlsMenu()
    {
        currentlySelected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }
}
