using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �ړ����x

    void Update()
    {
        // ���͂��擾
        float horizontal = Input.GetAxis("Horizontal"); // A��D�L�[
        float vertical = Input.GetAxis("Vertical"); // W��S�L�[

        // �ړ��x�N�g�����v�Z
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // �v���C���[���ړ�
        transform.Translate(movement, Space.World);
    }
}