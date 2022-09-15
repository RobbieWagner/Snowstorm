using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameMusic : MonoBehaviour
{
    [SerializeField]
    private AudioSource song;

    [SerializeField]
    private Player player;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.tilesGenerated % 500 == 250 || player.tilesGenerated % 500 == 251 || player.tilesGenerated % 500 == 252) StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        song.volume = 0;
        song.Stop();
        song.Play();
        while(song.volume < .99)
        {
            yield return new WaitForSeconds(.6f);
            song.volume += .125f;
        }

        yield return new WaitForSeconds(.1f);
        StopCoroutine(PlaySong());
    }
}
