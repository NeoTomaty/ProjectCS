using UnityEngine;




public class DropProjectorController : MonoBehaviour
{
    public Transform targetObject;       // 蹴り飛ばすオブジェクト
    public float baseSize = 1f;
    public float sizeMultiplier = 0.1f;
    public float groundY = 0f;           // 地面のY座標
    public float projectorOffsetY = -0.05f;  // 地面より少し下
    public float minYThreshold = 0.2f;   // これより下ならprojectorをオフ

    private Projector projector;

    void Start()
    {
        projector = GetComponent<Projector>();
    }

    void Update()
    {
        if (targetObject == null || projector == null) return;

        float targetY = targetObject.position.y;

        // Y座標が一定より下ならProjectorを無効化
        if (targetY < minYThreshold)
        {
            projector.enabled = false;
            return; // それ以上処理しない
        }
        else
        {
            projector.enabled = true;
        }

        // Projectorの位置（XZ追従、Yは固定）
        Vector3 pos = targetObject.position;
        pos.y = groundY + projectorOffsetY;
        transform.position = pos;

        // 回転固定（真下）
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // 高さに応じてサイズ調整
        float height = Mathf.Max(0.01f, targetY - groundY);
        projector.orthographicSize = baseSize + height * sizeMultiplier;
    }
}