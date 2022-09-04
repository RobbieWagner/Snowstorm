using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Snowfall : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem snowfall;
    private Transform player;
    private Transform snowfallT;

    private bool snowFalling;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        snowfallT = gameObject.GetComponent<Transform>();

        snowFalling = true;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(player.position, snowfallT.position) > 35 && snowFalling)
        {
            snowFalling = false;
            snowfall.Stop();
        }
        else if(Vector3.Distance(player.position, snowfallT.position) < 35 && !snowFalling)
        {
            snowFalling = true;
            snowfall.Play();
        }
    }
}
