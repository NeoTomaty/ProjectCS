using UnityEngine;

public class SparkEffectController : MonoBehaviour
{
    [Header("プレイヤーの速度を取得するためのRigidbody")]
    public Rigidbody playerRigidbody;

    [Header("火花の出現間隔（最小・最大）")]
    public float maxInterval = 0.5f;  // 低速時
    public float minInterval = 0.05f; // 高速時

    [Header("速度の範囲")]
    public float minSpeed = 40f;
    public float maxSpeed = 100f;

    [Header("エフェクト")]
    [SerializeField] private PlayerEffectController playerEffectController;

    private float timer = 0f;
    private float currentInterval = 0.5f;

    void Update()
    {
        if (playerRigidbody == null || playerEffectController == null) return;

        float speed = playerRigidbody.linearVelocity.magnitude;

        // スピードに応じて出現間隔を補間
        currentInterval = Mathf.Lerp(maxInterval, minInterval, Mathf.InverseLerp(minSpeed, maxSpeed, speed));

        timer += Time.deltaTime;
        if (timer >= currentInterval && speed > minSpeed)
        {
            Vector3 direction = playerRigidbody.linearVelocity.normalized;
            playerEffectController.PlaySparkEffect(direction);
            timer = 0f;
        }
    }
}
