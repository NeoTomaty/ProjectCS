using UnityEngine;

public class DebugCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.01f;

    private bool isDragging = false;
    private Vector2 lastMousePos;

    private Camera cam;
    private Vector3 eye;
    private Vector3 focus;
    private Vector3 up = Vector3.up;

    void Start()
    {
        cam = GetComponent<Camera>();
        eye = transform.position;
        focus = transform.position + transform.forward;
    }

    void Update()
    {
        UpdateMouseInput();
        UpdateKeyInput(Time.deltaTime);

        // �z�C�[���őO��Y�[��
        float scroll = Input.mouseScrollDelta.y;
        if (!Input.GetMouseButton(2) && Mathf.Abs(scroll) > 0.01f)
        {
            ZoomCamera(scroll);
        }

        // ���{�^�������Ȃ���̃p���ړ�
        if (Input.GetMouseButton(2))
        {
            Vector2 panScroll = Input.mouseScrollDelta;
            if (Mathf.Abs(panScroll.x) > 0.01f || Mathf.Abs(panScroll.y) > 0.01f)
            {
                PanCamera(panScroll.x, panScroll.y);
            }
        }

        CalculateViewMatrix();
    }

    void UpdateMouseInput()
    {
        Vector2 mousePos = Input.mousePosition;

        // �E�N���b�N�ŉ�]
        if (Input.GetMouseButton(1))
        {
            if (!isDragging)
            {
                lastMousePos = mousePos;
                isDragging = true;
            }

            float dx = (mousePos.x - lastMousePos.x) * mouseSensitivity;
            float dy = (mousePos.y - lastMousePos.y) * mouseSensitivity;

            RotateCamera(dy, dx);

            lastMousePos = mousePos;
        }
        else if (Input.GetMouseButton(2)) // �����{�^���Ńp���ړ�
        {
            if (!isDragging)
            {
                lastMousePos = mousePos;
                isDragging = true;
            }

            float dx = (mousePos.x - lastMousePos.x) * mouseSensitivity;
            float dy = (mousePos.y - lastMousePos.y) * mouseSensitivity;

            PanCameraByDrag(dx, dy);

            lastMousePos = mousePos;
        }
        else
        {
            isDragging = false;
        }
    }

    void UpdateKeyInput(float deltaTime)
    {
        if (!isDragging) return;

        float realSpeed = moveSpeed * deltaTime;

        float x = 0f, y = 0f, z = 0f;

        if (Input.GetKey(KeyCode.W)) z += realSpeed;
        if (Input.GetKey(KeyCode.S)) z -= realSpeed;
        if (Input.GetKey(KeyCode.A)) x -= realSpeed;
        if (Input.GetKey(KeyCode.D)) x += realSpeed;
        if (Input.GetKey(KeyCode.Q)) y += realSpeed;
        if (Input.GetKey(KeyCode.E)) y -= realSpeed;

        MoveCamera(x, y, z);
    }

    void MoveCamera(float dx, float dy, float dz)
    {
        Vector3 look = (focus - eye).normalized;
        Vector3 right = Vector3.Cross(up, look).normalized;

        eye += right * dx;
        eye += up * dy;
        eye += look * dz;

        focus += right * dx;
        focus += up * dy;
        focus += look * dz;
    }

    void RotateCamera(float pitch, float yaw)
    {
        Vector3 look = (focus - eye).normalized;

        // ���E�iY���j��]
        Quaternion yawRot = Quaternion.AngleAxis(Mathf.Rad2Deg * yaw, up);
        look = yawRot * look;

        // �㉺�i�J�����̉E���j��]
        Vector3 right = Vector3.Cross(up, look).normalized;
        Quaternion pitchRot = Quaternion.AngleAxis(Mathf.Rad2Deg * pitch, right);
        look = pitchRot * look;

        focus = eye + look;
    }

    void CalculateViewMatrix()
    {
        transform.position = eye;
        transform.LookAt(focus, up);
    }

    void ZoomCamera(float scrollAmount)
    {
        Vector3 look = (focus - eye).normalized;
        float zoomSpeed = moveSpeed * 0.5f; // �X�N���[�����x����
        Vector3 delta = look * scrollAmount * zoomSpeed;

        eye += delta;
        focus += delta;
    }

    void PanCamera(float scrollX, float scrollY)
    {
        Vector3 look = (focus - eye).normalized;
        Vector3 right = Vector3.Cross(up, look).normalized;

        float panSpeed = moveSpeed * 0.3f; // �p���̑����͒����\

        Vector3 delta = (right * scrollX + up * scrollY) * panSpeed;

        eye += delta;
        focus += delta;
    }

    void PanCameraByDrag(float deltaX, float deltaY)
    {
        Vector3 look = (focus - eye).normalized;
        Vector3 right = Vector3.Cross(up, look).normalized;

        float panSpeed = moveSpeed * 0.5f; // �p�����x�̒����l

        Vector3 delta = (-right * deltaX + -up * deltaY) * panSpeed;

        eye += delta;
        focus += delta;
    }
}
