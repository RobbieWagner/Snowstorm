using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    // Opens and closes the game menu in game

    [SerializeField]
    private Canvas gameMenu;

    bool menuOpen;
    bool menuChanging;

    [SerializeField]
    private Movement playerMovement;
    private Player player;

    
    [SerializeField]
    private GameObject[] notifIcons;
    [SerializeField]
    private Journal gameJournal;

    private bool canLeaveMenu;

    [SerializeField]
    private AudioSource scribblingSound; 

    void Start()
    {
        menuOpen = false;
        menuChanging = false;
        gameMenu.enabled = false;

        player = playerMovement.gameObject.GetComponent<Player>();

        canLeaveMenu = true;
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
            else if(menuOpen && gameMenu.enabled && !menuChanging && canLeaveMenu) 
            {
                Time.timeScale = 1.0f;
                menuChanging = true;
                gameMenu.enabled = false;
                menuOpen = false;
                playerMovement.canMove = true;
            }
            else
            {
                canLeaveMenu = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            menuChanging = false;
            canLeaveMenu = true;
        }
    }

    void Update()
    {
        if(!notifIcons[0].activeSelf && gameJournal.hasUnreadEntries)
        {
            foreach(GameObject notifIcon in notifIcons)
            {
                notifIcon.SetActive(true);
                scribblingSound.Play();
            }
        }
        else if(notifIcons[0].activeSelf && !gameJournal.hasUnreadEntries)
        {
            foreach(GameObject notifIcon in notifIcons)
            {
                notifIcon.SetActive(false);
            }
        }
    }
}
