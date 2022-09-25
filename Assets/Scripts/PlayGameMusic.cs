using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameMusic : MonoBehaviour
{
    // Controls the playing of music in game

    [SerializeField]
    private AudioSource song;

    [SerializeField]
    private Player player;

    // Update is called once per frame
    private void Start() 
    {
        song.volume = 0;
    }

    void FixedUpdate()
    {
        if((player.tilesGenerated % 500 == 250 || player.tilesGenerated % 500 == 251 || player.tilesGenerated % 500 == 252) && !song.isPlaying) StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        song.Stop();
        song.Play();
        song.volume = 0;

        while(song.volume < .99)
        {
            yield return new WaitForSeconds(.6f);
            song.volume += .125f;
        }

        yield return new WaitForSeconds(.1f);
        StopCoroutine(PlaySong());
    }
}
