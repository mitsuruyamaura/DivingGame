using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemTrampoline : MonoBehaviour
{
    private BoxCollider boxCol;

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
            StartCoroutine(Bound(player));
        }
    }

    /// <summary>
    /// バウンドさせる
    /// </summary>
    /// <param name="player"></param>
    private IEnumerator Bound(Player player) {

        // コライダーをオフにして重複判定を防止する
        boxCol.enabled = false;

        // キャラを中央に移動させる
        //player.transform.SetParent(transform);
        //player.transform.localPosition = new Vector3(0, 2.5f, 0);
        //player.transform.SetParent(null);

        // キャラの移動を一時停止し、キー入力を受け付けない状態にする
        player.StopMove();

        yield return new WaitForSeconds(0.15f);

        // キャラの移動をできる状態に戻す
        player.ResumeMove();

        // キャラを上空にバウンドさせる
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(800, 1000), ForceMode.Impulse);

        // キャラを回転させる
        player.transform.DORotate(new Vector3(90, 0, 1080), 1.5f, RotateMode.FastBeyond360)
            .OnComplete(() => {
                //player.transform.DORotate(new Vector3(180, 0, 0), 0.25f);
                player.Damping();
                Destroy(gameObject, 0.5f);
            });
    }
}
