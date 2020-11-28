using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float moveSpeed;

    public float fallSpeed;

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

    [SerializeField]
    private Text txtScore = null;

    [SerializeField]
    private Image imgGauge = null;


    private float attitudeTimer;

    private float chargeTime = 2.0f;

    private bool isCharge;

    [SerializeField]
    private ShinyEffectForUGUI shinyEffect = null;

    void Start()
    {
        // 初期の姿勢を設定
        transform.eulerAngles = straightRotation;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        //Debug.Log(x);
        //Debug.Log(z);

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
            score += col.transform.parent.GetComponent<FlowerCircle>().point;

            txtScore.text = score.ToString();

            // エフェクト


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

        // 未チャージ状態かつ、姿勢が普通の状態
        if (isCharge == false && attitudeType == AttitudeType.Straight) {

            // チャージを行う
            attitudeTimer += Time.deltaTime;

            // ゲージ表示を更新
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            // ゲージが満タンになったら
            if (attitudeTimer >= chargeTime) {

                // チャージ状態にする
                isCharge = true;

                attitudeTimer = chargeTime;

                // 満タン時のエフェクト
                shinyEffect.Play(0.5f);
            }
        }

        // 姿勢が伏せの状態
        if (attitudeType == AttitudeType.Prone) {

            // ゲージを減らす
            attitudeTimer -= Time.deltaTime;

            // ゲージ表示を更新
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            // ゲージが0になったら
            if (attitudeTimer <= 0) {

                attitudeTimer = 0;

                // 強制的に姿勢を直滑降に戻す
                ChangeAttitude();
            }
        }            
    }

    /// <summary>
    /// 姿勢の変更
    /// </summary>
    private void ChangeAttitude() {
        switch (attitudeType) {
            case AttitudeType.Straight:
                if(isCharge == false) {
                    return;
                }

                isCharge = false;
                attitudeType = AttitudeType.Prone;
                anim.SetBool("Prone", true);

                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);
                rb.drag = 25.0f;
                //transform.eulerAngles = proneRotation;
                break;
            case AttitudeType.Prone:

                attitudeType = AttitudeType.Straight;
                anim.SetBool("Prone", false);
                transform.DORotate(straightRotation, 0.25f);
                rb.drag = 0f;
                //transform.eulerAngles = straightRotation;
                break;
        }
    }
}
