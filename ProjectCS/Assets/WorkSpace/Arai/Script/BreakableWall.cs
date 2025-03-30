// 作成者:荒井修
// 作成日:2025/03/28
// 概要:破壊可能な壁のスクリプト
// 更新履歴:
// 2025//:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    // 加速量
    [SerializeField] private float CharacterAcceleration = 10.0f;
    private bool Breakable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーが壁に当たったときの処理
            Debug.Log("破壊可能な壁 >> プレイヤーが壁に当たりました");

            // プレイヤーを跳ね返す

            // プレイヤーを素通りさせる

            // 破壊される

            // プレイヤーを加速させる
        }
    }
}
