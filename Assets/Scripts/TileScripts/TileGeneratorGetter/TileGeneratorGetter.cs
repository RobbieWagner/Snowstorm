using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneratorGetter : MonoBehaviour
{
    public TileGenerator tileG;

    public void RunRecurseGeneration()
    {
        tileG.RecurseGeneration();
    }
}
