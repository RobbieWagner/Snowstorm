using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMatchsticks : MonoBehaviour
{
    // Functionality for matchsticks, which limits the usage of fires
    
    public int matchsticksCount;
    [SerializeField]
    private Image matchsticksIcon;
    [SerializeField]
    private TextMeshProUGUI matchsticksText;

    private void Start()
    {
        Color imageColor = matchsticksIcon.color;
        imageColor.a = .5f;
        matchsticksIcon.color = imageColor;

        imageColor = new Color(1, 1, 1, .5f);
        matchsticksText.color = imageColor;
    }

    //Flickers icon. Indicates to player that they have used a match
    public IEnumerator ChangeNumberOfMatchsticks(int newMatchsticksCount)
    {
        Color imageColor = matchsticksIcon.color;
        imageColor.a = 1f;
        matchsticksIcon.color = imageColor;

        imageColor = new Color(1, 1, 1, 1f);
        matchsticksText.color = imageColor;

        matchsticksCount = newMatchsticksCount;
        matchsticksText.text = " X " + matchsticksCount;

        yield return new WaitForSeconds(2f);

        imageColor = matchsticksIcon.color;
        imageColor.a = .5f;
        matchsticksIcon.color = imageColor;

        imageColor = new Color(1, 1, 1, .5f);
        matchsticksText.color = imageColor;

        StopCoroutine(ChangeNumberOfMatchsticks(newMatchsticksCount));
    }
}
