//======================================================
// [TestMovePlayer]
// 作成者：荒井修
// 最終更新日：03/31
// 
// [Log]
// 3/31　荒井　入力を受け付けない移動テストを作成
//======================================================

using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    // プレイヤーの移動速度
    public float speed = 10.0f;

    // プレイヤーの移動方向
    public Vector3 moveDirection = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 決まった方向に移動し続ける
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
