//======================================================
// HitEffect
// 作成者：森脇
// 最終更新日：4/16
//
// [Log]4/16 森脇　ヒットエフェクト用スクリプトの作成
//======================================================

using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float lifetime = 2f;
    public float scatterForce = 5f;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomDir = Random.onUnitSphere; // 全方向ランダム
                rb.AddForce(randomDir * scatterForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * scatterForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject, lifetime);
    }
}