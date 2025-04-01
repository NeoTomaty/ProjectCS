//スクリプト名:SpeedManager.cs
//作成者:中町雷我
//最終更新日:2025/04/01
//[Log]
//2025/04/01　中町雷我　プレイヤーの最大速度を100以上にしない処理

using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    //SpeedManagerのインスタンスを保持する静的プロパティ
    public static SpeedManager Instance { get; private set; }

    //プレイヤーの最大速度を設定する変数
    public float MaxSpeed = 100.0f;

    //Awakeメソッドはオブジェクトが有効になるときに呼び出される
    void Awake()
    {
        //インスタンスがまだ設定されていないとき
        if(Instance == null)
        {
            //このオブジェクトをインスタンスとして設定
            Instance = this;

            //このオブジェクトをシーン間で破棄されないように設定
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //すでにインスタンスが存在するとき、このオブジェクトを破棄
            Destroy(gameObject);
        }
    }

    //プレイヤーの速度を増加させるメソッド
    public void IncreaseSpeed(ref float Speed)
    {
        //現在の速度に5.0fを加算し、最大速度を超えないように制限
        Speed = Mathf.Min(Speed + 5.0f, MaxSpeed);
        Debug.Log("加速量:" + Speed);
    }
}