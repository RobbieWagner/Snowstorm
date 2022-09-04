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

    [SerializeField]
    private Canvas fireLightingTutorial;
    [SerializeField]
    private Canvas warmthTutorial;

    bool fireLit;

    Player player;

    void Start()
    {
        fireLit = false;
        canLight = false;
        LIGHT_TIME = 60;

        player = GameObject.Find("Player").GetComponent<Player>();

        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        playerWarmthDetection = GameObject.Find("Player").GetComponent<DetectWarmth>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            canLight = true;
            if(!player.hasSeenFireLightingTutorial)
            {
                player.hasSeenFireLightingTutorial = true;
                ToggleCanvas(fireLightingTutorial);
            }
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            canLight = false;
            if(!player.hasSeenWarmthTutorial) StartCoroutine(TimeTutorialDisplay(warmthTutorial));
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

        if(fireLightingTutorial.isActiveAndEnabled) ToggleCanvas(fireLightingTutorial);
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

    void ToggleCanvas(Canvas tutorialCanvas)
    {
        if(!tutorialCanvas.isActiveAndEnabled) tutorialCanvas.enabled = true;
        else tutorialCanvas.enabled = false;
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

    IEnumerator TimeTutorialDisplay(Canvas tutorialCanvas)
    {
        player.hasSeenWarmthTutorial = true;
        ToggleCanvas(tutorialCanvas);
        yield return new WaitForSeconds(10f);
        ToggleCanvas(tutorialCanvas);

        StopCoroutine(TimeTutorialDisplay(tutorialCanvas));
    }
}
