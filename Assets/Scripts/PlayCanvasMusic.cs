using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCanvasMusic : MonoBehaviour
{

    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        if(!music.isPlaying && canvas.enabled)
        {
            music.Play();
        }
    }
}
