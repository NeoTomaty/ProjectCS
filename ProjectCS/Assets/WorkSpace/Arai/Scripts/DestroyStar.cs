//======================================================
// [DestroyStar]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/12
// 
// [Log]
// 05/12�@�r��@���̃I�u�W�F�N�g���Ԃ����Ă����玩�������ł�����悤�Ɏ���
// 05/12�@�r��@�G�t�F�N�g���o���悤�Ɏ���
//======================================================
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    [SerializeField] private GameObject Effect; // �G�t�F�N�g�I�u�W�F�N�g

    private void OnTriggerEnter(Collider other)
    {
        // �G�t�F�N�g���o��
        if (Effect != null)
        {
            GameObject EffectInstance = Instantiate(Effect, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(EffectInstance, EffectInstance.GetComponent<ParticleSystem>().main.duration);   // �G�t�F�N�g�̌p�����Ԃ��I�������ɏ���
        }

        Destroy(gameObject);
    }
}
