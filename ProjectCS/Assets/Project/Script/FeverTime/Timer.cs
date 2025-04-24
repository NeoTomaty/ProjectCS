//Timer.cs
//作成者:中町雷我
//アタッチ:UIテキストにアタッチ
//最終更新日:2025/04/23
//[Log]
//04/23　中町　フィーバータイムのタイムを追加

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //タイマー表示用のUIテキスト
    public Text TimerText;

    //経過時間を保持する変数
    public float TimeElapsed = 0.0f;

    //フィーバータイム中かどうかを示すフラグ
    private bool IsFeverTime = false;

    void Update()
    {
        //経過時間を増加させる
        TimeElapsed += Time.deltaTime;

        //経過時間を表示する
        DisplayTime(TimeElapsed);

        //2分40秒(160秒)経過後にフィーバータイムを開始する(仮で20秒)
        if (TimeElapsed >= 20.0f && !IsFeverTime)
        {
            StartFeverTime();
        }
    }

    //経過時間を分と秒の形式で表示する関数
    void DisplayTime(float TimeToDisplay)
    {
        float Minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float Seconds = Mathf.FloorToInt(TimeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
    }

    //フィーバータイムを開始する関数
    void StartFeverTime()
    {
        IsFeverTime = true;

        //フィーバータイムの開始処理を追加
        FindAnyObjectByType<SphereSpawner>().StartSpawning();

        //20秒後にフィーバータイムを終了する
        Invoke("EndFeverTime", 20.0f);
    }

    //フィーバータイムを終了する関数
    void EndFeverTime()
    {
        IsFeverTime = false;

        //フィーバータイムの終了処理を追加
        FindAnyObjectByType<SphereSpawner>().StopSpawning();

        //すべてのSphereオブジェクトを削除する
        RemoveAllSpheres();
    }

    //すべてのSphereオブジェクトを削除する
    void RemoveAllSpheres()
    {
        //Sphereタグが付いたすべてのオブジェクトを取得
        GameObject[] Spheres = GameObject.FindGameObjectsWithTag("Sphere");

        //取得したすべてのオブジェクトを削除
        foreach (GameObject Sphere in Spheres)
        {
            Destroy(Sphere);
        }

        //SphereSpawnerに再生成を防ぐフラグを設定
        FindAnyObjectByType<SphereSpawner>().DisableSpawning();
    }
}
