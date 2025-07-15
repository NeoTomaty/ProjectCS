//====================================================
// スクリプト名：SnackLauncher
// 作成者：藤本
// 
// [Log]
// 05/07 藤本　カウントダウン終了後にSnackを打ち上げる処理実装
//====================================================

using UnityEngine;
using System.Collections;

public class SnackLauncher : MonoBehaviour
{
    private BlownAway_Ver3 BAV3;

    void Start()
    {
        BAV3 = GetComponent<BlownAway_Ver3>();

        if (!gameObject.name.EndsWith("(Clone)"))
        {
            BAV3.enabled = false;
        }
    }

    // Snackを打ち上げる関数
    public void Launch()
    {
        StartCoroutine(EnableAndLaunch());
    }

    private IEnumerator EnableAndLaunch()
    {
        BAV3.enabled = true;

        yield return null; // ここで1フレーム待つ

        BAV3.MoveToRandomXZInRespawnArea();
        BAV3.Launch();
        BAV3.PlayLaunchEffect();
        Debug.Log("Snackを打ち上げました！");
    }
}
