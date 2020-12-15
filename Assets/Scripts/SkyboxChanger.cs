using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField]
    private Material[] skyboxMaterials;

    [SerializeField]
    private int skyboxNum;   // 999 の場合、ランダムにする

    /// <summary>
    /// Skyboxを変更
    /// </summary>
    /// <param name="index"></param>
    public void ChangeSkybox() {

        // ランダム設定の場合
        if (skyboxNum == 999) {

            // Skybox を指定された要素番号のマテリアルの Skybox に変更
            RenderSettings.skybox = skyboxMaterials[RandomSelectIndexOfSkyboxMaterials()];
        } else {

            // Skybox を指定された要素番号のマテリアルの Skybox に変更
            RenderSettings.skybox = skyboxMaterials[skyboxNum];
        }

        Debug.Log("Skybox 変更");
    }

    /// <summary>
    /// Skyboxのランダムな要素番号を取得
    /// </summary>
    /// <returns></returns>
    public int RandomSelectIndexOfSkyboxMaterials() {
        return Random.Range(0, skyboxMaterials.Length);
    }
}
