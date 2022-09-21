using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float xzOffset;
    [SerializeField]
    float yOffset;
    [SerializeField]
    float tilt;

    [SerializeField]
    private Transform playerT;

    int currentRotationState;
    RotationState[] rotationStates;

    void Start()
    {
        currentRotationState = 0;

        rotationStates = new RotationState[6];
        for(int i = 0; i < rotationStates.Length; i++)
        {
            rotationStates[i] = new RotationState(new Vector3(xzOffset * (float)Math.Sin(i * 360/rotationStates.Length),
                                                                yOffset,
                                                                -xzOffset * (float)Math.Cos(i * 360/rotationStates.Length)),
                                                    Quaternion.Euler(tilt,
                                                                        i * 360/rotationStates.Length,
                                                                        0),
                                                    playerT,
                                                    gameObject);
        }

        rotationStates[currentRotationState].SetRotationState();
    }

    void OnGUI() 
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(currentRotationState == rotationStates.Length - 1) currentRotationState = 0;
            else currentRotationState++;

            rotationStates[currentRotationState].SetRotationState();
        }
    }
}

public class RotationState: MonoBehaviour
{
    Vector3 CameraPosition;
    Quaternion Rotation;
    Transform PlayerT;
    GameObject CameraGO;

    public RotationState(Vector3 cameraPosition, Quaternion rotation, Transform playerT, GameObject cameraGO)
    {
        CameraPosition = cameraPosition;
        Rotation = rotation;
        PlayerT = playerT;
        CameraGO = cameraGO;
    }

    public void SetRotationState()
    {
        CameraGO.transform.position = PlayerT.position + CameraPosition;
        CameraGO.transform.rotation = Rotation;
    }
}