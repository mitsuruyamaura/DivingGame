using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    float x;
    float z;

    private Vector3 straightRotation = new Vector3(180, 0, 0);

    private int score;

    private Vector3 proneRotation = new Vector3(-90, 0, 0);

    private bool isCharge;


    [Header("移動速度")]
    public float moveSpeed;

    [Header("落下速度")]
    public float fallSpeed;

    [Header("着水判定用。trueなら着水済")]
    public bool inWater;

    public enum AttitudeType {
        Straight,
        Prone,
    }

    [Header("現在のキャラの姿勢")]
    public AttitudeType attitudeType;


    [SerializeField, Header("水しぶきのエフェクト")]
    private GameObject waterEffectPrefab = null;

    [SerializeField, Header("水しぶきのSE")]
    private AudioClip splashSE = null;

    [SerializeField]
    private Text txtScore = null;

    [SerializeField]
    private Button btnChangeAttitude = null;


    [SerializeField]
    private Image imgGauge = null;


    private float attitudeTimer;

    private float chargeTime = 2.0f;


    [SerializeField]
    private ShinyEffectForUGUI shinyEffect = null;


    private Animator anim;

    [SerializeField]
    private Transform limitLeftBottom = null;

    [SerializeField]
    private Transform limitRightTop = null;

    [SerializeField]
    private FloatingJoystick joystick = null;


    void Start()
    {
        // 初期の姿勢を設定
        transform.eulerAngles = straightRotation;
        rb = GetComponent<Rigidbody>();
        btnChangeAttitude.onClick.AddListener(ChangeAttitude);

        // 現在の姿勢を「直滑降」に変更(いままでの姿勢)
        attitudeType = AttitudeType.Straight;

        btnChangeAttitude.interactable = false;

        // mi
        anim = GetComponent<Animator>();

    }

    void FixedUpdate() {
        if (inWater) {
            //rb.velocity = Vector3.zero;
            return;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //Debug.Log(x);
        //Debug.Log(z);

        x = joystick.Horizontal;
        z = joystick.Vertical;

        //Vector3 moveDir = new Vector3(x, 0, z);//.normalized;

        //rb.velocity = new Vector3(moveDir.x * moveSpeed, -fallSpeed, moveDir.z * moveSpeed);

        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);

        float limitX = Mathf.Clamp(transform.position.x, limitLeftBottom.position.x, limitRightTop.position.x);
        float limitZ = Mathf.Clamp(transform.position.z, limitLeftBottom.position.z, limitRightTop.position.z);

        transform.position = new Vector3(limitX, transform.position.y, limitZ);

        Debug.Log(rb.velocity);
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Water" && inWater == false) {
            // エフェクト
            GameObject effect = Instantiate(waterEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);


            Destroy(effect, 2.0f);

            // SE
            AudioSource.PlayClipAtPoint(splashSE, transform.position);

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

            // TODO 得点加算
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

        //transform.DOMoveY(0, 1.0f);
        transform.DOMoveY(4.7f, 1.0f);
    }

    void Update() {
        if (inWater) {
            btnChangeAttitude.interactable = false;
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    ChangeAttitude();
        //}

        // 未チャージ状態かつ、姿勢が普通の状態
        if (isCharge == false && attitudeType == AttitudeType.Straight) {

            // チャージを行う
            attitudeTimer += Time.deltaTime;

            // ゲージ表示を更新
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            btnChangeAttitude.interactable = false;

            // ゲージが満タンになったら
            if (attitudeTimer >= chargeTime) {

                // チャージ状態にする
                isCharge = true;

                btnChangeAttitude.interactable = true;

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

                btnChangeAttitude.interactable = false;

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

                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);
                //transform.eulerAngles = proneRotation;
                break;
            case AttitudeType.Prone:

                attitudeType = AttitudeType.Straight;
                anim.SetBool("Prone", false);
                transform.DORotate(straightRotation, 0.25f);
                rb.drag = 0f;

                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);
                //transform.eulerAngles = straightRotation;
                break;
        }
    }

    public void StopMove() {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public void ResumeMove() {
        rb.isKinematic = false;
        rb.velocity = new Vector3(0, -fallSpeed, 0);
    }
}
