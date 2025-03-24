//ファイル名:PlayerMoveUp.cs
//作成者:中町雷我
//作成日:2025/03/24
//更新履歴:
//2025/03/24:初版作成
//概要:プレイヤーをWASDで動かし、壁に当たると加速していく機能を実装

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUp : MonoBehaviour
{
    //プレイヤーの移動速度を設定する変数
    public float speed = 5.0f;

    //初期速度を保存する変数
    private float initialSpeed;

    //衝突状態を管理する変数
    private bool collided = false;

    void Start()
    {
        //初期速度を保存
        initialSpeed = speed;
    }

    void Update()
    {
        //キーボードとコントローラーの水平方向の入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal");

        //キーボードとコントローラーの垂直方向の入力を取得
        float moveVertical = Input.GetAxis("Vertical");

        //移動ベクトルを作成
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //プレイヤーを移動させる
        transform.Translate(movement * speed * Time.deltaTime);
    }

    //衝突が発生したときに呼び出される
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトのタグがWallのとき
        if (collision.gameObject.tag == "Wall")
        {
            //衝突状態をtrueに設定
            collided = true;
        }
    }

    //衝突が終了したときに呼び出される
    void OnCollisionExit(Collision collision)
    {
        //衝突したオブジェクトのタグがWallで、衝突状態がtrueのとき
        if (collision.gameObject.tag == "Wall" && collided)
        {
            //移動速度を10.0f増加させる
            speed += 10.0f;

            //衝突状態をfalseに設定
            collided = false;
        }
    }
}