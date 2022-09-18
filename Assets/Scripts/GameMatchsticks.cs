using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMatchsticks : MonoBehaviour
{
    // have a current number of matchsticks
    // create a function to change number of matchsticks
    //have a function to display these changes
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

        imageColor = new Color(0, 0, 0, .5f);
        matchsticksText.color = imageColor;
    }

    public IEnumerator ChangeNumberOfMatchsticks(int newMatchsticksCount)
    {
        Color imageColor = matchsticksIcon.color;
        imageColor.a = 1f;
        matchsticksIcon.color = imageColor;

        imageColor = new Color(0, 0, 0, 1f);
        matchsticksText.color = imageColor;

        matchsticksCount = newMatchsticksCount;
        matchsticksText.text = " X " + matchsticksCount;

        yield return new WaitForSeconds(2f);

        imageColor = matchsticksIcon.color;
        imageColor.a = .5f;
        matchsticksIcon.color = imageColor;

        imageColor = new Color(0, 0, 0, .5f);
        matchsticksText.color = imageColor;

        StopCoroutine(ChangeNumberOfMatchsticks(newMatchsticksCount));
    }
}