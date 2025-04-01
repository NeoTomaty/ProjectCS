//スクリプト名:PlayerMoveUp.cs
//作成者:中町雷我
//最終更新日:2025/04/01
//[Log]
//2025/03/24　中町雷我　プレイヤーをWASDで動かし、壁に当たると加速していく機能を実装
//2025/03/31　中町雷我　壁に当たると加速量が+5ずつあがる
//2025/04/01　中町雷我　プレイヤーの最大速度を100以上にしない処理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUp : MonoBehaviour
{
    //プレイヤーの移動速度を設定する変数
    public float Speed = 5.0f;

    //プレイヤーの最大速度を設定する変数
    public float MaxSpeed = 100.0f;

    //初期速度を保存する変数
    private float InitialSpeed;

    //衝突状態を管理する変数
    private bool Collided = false;

    void Start()
    {
        //初期速度を保存
        InitialSpeed = Speed;

        //初期速度をデバッグログに出力
        Debug.Log("初期速度: " + Speed);
    }

    void Update()
    {
        //キーボードとコントローラーの水平方向の入力を取得
        float MoveHorizontal = Input.GetAxis("Horizontal");

        //キーボードとコントローラーの垂直方向の入力を取得
        float MoveVertical = Input.GetAxis("Vertical");

        //移動ベクトルを作成
        Vector3 Movement = new Vector3(MoveHorizontal, 0.0f, MoveVertical);

        //プレイヤーを移動させる
        transform.Translate(Movement * Speed * Time.deltaTime);
    }

    //衝突が発生したときに呼び出される
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトのタグがWallのとき
        if (collision.gameObject.tag == "Wall")
        {
            //衝突状態をtrueに設定
            Collided = true;

            //壁に衝突したことをデバッグログに出力
            Debug.Log("壁に衝突");
        }
    }

    //衝突が終了したときに呼び出される
    void OnCollisionExit(Collision collision)
    {
        //衝突したオブジェクトのタグがWallで、衝突状態がtrueのとき
        if (collision.gameObject.tag == "Wall" && Collided)
        {
            //移動速度を5.0f増加させるが、100を超えないように制限
            Speed = Mathf.Min(Speed + 5.0f, MaxSpeed);

            //加速量をデバッグログに出力
            Debug.Log("加速量: " + Speed);

            //衝突状態をfalseに設定
            Collided = false;
        }
    }
}