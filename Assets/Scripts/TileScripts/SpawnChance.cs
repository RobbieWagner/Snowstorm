using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChance : MonoBehaviour
{
    // Stores the chance a tile/object has to spawn
    public int spawnChances;

    public bool onlySpawnsOnce = false;
    public bool canSpawn = true;

    public int tileRequirementsForSpawn = 0;

    [HideInInspector]
    public bool placedIntoArray = false;
}
