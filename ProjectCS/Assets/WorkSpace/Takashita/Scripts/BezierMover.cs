using UnityEngine;

public class BezierMover : MonoBehaviour
{
    public CubicBezierCurve curve;  // �Ȑ��X�N���v�g�i�x�W�F���j
    [Range(0.1f, 10f)]
    public float moveDuration = 3f; // �Ȑ����ړ����I����܂ł̕b��



    private float timer = 0f;

    void Update()
    {
        //if (curve == null) return;

        //// �o�ߎ��Ԃ��X�V
        //timer += Time.deltaTime;
        //float t = Mathf.Clamp01(timer / moveDuration); // 0�`1 �͈̔͂ɐ��K��

        //// �Ȑ���̈ʒu���擾���ăI�u�W�F�N�g���ړ�
        //Vector3 position = curve.CalculateBezierPoint(t);
        //transform.position = position;
    }
}
