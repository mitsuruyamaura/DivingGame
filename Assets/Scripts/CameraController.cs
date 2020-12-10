using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private Player playerObj = null;

    private Vector3 offset;

    [SerializeField]
    private Camera fpsCamera = null;

    [SerializeField]
    private Camera selfishCamera = null;

    [SerializeField]
    private Button btnChangeCameara = null;

    private int cameraIndex;
    private Camera mainCamera;


    void Start()
    {
        offset = transform.position - playerController.transform.position;
        if (playerObj != null) {
            offset = transform.position - playerObj.transform.position;
        }

        mainCamera = Camera.main;
        btnChangeCameara.onClick.AddListener(ChangeCamera);
        SetDefaultCamera();
    }

    // Update is called once per frame
    void Update() {
        //if (playerController.inWater == true) {
        //    return;
        //}

        //if (playerController != null) {
        //    transform.position = playerController.transform.position + offset;
        //}


        if (playerObj.inWater == true) {
            return;
        }

        if (playerObj != null) {
            transform.position = playerObj.transform.position + offset;
        }


    }

    /// <summary>
    /// カメラを変更
    /// </summary>
    private void ChangeCamera() {      

        switch (cameraIndex) {
            case 0:
                cameraIndex++;
                mainCamera.enabled = false;
                fpsCamera.enabled = true;
                break;
            case 1:
                cameraIndex++;
                fpsCamera.enabled = false;
                selfishCamera.enabled = true;
                break;
            case 2:
                cameraIndex = 0;
                selfishCamera.enabled = false;
                mainCamera.enabled = true;
                break;
        }
    }

    /// <summary>
    /// 初期カメラに戻す
    /// </summary>
    public void SetDefaultCamera() {
        cameraIndex = 0;

        mainCamera.enabled = true;
        fpsCamera.enabled = false;
        selfishCamera.enabled = false;
    }
}
