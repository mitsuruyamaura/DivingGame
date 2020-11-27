﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed;

    public float fallSpeed;
    private float proneSpeed;

    public bool inWater;

    private int score;

    float x;
    float z;

    [SerializeField]
    private GameObject waterEffectPrefab = null;

    private Vector3 straightRotation = new Vector3(180, 0, 0);

    private Vector3 proneRotation = new Vector3(-90, 0, 0);

    public enum AttitudeType {
        Straight,
        Prone,
    }
    public AttitudeType attitudeType;


    void Start()
    {
        transform.eulerAngles = straightRotation;
        rb = GetComponent<Rigidbody>();
        proneSpeed = fallSpeed / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Debug.Log(x);
        Debug.Log(z);

        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        if (attitudeType == AttitudeType.Straight) {
            rb.velocity = new Vector3(moveDir.x * moveSpeed, -fallSpeed, moveDir.z * moveSpeed);
        } else {
            rb.velocity = new Vector3(moveDir.x * moveSpeed, -fallSpeed, moveDir.z * moveSpeed);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Water" && inWater == false) {
            // エフェクト
            GameObject effect = Instantiate(waterEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);


            Destroy(effect, 2.0f);
            //rb.angularDrag = waterDrag;
            //transform.eulerAngles = new Vector3(-30, 180, 0);
            //rb.isKinematic = true;
            inWater = true;

            StartCoroutine(OutOfWater());
            //transform.DORotate(Vector3.zero, 1.0f);
        }

        if (col.gameObject.tag == "Flower") {
            Debug.Log("ステージクリア");

            Destroy(col.gameObject, 1.5f);

        }

        if (col.gameObject.tag == "FlowerCircle") {
            score += col.transform.parent.GetComponent<FlowerCircleRotater>().point;
        }
    }

    /// <summary>
    /// 水面に顔を出す
    /// </summary>
    /// <returns></returns>
    private IEnumerator OutOfWater()
    {
        yield return new WaitForSeconds(1.0f);

        rb.isKinematic = true;

        transform.eulerAngles = new Vector3(-30, 180, 0);

        transform.DOMoveY(0, 1.0f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeAttitude();
        }
    }

    /// <summary>
    /// 姿勢の変更
    /// </summary>
    private void ChangeAttitude() {
        switch (attitudeType) {
            case AttitudeType.Straight:
                attitudeType = AttitudeType.Prone;
                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);
                rb.drag = 25.0f;
                //transform.eulerAngles = proneRotation;
                break;
            case AttitudeType.Prone:
                attitudeType = AttitudeType.Straight;
                transform.DORotate(straightRotation, 0.25f);
                rb.drag = 0f;
                //transform.eulerAngles = straightRotation;
                break;
        }
    }
}
