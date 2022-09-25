using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to handle interactable objects
public abstract class Interactable : MonoBehaviour
{
    protected GameObject keyboardKey;
    protected Player player;

    protected void Start() 
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        keyboardKey = FindObject(player.gameObject, "K");
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {   
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) keyboardKey.SetActive(true);
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) keyboardKey.SetActive(false);
    }

    protected abstract void OnGUI();

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
}

