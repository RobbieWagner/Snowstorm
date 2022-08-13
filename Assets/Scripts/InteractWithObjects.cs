using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{

    private Fire fire;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if((fire = collision.gameObject.GetComponent<Fire>()) != null)
        {
            Debug.Log("hi");
            fire.canLight = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if((fire = collision.gameObject.GetComponent<Fire>()) != null)
        {
            fire.canLight = false;
        }
    }
}
