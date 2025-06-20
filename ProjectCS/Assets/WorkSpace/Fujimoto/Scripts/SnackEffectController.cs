using UnityEngine;


public class SnackEffectController : MonoBehaviour
{
    [Header("Snack������уG�t�F�N�g�ݒ�")]
    public EffectSetting SnackFlyingEffect;

    // �`���[�W�G�t�F�N�g�̃C���X�^���X
    private GameObject activeFlyingEffect = null;

    public void PlayFlyingEffect()
    {
        
        if (SnackFlyingEffect == null || activeFlyingEffect != null) return;

        Transform spawnRef = SnackFlyingEffect.offset != null ? SnackFlyingEffect.offset : transform;
        Quaternion rot = spawnRef.rotation * Quaternion.Euler(SnackFlyingEffect.rotationEuler);

        activeFlyingEffect = Instantiate(SnackFlyingEffect.prefab, spawnRef.position, rot);
        activeFlyingEffect.transform.parent = transform;
        
    }

    public void StopFlyingEffect()
    {
        if (activeFlyingEffect != null)
        {
            Destroy(activeFlyingEffect);
            activeFlyingEffect = null;
        }
    }
}
