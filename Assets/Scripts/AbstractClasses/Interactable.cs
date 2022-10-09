using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to handle interactable objects
public abstract class Interactable : MonoBehaviour
{
    protected GameObject keyboardKey;
    protected Player player;
    protected bool playerCanInteract;

    protected void Start() 
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        keyboardKey = FindObject(player.gameObject, "K");
        playerCanInteract = false;
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {   
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            playerCanInteract = true;
            keyboardKey.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        { 
            playerCanInteract = false;
            keyboardKey.SetActive(false);
        }
    }

    protected virtual void OnGUI()
    {
        if(playerCanInteract && Input.GetKeyDown(KeyCode.K))
        Interact();
    }

    //Find a child object of a parent
    protected virtual GameObject FindObject(GameObject parent, string name)
    {
        Transform[] children= parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in children){
            if(child.name.Equals(name)){
                return child.gameObject;
            }
        }
        return null;
    }

    protected abstract void Interact();
}

