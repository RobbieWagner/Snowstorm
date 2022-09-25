using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwap : MonoBehaviour
{
    // Swaps two canvases
    [SerializeField]
    private Canvas thisCanvas;
    [SerializeField]
    private Canvas otherCanvas;

    public void SwapCanvases()
    {
        thisCanvas.enabled = false;
        otherCanvas.enabled = true;
    }
}
