//======================================================
// [スクリプト名]PlayerDeceleration
// 作成者：宮林朋輝
// 最終更新日：3/31
// 
// [Log]
//======================================================
using UnityEngine;


public class PlayerDeceleration : MonoBehaviour
{
    //プレイヤーの移動速度を設定する変数
    public float Deceleration = 5.0f;//減速量

    float CurrentSpeed;//現在の速度

    private bool isHoldingKey = false;
    private float HoldTime = 0.0f;
    public float DecelerationsInterval = 1.0f; // 減速間隔


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ここでプレイヤーの初期速度を受け取る

    }

    // Update is called once per frame
    void Update()
    {
        //ここで現在の速度を受け取る


        if (Input.GetKeyDown(KeyCode.S))
        {
            // キーを押した瞬間の処理
            if (CurrentSpeed > Deceleration)
            {
                CurrentSpeed -= Deceleration;//減速
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (!isHoldingKey)
            {
                // 長押し開始時の処理
                isHoldingKey = true;
                HoldTime = 0.0f; // 初期化
            }

            HoldTime += Time.deltaTime;

            if (HoldTime >= DecelerationsInterval)
            {
                if (CurrentSpeed > Deceleration)
                {
                    CurrentSpeed -= Deceleration;
                }
                HoldTime = 0.0f; // 減速後にリセット
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            // キーを離した時の処理
            isHoldingKey = false;
        }

    }
}
