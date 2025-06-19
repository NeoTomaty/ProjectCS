using UnityEngine;

[System.Serializable]
public class EffectSetting
{
    public GameObject prefab;

    [Header("発生位置")]
    public Transform offset;

    [Header("再生時間")]
    public float duration = 1f;

    [Header("角度補正")]
    public Vector3 rotationEuler = Vector3.zero;
}

public class PlayerEffectController : MonoBehaviour
{
    [Header("エフェクト設定")]
    [Header("チャージエフェクト")]
    public EffectSetting ChargeJumpEffect;

    [Header("火花エフェクト")]
    public EffectSetting SparkEffect;



    // チャージエフェクトのインスタンス
    private GameObject activeChargeEffect = null;

    // チャージエフェクトの再生
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

    // 基本エフェクト再生ロジック
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
