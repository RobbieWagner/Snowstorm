using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    // Handles tile generation
    [SerializeField]
    private Transform tileT;

    private TileList levelsTiles;
    private RND random;
    private System.Random rnd;
    private bool surroundingTilesGenerated;

    private LayerMask playerLayer;
    private LayerMask groundLayer;

    private bool[] tilesGenerated; 
    private bool[] tilesBlockingMegaGeneration;

    private int megaTileSpawnChance;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        surroundingTilesGenerated = false;

        random = GameObject.Find("Random").GetComponent<RND>();
        rnd = random.rnd;

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");

        tilesGenerated = new bool[] {false, false, false, false, false, false};
        tilesBlockingMegaGeneration = new bool[] {false, false, false, false, false, false};

        levelsTiles = GameObject.Find("TileList").GetComponent<TileList>();
        megaTileSpawnChance = levelsTiles.chanceOfMegaTile;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            if(!surroundingTilesGenerated)
            {
                CheckForSurroundingTiles();
                GenerateTiles();
            }
        }
    }

    void CheckForSurroundingTiles()
    {
        // Linecasts from tile out to empty space, checking if tiles were already generated
        Vector3 direction;
        for(int i = 0; i < tilesGenerated.Length; i++)
        {
            direction = TileDirection(i);
            //Potential issue: layerMask was removed from if statement. May lead to issues further down the road            
            // if(Physics.Linecast(tileT.position, tileT.position + direction))//, groundLayer))
            // {
            //     tilesGenerated[i] = true;
            // }

            if(Physics.Linecast(tileT.position + direction + new Vector3(0, -1, 0), tileT.position + direction + new Vector3(0, 1, 0)))//, groundLayer))
            {
                tilesGenerated[i] = true;
            }

            Vector3 centerOfMegaTile = tileT.position + (direction * 2);
            //looks for potential of mega tile generation
            for(int j = 0; j < tilesBlockingMegaGeneration.Length; j++)
            {
                direction = TileDirection(j);
                if(Physics.Linecast(centerOfMegaTile, centerOfMegaTile + direction))
                {
                    tilesBlockingMegaGeneration[i] = true;
                }
            }
        }
    }

    void GenerateTiles()
    {
        // generates tiles in empty spots
        Vector3 direction;
        int tileToUse;
        bool megaTileSpawned = false;
        for(int i = 0; i < 6; i++)
        {
            int randomNumber = rnd.Next(1000);
            if(!tilesBlockingMegaGeneration[i] && !megaTileSpawned && randomNumber < megaTileSpawnChance)
            {
                direction = TileDirection(i) * 2;
                tileToUse = levelsTiles.size2TileSpawns[rnd.Next(levelsTiles.size2TileSpawns.Count)];
                Instantiate(levelsTiles.size2TileOptions[tileToUse], tileT.position + direction, Quaternion.identity);
                megaTileSpawned = true;
                player.tilesGenerated++;
            }
            else if(!tilesGenerated[i])
            {
                direction = TileDirection(i);
                tileToUse = levelsTiles.tileSpawns[rnd.Next(levelsTiles.tileSpawns.Count)];
                Instantiate(levelsTiles.tileOptions[tileToUse], tileT.position + direction, Quaternion.identity);
                player.tilesGenerated++;
            }
        }
        surroundingTilesGenerated = true;
        Destroy(gameObject);
    }

    Vector3 TileDirection(int adjacentTile)
    {
        // returns the direction a linecast will be performed in
        if(adjacentTile == 0) return new Vector3(0f, 0f, 18f);
        else if(adjacentTile == 1) return new Vector3(15.5889f, 0f, 9f);
        else if(adjacentTile == 2) return new Vector3(15.5889f, 0f, -9f); 
        else if(adjacentTile == 3) return new Vector3(0f, 0f, -18f); 
        else if(adjacentTile == 4) return new Vector3(-15.5889f, 0f, -9f); 
        else return new Vector3(-15.5889f, 0f, 9f); 
    }
}
