using UnityEngine;

public class BlownAway : MonoBehaviour
{
    public float forceMultiplier = 2000.0f;
    public float maxHitStop = 2.0f;
    public float forceMultiplierY = 500.0f;
    public float ySpeedMultiplier = 100.0f;

    public float scaleIncreasePerHit = 1.1f; // ヒットごとにスケール10%アップ

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb == null) return;

            // プレイヤー速度を取得
            Vector3 playerVelocity = playerRb.linearVelocity;
            float speed = Mathf.Max(playerVelocity.magnitude, 1f) * 5f;

            // 押し返す方向（XZ）
            Vector3 flatDir = (transform.position - collision.transform.position);
            flatDir.y = 0;
            flatDir = flatDir.normalized;

            // Y方向成分
            float yForce = forceMultiplierY + (speed * ySpeedMultiplier) * 100f;

            // 最終的な力
            Vector3 force = (flatDir * speed * forceMultiplier) + (Vector3.up * yForce);
            rb.AddForce(force, ForceMode.Impulse);

            // スケールアップ（累積）
            transform.localScale *= scaleIncreasePerHit;

            // スケールに応じてヒットストップ時間を強調（例：scale 1.5 → 1.5倍）
            float scaleFactor = transform.localScale.magnitude / new Vector3(1, 1, 1).magnitude; // 元スケールとの比率
            float stopTime = Mathf.Clamp(speed * 0.15f * scaleFactor, 0.5f, maxHitStop);

            StartCoroutine(HitStop(stopTime));

            Debug.Log($"Hit! Speed: {speed}, Scale: {transform.localScale}, HitStop: {stopTime}");
        }
    }

    System.Collections.IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;

        Vector3 originalPosition = transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            float shakeStrength = 0.05f;
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        Time.timeScale = 1f;
    }
}
