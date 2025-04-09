using UnityEngine;

public class BlownAway : MonoBehaviour
{
    public float forceMultiplier = 2000.0f;
    public float maxHitStop = 1.0f;
    public float forceMultiplierY = 500.0f;
    public float ySpeedMultiplier = 100.0f;

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
            Vector3 playerVelocity = playerRb.linearVelocity; // linearVelocity → velocity に変更
            float speed = Mathf.Max(playerVelocity.magnitude, 1f) * 5f;

            // 押し返す方向（XZ）
            Vector3 flatDir = (transform.position - collision.transform.position);
            flatDir.y = 0;
            flatDir = flatDir.normalized;

            // Y方向成分（固定 + 速度依存）
            float yForce = forceMultiplierY + (speed * ySpeedMultiplier) * 100f;

            // 最終的な力を構成
            Vector3 force = (flatDir * speed * forceMultiplier) + (Vector3.up * yForce);

            rb.AddForce(force, ForceMode.Impulse);

            float stopTime = Mathf.Clamp(speed * 0.15f, 0.5f, maxHitStop);
            StartCoroutine(HitStop(speed));

            Debug.Log("Hit! Speed: " + speed + " → Force: " + force);
        }
    }

    System.Collections.IEnumerator HitStop(float speed)
    {
        Time.timeScale = 0f;

        float normalizedSpeed = Mathf.InverseLerp(1f, 20f, speed);
        float duration = Mathf.Lerp(0.5f, maxHitStop, normalizedSpeed);

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
