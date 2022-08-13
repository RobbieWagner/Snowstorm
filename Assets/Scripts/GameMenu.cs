using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas gameMenu;

    bool menuOpen;
    bool menuChanging;

    [SerializeField]
    private ColdMeter coldMeter;

    [SerializeField]
    private Movement playerMovement;

    void Start()
    {
        menuOpen = false;
        menuChanging = false;
        gameMenu.enabled = false;
    }

    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!menuOpen && !menuChanging)
            { 
                menuChanging = true;
                gameMenu.enabled = true;
                coldMeter.depleting = false;
                coldMeter.replenishing = false;
                menuOpen = true;
                playerMovement.canMove = false;
            }
            else if(menuOpen && !menuChanging) 
            {
                menuChanging = true;
                gameMenu.enabled = false;
                coldMeter.depleting = true;
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
