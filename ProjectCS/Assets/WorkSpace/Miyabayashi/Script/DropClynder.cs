using UnityEngine;

public class DropClynder : MonoBehaviour
{
    public Transform target;    // �v���C���[�Ȃ�
    public float minY = 0f;     // �X�P�[�����O�J�n�̍���
    public float maxY = 10f;    // �X�P�[�����O�ő�̍���
    public float minRadius = 0.1f; // �ŏ����a
    public float maxRadius = 1.0f; // �ő唼�a

    void Update()
    {
        if (target == null) return;

        float height = Mathf.Max(0, target.position.y); // ������0�����ɂȂ�Ȃ��悤��

        // ���a����
        float t = Mathf.InverseLerp(minY, maxY, height);
        float radius = Mathf.Lerp(minRadius, maxRadius, t);

        // �~���̃X�P�[���ݒ�iY�͍����AX��Z�͔��a�j
        transform.localScale = new Vector3(radius, height / 2f, radius);

        // �~���̈ʒu�ݒ�iY�͍����̔����̈ʒu�����S�j
        transform.position = new Vector3(
            target.position.x,
            height / 2f,
            target.position.z
        );
    }
}
