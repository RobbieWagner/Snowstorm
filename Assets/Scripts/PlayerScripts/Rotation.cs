using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    // Handles the rotation of the player
    [SerializeField]
    Transform playerT;
    private Player player;

    [HideInInspector]
    public int currentRotationState;
    [HideInInspector]
    public Quaternion[] rotationStates;
    [SerializeField]
    private Sprite[] compassStates;
    [SerializeField]
    private Image compassSprite;

    [SerializeField]
    float rotationSpeed;
    
    bool rotatingPlayer;

    void Start()
    {
        currentRotationState = 0;

        rotationStates = new Quaternion[6];
        for(int i = 0; i < rotationStates.Length; i++)
        {
            rotationStates[i] = Quaternion.Euler(0, i * 360/rotationStates.Length, 0);
        }

        rotatingPlayer = true;
        StartCoroutine(SetRotationState(rotationSpeed));
        player = playerT.gameObject.GetComponent<Player>();
    }

    // Allow rotation for players that are outside and not currently rotating
    void Update() 
    {
        if(!player.playerIsInside && !rotatingPlayer)
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(currentRotationState == rotationStates.Length - 1) currentRotationState = 0;
                else currentRotationState++;

                StartCoroutine(SetRotationState(rotationSpeed));
            }

            else if(Input.GetKeyDown(KeyCode.Q))
            {
                if(currentRotationState == 0) currentRotationState = rotationStates.Length - 1;
                else currentRotationState--;

                StartCoroutine(SetRotationState(rotationSpeed));
            }
    }

    // Rotates the player
    public IEnumerator SetRotationState(float rotationSpeed)
    {
        rotatingPlayer = true;

        while(playerT.eulerAngles.y != rotationStates[currentRotationState].eulerAngles.y)
        {
            compassSprite.sprite = compassStates[currentRotationState];
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotationStates[currentRotationState].eulerAngles.y, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, angle, 0);
            yield return null;
        }

        rotatingPlayer = false;
        StopCoroutine(SetRotationState(rotationSpeed));
    }
}