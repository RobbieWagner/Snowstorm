using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

    DialogueManager dialogueM;

    
    [SerializeField]
    private GameObject[] notifIcons;
    [SerializeField]
    private Journal gameJournal;

    private bool canLeaveMenu;

    [SerializeField]
    private AudioSource scribblingSound; 

    [SerializeField]
    private GameObject journalButton;

    void Start()
    {
        menuOpen = false;
        menuChanging = false;
        gameMenu.enabled = false;

        player = playerMovement.gameObject.GetComponent<Player>();

        canLeaveMenu = true;

        dialogueM = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();
    }

    //For opening and closing the in game menu
    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!menuOpen && !menuChanging && !dialogueM.dialogueRunning)
            { 
                Time.timeScale = 0f;
                menuChanging = true;
                gameMenu.enabled = true;
                menuOpen = true;
                playerMovement.canMove = false;
                EventSystem.current.SetSelectedGameObject(journalButton);
                player.canInteractWithObjects = false;
            }
            else if(menuOpen && gameMenu.enabled && !menuChanging && canLeaveMenu) 
            {
                Time.timeScale = 1.0f;
                menuChanging = true;
                gameMenu.enabled = false;
                menuOpen = false;
                playerMovement.canMove = true;
                EventSystem.current.SetSelectedGameObject(null);
                player.canInteractWithObjects = true;
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

    //Toggles notifcation Icons
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
