using UnityEngine;

public class DropClynder : MonoBehaviour
{
    public Transform target;    // プレイヤーなど
    public float minY = 0f;     // スケーリング開始の高さ
    public float maxY = 10f;    // スケーリング最大の高さ
    public float minRadius = 0.1f; // 最小半径
    public float maxRadius = 1.0f; // 最大半径

    void Update()
    {
        if (target == null) return;

        float height = Mathf.Max(0, target.position.y); // 高さが0未満にならないように

        // 半径を補間
        float t = Mathf.InverseLerp(minY, maxY, height);
        float radius = Mathf.Lerp(minRadius, maxRadius, t);

        // 円柱のスケール設定（Yは高さ、XとZは半径）
        transform.localScale = new Vector3(radius, height / 2f, radius);

        // 円柱の位置設定（Yは高さの半分の位置＝中心）
        transform.position = new Vector3(
            target.position.x,
            height / 2f,
            target.position.z
        );
    }
}
