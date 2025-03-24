using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //移動速度を設定する変数
    public float speed = 5.0f;

    void Update()
    {
        //Wキーが押されているとき、前方に移動
        if (Input.GetKey(KeyCode.W))
        {
            //プレイヤーの位置を前方方向に移動
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        //Sキーが押されているとき、後方に移動
        if (Input.GetKey(KeyCode.S))
        {
            //プレイヤーの位置を後方方向に移動
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        //Dキーが押されているとき、右方向に移動
        if (Input.GetKey(KeyCode.D))
        {
            //プレイヤーの位置を右方向に移動
            transform.position += speed * transform.right * Time.deltaTime;
        }

        //Aキーが押されているとき、左方向に移動
        if (Input.GetKey(KeyCode.A))
        {
            //プレイヤーの位置を左方向に移動
            transform.position -= speed * transform.right * Time.deltaTime;
        }
    }
}