using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable
{
    // Functionality for lightable fires, inherits interactable

    public bool canLight;
    int LIGHT_TIME;

    GameObject playerGO;
    Player player;
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

    GameMatchsticks matchsticks;

    protected void Start()
    {
        base.Start();

        fireLit = false;
        canLight = false;
        LIGHT_TIME = 60;

        playerGO = GameObject.Find("Player");
        playerMovement = playerGO.GetComponent<Movement>();
        playerWarmthDetection = playerGO.GetComponent<DetectWarmth>();
        player = playerGO.GetComponent<Player>();
        matchsticks = GameObject.Find("GameMatchsticks").GetComponent<GameMatchsticks>();
    }

    // Allow player to interact with fire only if they are close enough
    protected override void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            //display keyhint if fire is not lit
            if(!fireLit)
            {
                player.j.SetActive(true);
                playerCanInteract = true;
                player.canInteractWithObjects = true;
            }

            canLight = true;
            if(!player.hasSeenFireLightingTutorial)
            {
                player.hasSeenFireLightingTutorial = true;
                ToggleCanvas(fireLightingTutorial);
            }
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }

    protected override void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            player.j.SetActive(false);
            playerCanInteract = false;
            player.canInteractWithObjects = false;
            
            canLight = false;
            if(!player.hasSeenWarmthTutorial && fireLit) StartCoroutine(TimeTutorialDisplay(warmthTutorial));
        }
        else Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

        if(fireLightingTutorial.isActiveAndEnabled) ToggleCanvas(fireLightingTutorial);
    }

    // Press J to light a fire
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.J) && playerMovement.canMove && canLight && !fireLit && matchsticks.matchsticksCount > 0 && playerCanInteract && player.canInteractWithObjects)
        {
            keyboardKey.SetActive(false);
            LightFire();
            StartCoroutine(matchsticks.ChangeNumberOfMatchsticks(matchsticks.matchsticksCount - 1));
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

    // Times the fire burning. Fire will go out after LIGHT_TIME seconds
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

    // Display tutorial for lighting fires
    IEnumerator TimeTutorialDisplay(Canvas tutorialCanvas)
    {
        player.hasSeenWarmthTutorial = true;
        ToggleCanvas(tutorialCanvas);
        yield return new WaitForSeconds(10f);
        ToggleCanvas(tutorialCanvas);

        StopCoroutine(TimeTutorialDisplay(tutorialCanvas));
    }

    protected override void Interact()
    {
        
    }
}
