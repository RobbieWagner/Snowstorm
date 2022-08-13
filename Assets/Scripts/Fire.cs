using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    bool canLight;
    int LIGHT_TIME;

    Movement playerMovement;

    [SerializeField]
    GameObject firewood;
    [SerializeField]
    GameObject flame;
    [SerializeField]
    GameObject burntFirewood;

    void Start()
    {
        canLight = false;
        LIGHT_TIME = 60;

        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            canLight = true;
            Debug.Log("can light");
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) canLight = false;
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.J) && playerMovement.canMove && canLight)
        {
            LightFire();
        }
    }

    void LightFire()
    {
        flame.SetActive(true);
        StartCoroutine(BurnWood());
    }

    IEnumerator BurnWood()
    {
        yield return new WaitForSeconds(LIGHT_TIME);
        flame.SetActive(false);
        firewood.SetActive(false);
        burntFirewood.SetActive(true);

        StopCoroutine(BurnWood());
    }
}
