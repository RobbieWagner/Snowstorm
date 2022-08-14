using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    public List<GameObject> tileOptions;
    [HideInInspector]
    public List<int> tileSpawns;

    public List<GameObject> size2TileOptions;
    [HideInInspector]
    public List<int> size2TileSpawns;
    
    [SerializeField]
    private int chanceOfMegaTile;

    void Start()
    {
        for(int i = 0; i < tileOptions.Count; i++)
        {
            SpawnChance spawnChance = tileOptions[i].GetComponent<SpawnChance>();
            for(int j = 0; j < spawnChance.spawnChances; j++) tileSpawns.Add(i);
        }

        for(int i = 0; i < size2TileOptions.Count; i++)
        {
            SpawnChance spawnChance = size2TileOptions[i].GetComponent<SpawnChance>();
            for(int j = 0; j < spawnChance.spawnChances; j++) size2TileSpawns.Add(i);
        }
    }
}
