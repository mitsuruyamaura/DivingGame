using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    private PlayerController player;

    [SerializeField]
    private Transform goal;

    private bool isGoal;

    private float distance;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal == true) {
            return;
        }

        distance = player.transform.position.y - goal.position.y;
        Debug.Log(distance.ToString("F2"));

        if(distance <= 0) {
            isGoal = true;
            Debug.Log("Goal");
        }
    }
}
