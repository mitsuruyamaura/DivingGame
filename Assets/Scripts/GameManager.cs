using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField, Header("移動する花輪の割合"),Range(0, 100)]
    private int movingFlowerCircleRatio = 0;

    [SerializeField, Header("大きさが変化する花輪の割合"), Range(0, 100)]
    private int scalingFlowerCircleRatio = 0;

    [SerializeField]
    private FlowerCircle flowerCirclePrefab = null;

    [SerializeField]
    private Transform limitLeftBottom = null;

    [SerializeField]
    private Transform limitRightTop = null;

    // mi

    [SerializeField]
    private AudioClip[] bgms = null;

    private AudioSource audioSource;

    IEnumerator Start()
    {
        //player = GetComponent<PlayerController>();
        //audioSource = GetComponent<AudioSource>();

        // BGM再生
        //audioSource.clip = bgms[0];
        //audioSource.Play();

        // Updateを止める
        isGoal = true;

        // 花輪をランダムで配置する場合
        if (isRandomStaging) {

            // 花輪の生成処理を行う
            yield return StartCoroutine(CreateRandomStage());
        }

        // Updateを再開
        isGoal = false;
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
        int stageDistance = (int)(goal.position.y);
        int count = 0;
        Debug.Log(stageDistance);

        while (stageDistance <= player.transform.position.y) {
            stageDistance += Random.Range(5, 11);

            Debug.Log(stageDistance);

            // 位置を設定して生成
            FlowerCircle flowerCircle = Instantiate(flowerCirclePrefab, new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), stageDistance, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)),Quaternion.identity);
            
            // 花輪の初期設定
            flowerCircle.SetUpMovingFlowerCircle(Random.Range(0, 100) <= movingFlowerCircleRatio, Random.Range(0, 100) <= scalingFlowerCircleRatio);

            count++;

            Debug.Log("花輪生成 : " + count);

            yield return null;
        }

        Debug.Log("ランダムステージ完成");
    }
}
