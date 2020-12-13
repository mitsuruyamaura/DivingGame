﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleFlower : MonoBehaviour
{
    private Animator anim;
    private BoxCollider boxCol;

    void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col) {

        // 指定されたタグのゲームオブジェクトが侵入しても判定を行わない
        if(col.gameObject.tag == "Water" || col.gameObject.tag == "FlowerCircle") {
            return;
        }

        // 食べる
        StartCoroutine(EatingTarget(col.gameObject.GetComponent<Player>()));
    }

    /// <summary>
    /// 対象を食べて吐き出す
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private IEnumerator EatingTarget(Player player) {
        // コライダーをオフにして重複判定を防止する
        boxCol.enabled = false;

        // キャラを口の中央に移動させる
        player.transform.SetParent(transform);
        player.transform.localPosition = new Vector3(0, -2.0f, 0);
        player.transform.SetParent(null);
        
        // 食べるアニメ再生
        anim.SetTrigger("attack");

        // キャラの移動を一時停止し、キー入力を受け付けない状態にする
        player.StopMove();
               
　　　　// 食べているアニメの時間の間だけ処理を中断
        yield return new WaitForSeconds(0.75f);

        // キャラの移動をできる状態に戻す
        player.ResumeMove();

        // スコアを半分にする
        player.HalveScore();

        // キャラを上空に吐き出す
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 300, ForceMode.Impulse);

        // 小さくなりながら消す
        transform.DOScale(Vector3.zero, 0.5f);
        Destroy(gameObject, 0.5f);
    }
}