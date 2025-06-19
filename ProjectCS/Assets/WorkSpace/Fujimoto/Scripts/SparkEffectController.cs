using UnityEngine;

public class SparkEffectController : MonoBehaviour
{
    [Header("�v���C���[�̑��x���擾���邽�߂�Rigidbody")]
    public Rigidbody playerRigidbody;

    [Header("�ΉԂ̏o���Ԋu�i�ŏ��E�ő�j")]
    public float maxInterval = 0.5f;  // �ᑬ��
    public float minInterval = 0.05f; // ������

    [Header("���x�͈̔�")]
    public float minSpeed = 40f;
    public float maxSpeed = 100f;

    [Header("�G�t�F�N�g")]
    [SerializeField] private PlayerEffectController playerEffectController;

    private float timer = 0f;
    private float currentInterval = 0.5f;

    void Update()
    {
        if (playerRigidbody == null || playerEffectController == null) return;

        float speed = playerRigidbody.linearVelocity.magnitude;

        // �X�s�[�h�ɉ����ďo���Ԋu����
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
