//======================================================
// TimeCounterスクリプト
// 作成者：宮林
// 最終更新日：4/20
// 
// [Log]4/20 宮林　タイマーの追加
//      4/22 宮林　シーン遷移の追加      
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCounter : MonoBehaviour
{
    public float countdown = 5.0f;
    public Text timeText;

    [Header("遷移先のシーン名")]
    public string nextSceneName;

    private bool hasTransitioned = false;

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown > 0)
        {
            timeText.text = countdown.ToString("f1") + "秒";
        }
        else
        {
            timeText.text = "0.00秒";

            if (!hasTransitioned)
            {
                hasTransitioned = true;

                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogWarning("次のシーン名が設定されていません！");
                }
            }
        }
    }
}
