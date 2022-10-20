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

    [SerializeField]
    private bool runOnStart = false;

    private bool finishedGeneration;
    private List<GameObject> instantiatedGO;
    private bool[] isTileGenerated; 
    private bool[] tilesBlockingMegaGeneration;

    private int megaTileSpawnChance;

    private Player player;

    [SerializeField]
    public bool recurseGeneration = true;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        surroundingTilesGenerated = false;

        random = GameObject.Find("Random").GetComponent<RND>();
        rnd = random.rnd;

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");

        instantiatedGO = new List<GameObject>();
        isTileGenerated = new bool[] {false, false, false, false, false, false};
        tilesBlockingMegaGeneration = new bool[] {false, false, false, false, false, false};
        finishedGeneration = false;

        levelsTiles = GameObject.Find("TileList").GetComponent<TileList>();
        megaTileSpawnChance = levelsTiles.chanceOfMegaTile;

        started = true;

        if(runOnStart)
        {
            RecurseGeneration();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //When player steps on a tile, generate next tiles
        if(collision.gameObject.layer == playerLayer)
        {
            if(!surroundingTilesGenerated)
            {
                CheckForSurroundingTiles();
                RunTileGenerator();
            }
        }
    }

    public void RecurseGeneration()
    {
        if(!finishedGeneration && !surroundingTilesGenerated)
        {
            if(!started)
            {
                started = true;
                Start();
            }
            CheckForSurroundingTiles();
            RunTileGenerator();
        }
    }

    void CheckForSurroundingTiles()
    {
        // Linecasts from tile out to empty space, checking if tiles were already generated
        Vector3 direction;
        if(isTileGenerated != null)
        for(int i = 0; i < isTileGenerated.Length; i++)
        {
            direction = TileDirection(i);

            if(Physics.Linecast(tileT.position + direction + new Vector3(0, -1, 0), tileT.position + direction + new Vector3(0, 1, 0)))//, groundLayer))
            {
                isTileGenerated[i] = true;
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

    void RunTileGenerator()
    {
        // generates tiles in empty spots
        Vector3 direction;
        bool megaTileSpawned = false;
        for(int i = 0; i < 6; i++)
        {
            if(rnd == null)
            {
                random = GameObject.Find("Random").GetComponent<RND>();
                rnd = random.rnd;
            }
            
            int randomNumber = rnd.Next(1000);

            if(levelsTiles.tileLimit > player.tilesGenerated)
            {
                //generate mega tile
                if(!tilesBlockingMegaGeneration[i] && !megaTileSpawned && randomNumber < megaTileSpawnChance)
                {
                    direction = TileDirection(i) * 2;

                    List<int> tileSpawns = levelsTiles.size2TileSpawns;
                    List<GameObject> tiles = levelsTiles.size2TileOptions;

                    GenerateTile(tiles, tileSpawns, direction);

                    megaTileSpawned = true;
                    player.tilesGenerated += 6;
                    
                }
                //generate regular tile
                else if(!isTileGenerated[i])
                {
                    direction = TileDirection(i);

                    List<int> tileSpawns = levelsTiles.tileSpawns;
                    List<GameObject> tiles = levelsTiles.tileOptions;

                    GenerateTile(tiles, tileSpawns, direction);

                }
            }
            else if(!isTileGenerated[i])
            {
                direction = TileDirection(i);
                Instantiate(levelsTiles.borderTile, tileT.position + direction, Quaternion.identity);
            }
        }
        surroundingTilesGenerated = true;
        levelsTiles.CheckForTileAdditions();

        finishedGeneration = true;

        if(recurseGeneration)
        {
            for(int i = 0; i < instantiatedGO.Count; i++)
            {
                Transform tileType = instantiatedGO[i].transform.Find(levelsTiles.tileType);
                if(tileType != null)
                {
                    TileGenerator tg = tileType.Find("TriggerGeneration").GetComponent<TileGenerator>();
                    if(tg != null)
                    {
                        tg.RecurseGeneration();
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    void GenerateTile(List<GameObject> tiles, List<int> tileSpawns, Vector3 direction)
    {
        int tileToUse = tileSpawns[rnd.Next(tileSpawns.Count)];
        SpawnChance goSpawnChance = tiles[tileToUse].GetComponent<SpawnChance>();

        //find a tile that can generate
        while(!goSpawnChance.canSpawn) {
            tileToUse = tileSpawns[rnd.Next(tileSpawns.Count)];
            goSpawnChance = tiles[tileToUse].GetComponent<SpawnChance>();
        }
        
        //remove from list if it cannot spawn again
        if(goSpawnChance.onlySpawnsOnce) goSpawnChance.canSpawn = false;
        
        //generate the tile
        instantiatedGO.Add(Instantiate(tiles[tileToUse], tileT.position + direction, Quaternion.identity) as GameObject);
        
        player.tilesGenerated++;
    }

    Vector3 TileDirection(int adjacentTile)
    {
        // returns the direction for tile checks and generation
        if(adjacentTile == 0) return new Vector3(0f, 0f, 18f);
        else if(adjacentTile == 1) return new Vector3(15.5889f, 0f, 9f);
        else if(adjacentTile == 2) return new Vector3(15.5889f, 0f, -9f); 
        else if(adjacentTile == 3) return new Vector3(0f, 0f, -18f); 
        else if(adjacentTile == 4) return new Vector3(-15.5889f, 0f, -9f); 
        else return new Vector3(-15.5889f, 0f, 9f); 
    }
}
