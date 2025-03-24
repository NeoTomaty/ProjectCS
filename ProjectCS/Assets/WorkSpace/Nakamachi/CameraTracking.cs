//ファイル名:CameraTracking.cs
//作成者:中町雷我
//作成日:2025/03/24
//更新履歴:
//2025/03/24:初版作成
//概要:プレイヤーにカメラ追従機能を実装

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    //プレイヤーオブジェクトを格納する変数
    private GameObject player;

    //カメラのy軸方向のオフセット
    public float yOffset;

    //カメラのz軸方向のオフセット
    public float zOffset;

    // Start is called before the first frame update
    void Start()
    {
        //"Player"という名前のオブジェクトをシーンから探してplayerに格納
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの現在のx座標を取得
        float x = player.transform.position.x;

        //プレイヤーの現在のy座標を取得
        float y = player.transform.position.y;

        //プレイヤーの現在のz座標を取得
        float z = player.transform.position.z;

        //カメラの位置をプレイヤーの位置にオフセットを加えた位置に設定
        transform.position = new Vector3(x, y + yOffset, z + zOffset);
    }
}