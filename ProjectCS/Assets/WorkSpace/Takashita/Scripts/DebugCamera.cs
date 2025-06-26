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

        // ホイールで前後ズーム
        float scroll = Input.mouseScrollDelta.y;
        if (!Input.GetMouseButton(2) && Mathf.Abs(scroll) > 0.01f)
        {
            ZoomCamera(scroll);
        }

        // 中ボタン押しながらのパン移動
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

        // 右クリックで回転
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
        else if (Input.GetMouseButton(2)) // ★中ボタンでパン移動
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

        // 左右（Y軸）回転
        Quaternion yawRot = Quaternion.AngleAxis(Mathf.Rad2Deg * yaw, up);
        look = yawRot * look;

        // 上下（カメラの右軸）回転
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
        float zoomSpeed = moveSpeed * 0.5f; // スクロール感度調整
        Vector3 delta = look * scrollAmount * zoomSpeed;

        eye += delta;
        focus += delta;
    }

    void PanCamera(float scrollX, float scrollY)
    {
        Vector3 look = (focus - eye).normalized;
        Vector3 right = Vector3.Cross(up, look).normalized;

        float panSpeed = moveSpeed * 0.3f; // パンの速さは調整可能

        Vector3 delta = (right * scrollX + up * scrollY) * panSpeed;

        eye += delta;
        focus += delta;
    }

    void PanCameraByDrag(float deltaX, float deltaY)
    {
        Vector3 look = (focus - eye).normalized;
        Vector3 right = Vector3.Cross(up, look).normalized;

        float panSpeed = moveSpeed * 0.5f; // パン速度の調整値

        Vector3 delta = (-right * deltaX + -up * deltaY) * panSpeed;

        eye += delta;
        focus += delta;
    }
}
