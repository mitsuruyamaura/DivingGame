using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircleRotater : MonoBehaviour
{
    public int point;

    private GameObject[] flowerObjs;

    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        flowerObjs = GetAllChildren();
    }

    private GameObject[] GetAllChildren() {
        GameObject[] objs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            objs[i] = transform.GetChild(i).gameObject;
        }
        return objs;
    }

    private void OnTriggerEnter(Collider other) {
        // エフェクト

        for (int i = 0; i < flowerObjs.Length; i++) {
            flowerObjs[i].transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => { Destroy(flowerObjs[i]); });
        }
    }
}
