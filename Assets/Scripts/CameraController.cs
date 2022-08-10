using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float horizontalOffset;
    [SerializeField]
    float verticalOffset;
    [SerializeField]
    float tilt;

    [SerializeField]
    private Transform playerT;

    int rotationState;

    void Start()
    {
        rotationState = 0;
        transform.position = new Vector3(playerT.position.x,
                                         playerT.position.y + verticalOffset, 
                                         playerT.position.z - horizontalOffset);
        transform.rotation = Quaternion.Euler(tilt, 0, 0);
    }

    void Update()
    {
        transform.position = new Vector3(playerT.position.x,
                                         playerT.position.y + verticalOffset, 
                                         playerT.position.z - horizontalOffset);
        transform.rotation = Quaternion.Euler(tilt, 0, 0);
    }
}
