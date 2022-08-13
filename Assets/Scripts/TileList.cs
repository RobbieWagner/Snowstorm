using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileList : MonoBehaviour
{
    public List<GameObject> tileOptions;
    [HideInInspector]
    public List<int> tileSpawns;

    void Start()
    {
        for(int i = 0; i < tileOptions.Count; i++)
        {
            SpawnChance spawnChance = tileOptions[i].GetComponent<SpawnChance>();
            for(int j = 0; j < spawnChance.spawnChances; j++) tileSpawns.Add(i);
        }
    }
}
