using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameMusic : MonoBehaviour
{
    // Controls the playing of music in game

    [SerializeField]
    private AudioSource[] songs;

    [SerializeField]
    private Player player;

    private int currentSong;

    private bool songStarted;

    // Update is called once per frame
    private void Start() 
    {
        foreach(AudioSource song in songs)
        song.volume = 0;

        currentSong = 0;

        songStarted = false;
    }

    void FixedUpdate()
    {
        if((player.tilesGenerated % 350 == 250 || player.tilesGenerated % 350 == 251 || player.tilesGenerated % 350 == 252) && !songs[currentSong].isPlaying) 
        {
            StartCoroutine(PlaySong(currentSong));
        }

        if(songStarted && !songs[currentSong].isPlaying)
        {
            songStarted = false;
            if(currentSong < songs.Length - 1) currentSong++;
            else currentSong = 0;
        }
    }

    private IEnumerator PlaySong(int songNumber)
    {
        songStarted = true;
        songs[songNumber].Stop();
        songs[songNumber].Play();
        songs[songNumber].volume = 0;

        while(songs[songNumber].volume < .99)
        {
            yield return new WaitForSeconds(.6f);
            songs[songNumber].volume += .125f;
        }

        yield return new WaitForSeconds(.1f);
        StopCoroutine(PlaySong(songNumber));
    }
}
