using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
        
    private Vector3 offset;

    [SerializeField]
    private Camera fpsCamera;

    [SerializeField]
    private Camera selfishCamera;

    [SerializeField]
    private Button btnChangeCameara;

    private int cameraIndex;
    private Camera mainCamera;


    void Start()
    {
        offset = transform.position - player.transform.position;
        mainCamera = Camera.main;
        btnChangeCameara.onClick.AddListener(ChangeCamera);
        SetDefaultCamera();
    }

    // Update is called once per frame
    void Update() {
        if (player.inWater == true) {
            return;
        }

        if (player != null) {
            transform.position = player.transform.position + offset;
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
