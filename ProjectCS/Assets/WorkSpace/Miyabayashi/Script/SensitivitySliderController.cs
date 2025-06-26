using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SensitivitySliderController : MonoBehaviour
{
    [SerializeField] private GameObject sensitivityButton;     // ���x�����p�{�^��
    [SerializeField] private Slider sensitivitySlider;         // ���x�X���C�_�[
    [SerializeField] private InputActionReference adjustAction;
    [SerializeField] private CameraFunction cameraFunction;    // ���x���f�p�i�C�Ӂj

    private float adjustCooldown = 0.15f;
    private float adjustTimer = 0f;

    private void OnEnable()
    {
        adjustAction.action.Enable();
    }

    private void OnDisable()
    {
        adjustAction.action.Disable();
    }

    private void Update()
    {
        adjustTimer -= Time.unscaledDeltaTime;

        if (EventSystem.current.currentSelectedGameObject == sensitivityButton)
        {
            Vector2 input = adjustAction.action.ReadValue<Vector2>();
            float horizontal = input.x;

            if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
            {
                float moveAmount = 0.05f;
                float direction = Mathf.Sign(horizontal);

                sensitivitySlider.value = Mathf.Clamp(sensitivitySlider.value + direction * moveAmount, 0.5f, sensitivitySlider.maxValue);

                // �ݒ�ۑ������f
                GameSettingsManager.Instance.SetSensitivity(sensitivitySlider.value);
                if (cameraFunction != null)
                {
                    cameraFunction.SetRatio(sensitivitySlider.value);
                }

                Debug.Log($"Sensitivity: {sensitivitySlider.value}");

                adjustTimer = 0.15f;
            }
        }
    }
}
