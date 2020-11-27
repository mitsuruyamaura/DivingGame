using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
        
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
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
}
