using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Opens and closes the game menu in game

    [SerializeField]
    private Canvas gameMenu;

    bool menuOpen;
    bool menuChanging;

    [SerializeField]
    private ColdMeter coldMeter;

    [SerializeField]
    private Movement playerMovement;

    bool wasColdMeterDepleting;
    bool wasColdMeterReplenishing;

    void Start()
    {
        menuOpen = false;
        menuChanging = false;
        gameMenu.enabled = false;

        wasColdMeterDepleting = true;
        wasColdMeterReplenishing = true;
    }

    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!menuOpen && !menuChanging)
            { 
                Time.timeScale = 0f;
                menuChanging = true;
                gameMenu.enabled = true;
                menuOpen = true;
                playerMovement.canMove = false;
            }
            else if(menuOpen && !menuChanging) 
            {
                Time.timeScale = 1.0f;
                menuChanging = true;
                gameMenu.enabled = false;
                menuOpen = false;
                playerMovement.canMove = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            menuChanging = false;
        }
    }
}
