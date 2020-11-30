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
    private Text txtDistance = null;

    [SerializeField]
    private ResultPopUp resultPopUp = null;

    [SerializeField]
    private CameraController cameraController = null;

    [SerializeField]
    private AudioClip[] bgms = null;


    private AudioSource audioSource;

    void Start()
    {
        //player = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        // BGM再生
        audioSource.clip = bgms[0];
        audioSource.Play();
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

            // BGM切り替え
            audioSource.Stop();
            audioSource.clip = bgms[1];
            audioSource.Play();
        }
    }
}
