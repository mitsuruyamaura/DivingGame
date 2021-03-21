using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GameManager : MonoBehaviour        // GameManagerにする
{
    [SerializeField]
    private Player player = null;

    //[SerializeField]
    //private PlayerController player = null;

    [SerializeField]
    private Transform goal = null;

    private float distance;

    private bool isGoal;

    [SerializeField]
    private Text txtDistance = null;

    [SerializeField]
    private CameraController cameraController = null;

    [SerializeField]
    private ResultPopUp resultPopUp = null;

    [SerializeField]
    private AudioManager audioManager = null;

    [SerializeField, Header("ステージをランダム生成する場合にはチェックする")]
    private bool isRandomStaging = false;

    [SerializeField, Header("移動する花輪の割合"), Range(0, 100)]
    private int movingFlowerCirclePercent = 0;

    [SerializeField, Header("大きさが変化する花輪の割合"), Range(0, 100)]
    private int scalingFlowerCirclePercent = 0;

    [SerializeField]
    private FlowerCircle flowerCirclePrefab = null;

    [SerializeField]
    private Transform limitLeftBottom = null;

    [SerializeField]
    private Transform limitRightTop = null;

    public Slider sliderAltimeter = null;

    private float startPos;

    [SerializeField]
    private SkyboxChanger skyboxChanger;

    [SerializeField, Header("障害物とアイテムをランダムに生成する場合にはチェックする")]
    private bool isRandomObjects;

    [SerializeField,Header("障害物とアイテムのプレファブ登録")]
    private GameObject[] randomObjPrefabs;


    // mi

    [SerializeField]
    private AudioClip[] bgms = null;

    private AudioSource audioSource;


    void Awake() {

        // Skyboxの変更
        skyboxChanger.ChangeSkybox();
    }

    IEnumerator Start()
    {
        //player = GetComponent<PlayerController>();
        //audioSource = GetComponent<AudioSource>();

        // BGM再生
        //audioSource.clip = bgms[0];
        //audioSource.Play();

        // スタート地点取得
        startPos = player.transform.position.y;

        // プレイヤーの移動を停止
        player.StopMove();

        // Updateを止める
        isGoal = true;

        // 花輪をランダムで配置する場合
        if (isRandomStaging) {

            // 花輪の生成処理を行う
            yield return StartCoroutine(CreateRandomStage());
        }

        // 障害物とアイテムをランダムで配置する場合
        if (isRandomObjects) {

            // 障害物とアイテムをランダムに生成して設置する
            yield return StartCoroutine(CreateRandomObjects());
        }

        // Updateを再開
        isGoal = false;

        player.ResumeMove();
        Debug.Log(isGoal);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal == true) {
            return;
        }

        distance = player.transform.position.y - goal.position.y;
        txtDistance.text = distance.ToString("F2");
        //Debug.Log(distance.ToString("F2"));

        // 高度計を更新
        sliderAltimeter.DOValue(distance / startPos, 0.1f);

        if(distance <= 0) {
            isGoal = true;
            txtDistance.text = 0.ToString("F2");
            //Debug.Log("Goal");

            // リザルト表示
            resultPopUp.DisplayResult();

            // カメラを初期位置に戻す
            cameraController.SetDefaultCamera();

            audioManager.PlayBGM(AudioManager.BgmType.GameClear);
            // BGM切り替え
            //audioSource.Stop();
            //audioSource.clip = bgms[1];
            //audioSource.Play();
        }
    }

    /// <summary>
    /// ランダムで花輪を生成してステージ作成
    /// </summary>
    private IEnumerator CreateRandomStage() {

        // ステージの長さ
        int flowerHeight = (int)(goal.position.y);
        int count = 0;
        Debug.Log("初期の花輪のスタート位置 : " + flowerHeight);

        while (flowerHeight <= player.transform.position.y) {
            flowerHeight += Random.Range(5, 11);

            Debug.Log("現在の花輪の生成位置 : " + flowerHeight);

            // 位置を設定して生成
            FlowerCircle flowerCircle = Instantiate(flowerCirclePrefab, new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), flowerHeight, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)),Quaternion.identity);
            
            // 花輪の初期設定
            flowerCircle.SetUpMovingFlowerCircle(Random.Range(0, 100) <= movingFlowerCirclePercent, Random.Range(0, 100) <= scalingFlowerCirclePercent);

            count++;

            Debug.Log("花輪の合計生成数 : " + count);

            yield return null;
        }

        Debug.Log("ランダムステージ完成");
    }

    /// <summary>
    /// // 障害物とアイテムをランダムに生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateRandomObjects() {

        // ステージの長さ
        int height = (int)(goal.position.y);
        int count = 0;
        Debug.Log("初期のスタート位置 : " + height);

        while (height <= player.transform.position.y) {
            height += Random.Range(10, 15);

            Debug.Log("現在の生成位置 : " + height);

            // 位置を設定して生成
            Instantiate(randomObjPrefabs[Random.Range(0, randomObjPrefabs.Length)], new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), height, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)), Quaternion.identity);

            count++;

            Debug.Log("障害物とアイテムの合計生成数 : " + count);

            yield return null;
        }

        Debug.Log("障害物とアイテムのランダム配置完成");
    }
}
