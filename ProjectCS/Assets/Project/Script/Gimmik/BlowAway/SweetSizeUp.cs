//====================================================
// スクリプト名：SweetSizeUp
// 作成者：髙下
// 内容：お菓子のサイズを大きくする
// 最終更新日：04/13
// 
// [Log]
// 04/13 高下 スクリプト作成
// 04/15 竹内 初期サイズの修正（サイズアップを倍率に）
// 
//====================================================
using UnityEngine;
using UnityEngine.UIElements;

public class SweetSizeUp : MonoBehaviour
{
    [SerializeField]
    private float MaxSize = 50.0f;      // お菓子の最大サイズ
    [SerializeField]
    private float MinSize = 10.0f;      // お菓子の最小（初期）サイズ
    [SerializeField]
    private float SizeUpAmount = 10.0f; // 一度のサイズアップ倍率
    [SerializeField]
    private float ColliderSizeMultiplier = 1.0f; // 当たり判定のサイズ倍率



    void Start()
    {
        // 初期サイズの決定
        Vector3 scale = transform.localScale;
        scale *= MinSize;

        BoxCollider Box = GetComponent<BoxCollider>(); // ボックスコライダー取得
        if(!Box)
        {
            Debug.LogError("BoxColliderがアタッチされていません");
        }
        Box.size *= ColliderSizeMultiplier; // 当たり判定のサイズを設定
    }
   
    // オブジェクトを大きくする関数
    public void ScaleUpSweet()
    {
        // 現在のスケール
        Vector3 CurrentScale = transform.localScale;

        // スケールアップ後のサイズ（各軸ごとに計算）
        Vector3 NewScale = CurrentScale * SizeUpAmount;

        // MaxSize を超えないように制限
        NewScale.x = Mathf.Min(NewScale.x, MaxSize);
        NewScale.y = Mathf.Min(NewScale.y, MaxSize);
        NewScale.z = Mathf.Min(NewScale.z, MaxSize);

        // 最終的なスケールを設定
        transform.localScale = NewScale;
    }

    // 現在の大きさの割合を取得
    public float GetScaleRatio()
    {
        return Mathf.Clamp01((transform.localScale.x - MinSize) / (MaxSize - MinSize));
    }
}
