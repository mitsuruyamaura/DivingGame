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

    [SerializeField,HideInInspector]
    private AudioClip flowerSE;

    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        //flowerObjs = GetAllChildren();
    }

    //private GameObject[] GetAllChildren() {
    //    GameObject[] objs = new GameObject[transform.childCount];
    //    for (int i = 0; i < transform.childCount; i++) {
    //        objs[i] = transform.GetChild(i).gameObject;
    //    }
    //    return objs;
    //}

    private void OnTriggerEnter(Collider other) {
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
        //AudioSource.PlayClipAtPoint(flowerSE, transform.position);


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
}
