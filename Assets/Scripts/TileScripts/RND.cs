using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RND : MonoBehaviour
{
    public System.Random rnd;

    // Creates a random number generator for any class to use
    void Start()
    {
        rnd = new System.Random();
    }
}
