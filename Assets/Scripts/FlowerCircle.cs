using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircle : MonoBehaviour
{
    [Header("花輪通過時の得点")]
    public int point = 10;

    //private GameObject[] flowerObjs;

    [SerializeField]
    private BoxCollider boxCollider = null;

    [SerializeField]
    private GameObject effectPrefab= null;

    [SerializeField]
    private AudioClip flowerSE;

    [SerializeField, Header("移動させる場合スイッチ入れる")]
    private bool isMoving;

    [SerializeField, Header("移動する時間と距離をランダムにする割合"), Range(0, 100)]
    private int randomMovingRatio;

    [SerializeField, Header("移動時間")]
    private float duration;

    [SerializeField, Header("移動距離")]
    private float moveDistance;


    [SerializeField, Header("移動時間のランダム幅")]
    private Vector2 durationRange;

    [SerializeField, Header("移動距離のランダム幅")]
    private Vector2 moveDistanceRange;


    [SerializeField, Header("大きさの設定")]
    private float[] flowerSizes;

    [SerializeField, Header("点数の倍率")]
    private float[] pointRate;


    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        //flowerObjs = GetAllChildren();

        if (isMoving) {
            transform.DOMoveZ(transform.position.z + moveDistance, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

    //private GameObject[] GetAllChildren() {
    //    GameObject[] objs = new GameObject[transform.childCount];
    //    for (int i = 0; i < transform.childCount; i++) {
    //        objs[i] = transform.GetChild(i).gameObject;
    //    }
    //    return objs;
    //}

    private void OnTriggerEnter(Collider other) {

        // 水面に触れても判定しない
        if (other.gameObject.tag == "Water") {
            return;
        }

        boxCollider.enabled = false;

        transform.SetParent(other.transform);

        StartCoroutine(PlayGetEffect());        
    }

    /// <summary>
    /// 花輪をくぐった際の演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGetEffect() {
        // SE
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));

        //for (int i = 0; i < flowerObjs.Length; i++) {
        //    Sequence sequence = DOTween.Sequence();
        //    //sequence.Append(flowerObjs[i].transform.DOMoveY(-10.0f, 0.25f)).SetRelative();
        //    sequence.Append(flowerObjs[i].transform.DOScale(Vector3.zero, 1.0f));   // .OnComplete(() => { Destroy(flowerObjs[i]); })
        //    sequence.Join(flowerObjs[i].transform.DOLocalMove(new Vector3(0, 0, 0), 1.0f));
        //}

        yield return new WaitForSeconds(1.0f);

        // エフェクト
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y -1.5f, effect.transform.position.z);
        Destroy(effect, 1.0f);

        Destroy(gameObject, 1.0f);
    }

    /// <summary>
    /// 移動する花輪の設定
    /// </summary>
    public void SetUpMovingFlowerCircle(bool isMove, bool isScaling) {

        // 移動する花輪か、通常の花輪かの設定
        isMoving = isMove;

        // 移動する場合
        if (isMoving) {

            // ランダムな移動時間や距離を使うか判定
            if (JudgeRandomMoving()) {

                // ランダムの場合には、移動時間と距離のランダム設定を行う
                RandomMoveOn();
            }
        }

        // 花輪の大きさを変更する場合
        if (isScaling) {

            // 大きさを変更
            ChangeScales();
        }
    }

    /// <summary>
    /// 移動時間と距離をランダムにするか判定
    /// </summary>
    /// <returns></returns>
    private bool JudgeRandomMoving() {
        return Random.Range(0, 100) <= randomMovingRatio;
    }

    /// <summary>
    /// ランダム値を取得して移動
    /// </summary>
    private void RandomMoveOn() {

        // 移動時間をランダム値の範囲で設定
        duration = Random.Range(durationRange.x, durationRange.y);

        // 移動距離をランダム値の範囲で設定
        moveDistance = Random.Range(moveDistanceRange.x, moveDistanceRange.y);
    }

    /// <summary>
    /// 大きさを変更して点数に反映
    /// </summary>
    private void ChangeScales() {

        // ランダム値の範囲内で大きさを設定
        int index = Random.Range(0, flowerSizes.Length);

        // 大きさを変更
        transform.localScale *= flowerSizes[index];

        // 点数を変更
        point = Mathf.CeilToInt(point * pointRate[index]);
    }
}
