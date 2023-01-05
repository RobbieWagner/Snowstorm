using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    // Stores the list of possible tiles, and also the chance of a mega tile spawning
    public List<GameObject> tileOptions;
    //[HideInInspector]
    public List<int> tileSpawns;

    public List<GameObject> size2TileOptions;
    [HideInInspector]
    public List<int> size2TileSpawns;

    //[HideInInspector]
    public List<GameObject> generatedTilesList;
    
    public int chanceOfMegaTile;

    private Player player;

    [SerializeField]
    public GameObject borderTile;

    [SerializeField]
    public int tileLimit;

    [SerializeField]
    public string tileType;

    [HideInInspector]
    public bool started = false;

    public void Start()
    {
        if(!started)
        {
            player = GameObject.Find("Player").GetComponent<Player>();

            for(int i = 0; i < tileOptions.Count; i++)
            {
                SpawnChance spawnChance = tileOptions[i].GetComponent<SpawnChance>();
                spawnChance.placedIntoArray = false;
                if(spawnChance.tileRequirementsForSpawn == 0)
                {
                    spawnChance.placedIntoArray = true;
                    for(int j = 0; j < spawnChance.spawnChances; j++) tileSpawns.Add(i);
                }
            }

            for(int i = 0; i < size2TileOptions.Count; i++)
            {
                SpawnChance spawnChance = size2TileOptions[i].GetComponent<SpawnChance>();
                spawnChance.placedIntoArray = false;
                if(spawnChance.tileRequirementsForSpawn == 0)
                {
                    spawnChance.placedIntoArray = true;
                    for(int j = 0; j < spawnChance.spawnChances; j++) size2TileSpawns.Add(i);
                }
            }
        }

        started = true;
    }

    //Find a way to make this less intensive
    public void CheckForTileAdditions()
    {
        //Checks if a tile has met the requirements needed to be added onto the tile lists
        for(int i = 0; i < tileOptions.Count; i++)
        {
            SpawnChance spawnChance = tileOptions[i].GetComponent<SpawnChance>();
            if(!spawnChance.placedIntoArray && spawnChance.tileRequirementsForSpawn <= player.tilesGenerated)
            {
                spawnChance.placedIntoArray = true;
                for(int j = 0; j < spawnChance.spawnChances; j++) tileSpawns.Add(i);
            }
        }

        for(int i = 0; i < size2TileOptions.Count; i++)
        {
            SpawnChance spawnChance = size2TileOptions[i].GetComponent<SpawnChance>();
            if(!spawnChance.placedIntoArray && spawnChance.tileRequirementsForSpawn <= player.tilesGenerated)
            {
                spawnChance.placedIntoArray = true;
                for(int j = 0; j < spawnChance.spawnChances; j++) size2TileSpawns.Add(i);
            }
        }
    }

    public void RemoveFromSize2List(int tile)
    {
        while(size2TileSpawns.Contains(tile)) size2TileSpawns.Remove(tile);
    }

    public void RemoveFromList(int tile)
    {
        while(tileSpawns.Contains(tile)) tileSpawns.Remove(tile);
    }
}
