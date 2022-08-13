using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool canLight;
    int LIGHT_TIME;

    Movement playerMovement;
    DetectWarmth playerWarmthDetection;

    [SerializeField]
    GameObject firewood;
    [SerializeField]
    GameObject flame;
    [SerializeField]
    GameObject burntFirewood;

    bool fireLit;

    void Start()
    {
        fireLit = false;
        canLight = false;
        LIGHT_TIME = 60;

        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        playerWarmthDetection = GameObject.Find("Player").GetComponent<DetectWarmth>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            canLight = true;
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) canLight = false;
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.J) && playerMovement.canMove && canLight && !fireLit)
        {
            LightFire();
        }
    }

    void LightFire()
    {
        flame.SetActive(true);
        fireLit = true;
        StartCoroutine(BurnWood());
    }

    IEnumerator BurnWood()
    {
        yield return new WaitForSeconds(LIGHT_TIME);
        flame.SetActive(false);
        firewood.SetActive(false);
        burntFirewood.SetActive(true);

        playerWarmthDetection.depleting = true;
        playerWarmthDetection.replenishing = false;

        StopCoroutine(BurnWood());
    }
}
