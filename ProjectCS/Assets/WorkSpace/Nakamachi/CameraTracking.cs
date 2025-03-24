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
    private GameObject player;
    public float yOffset; //y軸方向のオフセット
    public float zOffset; //z軸方向のオフセット

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.position.z;
        transform.position = new Vector3(x, y + yOffset, z + zOffset);
    }
}