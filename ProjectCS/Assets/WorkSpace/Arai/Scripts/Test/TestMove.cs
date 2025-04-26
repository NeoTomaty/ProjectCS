using UnityEngine;

// �e�X�g�p�̈ړ��X�N���v�g
public class TestMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;

    private Vector3 MoveVector;

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
    }

    public Vector3 GetMoveVector()
    {
        return MoveVector;
    }

    public void SetMoveVector(Vector3 moveVector)
    {
        MoveVector = moveVector;
        MoveVector.Normalize(); // ���K�����ĕ����x�N�g����ݒ�
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveVector = Vector3.zero; // ������
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(MoveVector * MoveSpeed * Time.deltaTime);
    }
}
