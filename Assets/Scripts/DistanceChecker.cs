using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DistanceChecker : MonoBehaviour        // GameManagerにする
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Transform goal;

    private bool isGoal;

    private float distance;

    [SerializeField]
    private Text txtDistance;

    [SerializeField]
    private ResultPopUp resultPopUp;

    [SerializeField]
    private CameraController cameraController;

    void Start()
    {
        //player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal == true) {
            return;
        }

        distance = player.transform.position.y - goal.position.y;
        txtDistance.text = distance.ToString("F2");
        Debug.Log(distance.ToString("F2"));

        if(distance <= 0) {
            isGoal = true;
            txtDistance.text = 0.ToString("F2");
            Debug.Log("Goal");

            // リザルト表示
            resultPopUp.DisplayResult();

            // カメラを初期位置に戻す
            cameraController.SetDefaultCamera();
        }
    }
}
