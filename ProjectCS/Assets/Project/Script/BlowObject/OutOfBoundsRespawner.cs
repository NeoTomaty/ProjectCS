//======================================================
// [OutOfBoundsRespawner]
// 作成者：荒井修
// 最終更新日：04/22
// 
// [Log]
// 04/22　荒井　ステージ外に出た瞬間に自分自身の複製を生成するように実装
//======================================================
using UnityEngine;

// 範囲外に出たオブジェクトをリスポーンさせるクラス
// ステージが回転していない四角形範囲であることを前提とする
// 飛ばすオブジェクトにアタッチ
public class OutOfBoundsRespawner : MonoBehaviour
{
    [SerializeField] private Vector3 RespawnPosition;   // リスポーン位置
    private GameObject RespawnObject;                   // リスポーン対象

    [SerializeField] private GameObject Stage;  // ステージ
    private Rect StageRect;                     // ステージの判定矩形

    private bool IsRespawnComplete = false; // リスポーン完了フラグ

    // ステージ内判定
    private bool IsInsideStage()
    {
        if(Stage == null) return false;

        // リスポーン対象の座標を取得
        Vector3 ObjectPosition = RespawnObject.transform.position;
        Vector2 ObjectPosition2D = new Vector2(ObjectPosition.x, ObjectPosition.z); // 2D座標に変換

        // ステージの範囲内にいるか判定
        if (StageRect.Contains(ObjectPosition2D))
        {
            return true; // ステージ内
        }
        else
        {
            return false; // ステージ外
        }
    }

    // リスポーン処理
    private void Respawn()
    {
        // リスポーン対象を複製
        GameObject RespawnedObject = Instantiate(RespawnObject, RespawnPosition, Quaternion.identity);

        // リスポーン完了フラグを立てる
        IsRespawnComplete = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Stage == null) return;

        // リスポーン対象を自身に設定
        RespawnObject = this.gameObject;

        // ステージの座標とサイズを取得
        Vector3 StagePosition = Stage.transform.position;
        Vector3 StageSize = Stage.GetComponent<Renderer>().bounds.size;

        // ステージの判定矩形を作成
        float halfWidth = StageSize.x / 2;
        float halfHeight = StageSize.z / 2;
        StageRect = new Rect(StagePosition.x - halfWidth, StagePosition.z - halfHeight, StageSize.x, StageSize.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRespawnComplete) return;

        if (!IsInsideStage())
        {
            Respawn(); // ステージ外に出たらリスポーン
        }
    }
}
