using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    Transform playerT;

    int currentRotationState;
    Quaternion[] rotationStates;

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
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.R) && !rotatingPlayer)
        {
            if(currentRotationState == rotationStates.Length - 1) currentRotationState = 0;
            else currentRotationState++;

            StartCoroutine(SetRotationState(rotationSpeed));
        }

        else if(Input.GetKeyDown(KeyCode.E) && !rotatingPlayer)
        {
            if(currentRotationState == 0) currentRotationState = rotationStates.Length - 1;
            else currentRotationState--;

            StartCoroutine(SetRotationState(rotationSpeed));
        }
    }

    public IEnumerator SetRotationState(float rotationSpeed)
    {
        rotatingPlayer = true;

        while(playerT.eulerAngles.y != rotationStates[currentRotationState].eulerAngles.y)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotationStates[currentRotationState].eulerAngles.y, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, angle, 0);
            yield return null;
        }

        rotatingPlayer = false;
        StopCoroutine(SetRotationState(rotationSpeed));
    }
}