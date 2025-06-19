using UnityEngine;

[System.Serializable]
public class EffectSetting
{
    public GameObject prefab;

    [Header("�����ʒu")]
    public Transform offset;

    [Header("�Đ�����")]
    public float duration = 1f;

    [Header("�p�x�␳")]
    public Vector3 rotationEuler = Vector3.zero;
}

public class PlayerEffectController : MonoBehaviour
{
    [Header("�G�t�F�N�g�ݒ�")]
    [Header("�`���[�W�G�t�F�N�g")]
    public EffectSetting ChargeJumpEffect;

    [Header("�ΉԃG�t�F�N�g")]
    public EffectSetting SparkEffect;



    // �`���[�W�G�t�F�N�g�̃C���X�^���X
    private GameObject activeChargeEffect = null;

    // �`���[�W�G�t�F�N�g�̍Đ�
    public void StartChargeJumpEffect()
    {
        if (ChargeJumpEffect.prefab == null) return;

        Transform spawnRef = ChargeJumpEffect.offset != null ? ChargeJumpEffect.offset : transform;
        Quaternion rot = spawnRef.rotation * Quaternion.Euler(ChargeJumpEffect.rotationEuler);

        activeChargeEffect = Instantiate(ChargeJumpEffect.prefab, spawnRef.position, rot);
        activeChargeEffect.transform.parent = transform;
    }

    public void PlaySparkEffect(Vector3 direction)
    {
        if (SparkEffect.prefab == null) return;

        Transform spawnRef = SparkEffect.offset != null ? SparkEffect.offset : transform;
        Vector3 pos = spawnRef.position;

        Quaternion rot = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(SparkEffect.rotationEuler);

        GameObject fx = Instantiate(SparkEffect.prefab, pos, rot);
        Destroy(fx, SparkEffect.duration);
    }

    // ��{�G�t�F�N�g�Đ����W�b�N
    private void PlayEffect(EffectSetting setting, Vector3? direction = null)
    {
        if (setting.prefab == null) return;

        Transform spawnRef = setting.offset != null ? setting.offset : transform;
        Vector3 pos = spawnRef.position;
        Quaternion rot;

        if (direction != null)
        {
            rot = Quaternion.LookRotation(direction.Value) * Quaternion.Euler(setting.rotationEuler);
        }
        else
        {
            rot = spawnRef.rotation * Quaternion.Euler(setting.rotationEuler);
        }

        GameObject fx = Instantiate(setting.prefab, pos, rot);
        Destroy(fx, setting.duration);
    }
}
