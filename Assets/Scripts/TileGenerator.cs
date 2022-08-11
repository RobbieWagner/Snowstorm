using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform tileT;

    [SerializeField]
    private TileList LevelsTiles;
    private RND random;
    private System.Random rnd;
    private bool surroundingTilesGenerated;

    private LayerMask playerLayer;
    private LayerMask groundLayer;

    private bool[] tilesGenerated; 

    // Start is called before the first frame update
    void Start()
    {
        surroundingTilesGenerated = false;

        random = GameObject.Find("Random").GetComponent<RND>();
        rnd = random.rnd;
        if(rnd == null) Debug.Log("b");

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");

        tilesGenerated = new bool[] {false, false, false, false, false, false};
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("hi");

        if(collision.gameObject.layer == playerLayer)
        {
            Debug.Log("collision successful");
            if(!surroundingTilesGenerated)
            {
                Debug.Log("Checking for Tiles");
                CheckForSurroundingTiles();
                Debug.Log("Generating Tiles");
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
            Debug.DrawRay(tileT.position, direction, Color.white, 5f, false);
            if(Physics.Raycast(tileT.position, direction, 1f, groundLayer)) 
            {
                tilesGenerated[i] = true;
                Debug.Log("Tile " + i + " already generated");
            }
            else Debug.Log("Tile " + i + " not already generated");
        }
    }

    void GenerateTiles()
    {
        Vector3 direction;
        int tileToUse;
        for(int i = 0; i < 6; i++)
        {
            direction = TileDirection(i);
            tileToUse = rnd.Next(LevelsTiles.tileOptions.Count);
            Instantiate(LevelsTiles.tileOptions[tileToUse], tileT.position + direction, Quaternion.identity);
            Debug.Log("Tile " + i + " generated");
        }
        surroundingTilesGenerated = true;
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
