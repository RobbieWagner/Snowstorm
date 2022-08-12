using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RollCredits : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI developerCreditCard;
    [SerializeField]
    private TextMeshProUGUI rollingCredits;

    // Start is called before the first frame update
    void Start()
    {
        developerCreditCard.gameObject.SetActive(true);
        rollingCredits.gameObject.SetActive(false);
        StartCoroutine(RunCredits());
    }

    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScreen");
        }
    }

    public IEnumerator RunCredits()
    {
        yield return new WaitForSeconds(4f);

        developerCreditCard.gameObject.SetActive(false);
        rollingCredits.gameObject.SetActive(true);
    }
}
