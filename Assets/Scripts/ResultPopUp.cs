using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnRetry;

    [SerializeField]
    private CanvasGroup canvasGroupTxt;

    [SerializeField]
    private CanvasGroup canvasGroupPopUp;

    [SerializeField]
    private Image imgTitle;


    void Start() {
        // リザルトを非表示にしておく
        canvasGroupPopUp.alpha = 0;
        canvasGroupTxt.alpha = 0;

        // ボタンにメソッドを登録
        btnRetry.onClick.AddListener(OnClickRetry);

        // ボタンを切っておく
        btnRetry.interactable = false;
    }

    /// <summary>
    /// リザルト表示を行う
    /// </summary>
    public void DisplayResult() {
        // CanvasGroup のアルファを変更してリザルト表示
        canvasGroupPopUp.DOFade(1.0f, 1.0f).OnComplete(() => { btnRetry.interactable = true; }); ;


        // タイトルの大きさを変更
        Vector3 scale = imgTitle.transform.localScale;

        imgTitle.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.0f);
        //sequence.Append(canvasGroupPopUp.DOFade(1.0f, 1.0f));
        sequence.Append(imgTitle.transform.DOScale(1.5f, 0.25f));
        sequence.Append(imgTitle.transform.DOScale(scale, 0.15f));
        //sequence.AppendInterval(0.5f);
        //sequence.Append(canvasGroupTxt.DOFade(1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo));
        //sequence.AppendCallback(() => { btnRetry.interactable = true; });

        // リトライの文字を点滅
        //canvasGroupTxt.DOFade(0, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        //btnRetry.interactable = true;
        canvasGroupTxt.DOFade(1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// リザルトをタップした際の処理
    /// </summary>
    private void OnClickRetry() {
        StartCoroutine(Retry());
    }

    /// <summary>
    /// リトライ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Retry() {

        // リザルトを徐々に非表示にする
        canvasGroupPopUp.DOFade(0, 1.0f);

        // リザルトが非表示になるまで待機
        yield return new WaitForSeconds(1.0f);

        // 現在と同じシーンを読み込む
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
