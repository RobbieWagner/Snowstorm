using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        surroundingTilesGenerated = false;

        random = GameObject.Find("Random").GetComponent<RND>();
        rnd = random.rnd;

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");

        tilesGenerated = new bool[] {false, false, false, false, false, false};
        tilesBlockingMegaGeneration = new bool[] {false, false, false, false, false, false};

        levelsTiles = GameObject.Find("TileList").GetComponent<TileList>();
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
        Vector3 direction;
        for(int i = 0; i < tilesGenerated.Length; i++)
        {
            direction = TileDirection(i);
            //Potential issue: layerMask was removed from if statement. May lead to issues further down the road            
            if(Physics.Linecast(tileT.position, tileT.position + direction))//, groundLayer)) 
            {
                tilesGenerated[i] = true;
            }

            Vector3 centerOfMegaTile = tileT.position + (direction * 2);
            //looks for potential of mega tile generation
            for(int j = 0; j < tilesBlockingMegaGeneration.Length; j++)
            {
                direction = TileDirection(j);
                Debug.Log(centerOfMegaTile.ToString() + " " + (centerOfMegaTile + direction).ToString());
                if(Physics.Linecast(centerOfMegaTile, centerOfMegaTile + direction))
                {
                    tilesBlockingMegaGeneration[i] = true;
                    Debug.Log(i + " cant form mega tile because of " + j);
                }
            }
        }
    }

    void GenerateTiles()
    {
        Vector3 direction;
        int tileToUse;
        for(int i = 0; i < 6; i++)
        {
            if(!tilesGenerated[i])
            {
                direction = TileDirection(i);
                tileToUse = levelsTiles.tileSpawns[rnd.Next(levelsTiles.tileSpawns.Count)];
                Instantiate(levelsTiles.tileOptions[tileToUse], tileT.position + direction, Quaternion.identity);
            }
        }
        surroundingTilesGenerated = true;
        Destroy(gameObject);
    }

    Vector3 TileDirection(int adjacentTile)
    {
        if(adjacentTile == 0) return new Vector3(0f, 0f, 18f);
        else if(adjacentTile == 1) return new Vector3(15.5889f, 0f, 9f);
        else if(adjacentTile == 2) return new Vector3(15.5889f, 0f, -9f); 
        else if(adjacentTile == 3) return new Vector3(0f, 0f, -18f); 
        else if(adjacentTile == 4) return new Vector3(-15.5889f, 0f, -9f); 
        else return new Vector3(-15.5889f, 0f, 9f); 
    }
}
