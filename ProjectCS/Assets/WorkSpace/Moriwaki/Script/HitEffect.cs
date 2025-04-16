//======================================================
// HitEffect
// �쐬�ҁF�X�e
// �ŏI�X�V���F4/16
//
// [Log]4/16 �X�e�@�q�b�g�G�t�F�N�g�p�X�N���v�g�̍쐬
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
                Vector3 randomDir = Random.onUnitSphere; // �S���������_��
                rb.AddForce(randomDir * scatterForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * scatterForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject, lifetime);
    }
}