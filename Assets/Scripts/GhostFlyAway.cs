using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFlyAway : MonoBehaviour
{
    [SerializeField]
    Animator ghostAnimator;

    GameObject player;
    bool runningAway;

    [SerializeField]
    float distanceUntilRun;
    [SerializeField]
    float distanceToRun;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        runningAway = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!runningAway && Vector3.Distance(player.transform.position, transform.position) < distanceUntilRun)
        {
            runningAway = true;
            ghostAnimator.SetBool("RunAway",true);
            StartCoroutine(RunAway());
        }

    }

    IEnumerator RunAway()
    {
        while(Vector3.Distance(player.transform.position, transform.position) < distanceToRun)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -50f * Time.deltaTime);
            yield return new WaitForSeconds(.001f);
        }

        Destroy(gameObject);

        StopCoroutine(RunAway());
    }
}
