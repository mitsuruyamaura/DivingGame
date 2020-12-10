using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Header("BGM用オーディオファイル")]
    private AudioClip[] bgms = null;

    private AudioSource audioSource;         // BGM再生用コンポーネント


    public enum BgmType {
        Main,          // 0
        GameClear      // 1
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ゲーム中のBGMを再生
        PlayBGM(BgmType.Main);
    }

    /// <summary>
    /// 指定した種類のBGMを再生
    /// </summary>
    /// <param name="bgmType"></param>
    public void PlayBGM(BgmType bgmType) {
        // BGM停止
        audioSource.Stop();

        // 再生するBGMを設定する
        audioSource.clip = bgms[(int)bgmType];

        // BGM再生
        audioSource.Play();

        Debug.Log("再生中のBGM : " + bgmType);
    }
}
