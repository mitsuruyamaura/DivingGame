using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField]
    private Material[] skyboxMaterials;

    /// <summary>
    /// Skyboxを変更
    /// </summary>
    /// <param name="index"></param>
    public void ChangeSkybox(int index) {

        // Skybox を指定された要素番号のマテリアルの Skybox に変更
        RenderSettings.skybox = skyboxMaterials[index];
    }

    /// <summary>
    /// Skyboxのランダムな要素番号を取得
    /// </summary>
    /// <returns></returns>
    public int RandomSelectIndexOfSkyboxMaterials() {
        return Random.Range(0, skyboxMaterials.Length);
    }
}
