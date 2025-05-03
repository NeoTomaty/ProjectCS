//====================================================
// �X�N���v�g���FStageSelectManager
// �쐬�ҁF�X�e
// ���e�F�X�e�[�W�Z���N�g
// �ŏI�X�V���F05/03
//
// [Log]
// 05/03 �X�e �X�N���v�g�쐬
//====================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageData
    {
        public Transform stageTransform;   // �X�e�[�W�̈ʒu
        public string sceneName;           // �J�ڐ�̃V�[����
        public GameObject labelUI;         // �X�e�[�W����\������UI�iCanvas�Ȃǁj
    }

    public StageData[] stages;
    public Transform cameraTransform;
    public float cameraMoveSpeed = 5f;

    private int currentIndex = 0;
    private Vector3 cameraTargetPosition;
    private bool isCameraMoving = false;

    private void Start()
    {
        MoveCameraToCurrentStage(true);
        UpdateStageLabels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % stages.Length;
            UpdateStageLabels();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
            UpdateStageLabels();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            LoadSelectedStage();
        }

        MoveCameraToCurrentStage(false);
    }

    private void MoveCameraToCurrentStage(bool instant)
    {
        cameraTargetPosition = stages[currentIndex].stageTransform.position + new Vector3(0, 2, -5);

        if (instant)
        {
            cameraTransform.position = cameraTargetPosition;
            isCameraMoving = false;
            UpdateStageLabels(); // ���\��
        }
        else
        {
            float distance = Vector3.Distance(cameraTransform.position, cameraTargetPosition);

            if (distance > 0.1f)
            {
                isCameraMoving = true;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, Time.deltaTime * cameraMoveSpeed);
                HideAllLabels(); // �ړ�����UI��\��
            }
            else
            {
                if (isCameraMoving)
                {
                    // ���߂ē��B�����Ƃ��������x���\��
                    isCameraMoving = false;
                    cameraTransform.position = cameraTargetPosition; // �҂����荇�킹��
                    UpdateStageLabels();
                }
            }
        }

        // LookAt�폜�ς݁A�J���������͌Œ�
    }

    private void UpdateStageLabels()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i].labelUI != null)
            {
                stages[i].labelUI.SetActive(i == currentIndex);
            }
        }
    }

    private void LoadSelectedStage()
    {
        string sceneName = stages[currentIndex].sceneName;
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty for stage " + currentIndex);
        }
    }

    private void HideAllLabels()
    {
        foreach (var stage in stages)
        {
            if (stage.labelUI != null)
            {
                stage.labelUI.SetActive(false);
            }
        }
    }
}