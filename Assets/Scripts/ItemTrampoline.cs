using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemTrampoline : MonoBehaviour
{
    private BoxCollider boxCol;

    [SerializeField, Header("跳ねたときの空気抵抗値")]
    private float airResistance;

    void Start() {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col) {
        // 指定されたタグのゲームオブジェクトが接触した場合には、判定を行わない
        if (col.gameObject.tag == "Water" || col.gameObject.tag == "FlowerCircle") {
            return;
        }

        // 侵入してきたゲームオブジェクトが PlayerController スクリプトを持っていたら取得
        if (col.gameObject.TryGetComponent(out Player player)) {
            Bound(player);
        }
    }

    /// <summary>
    /// バウンドさせる
    /// </summary>
    /// <param name="player"></param>
    private void Bound(Player player) {

        // コライダーをオフにして重複判定を防止する
        boxCol.enabled = false;

        // キャラを上空にバウンドさせる(操作は可能)
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(800, 1000), ForceMode.Impulse);

        // キャラを回転させる
        player.transform.DORotate(new Vector3(90, 1080, 0), 1.5f, RotateMode.FastBeyond360)
            .OnComplete(() => {
                // しばらくの間、落下速度をゆっくりにする
                player.DampingDrag(airResistance);
            });
        
        Destroy(gameObject);
    }
}
