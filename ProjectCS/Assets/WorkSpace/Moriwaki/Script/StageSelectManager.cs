//====================================================
// �X�N���v�g���FStageSelectManager
// �쐬�ҁF�X�e
// ���e�F�X�e�[�W�Z���N�g
// �ŏI�X�V���F05/07
//
// [Log]
// 05/03 �X�e �X�N���v�g�쐬
// 05/07 �X�e ��
//====================================================

using UnityEngine.SceneManagement;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageData
    {
        public Transform stageTransform;
        public string sceneName;
        public GameObject labelUI;
        public Collider stageCollider;
    }

    [Header("Audio")]
    public AudioSource audioSource;          // �� Inspector�ŃA�T�C��

    public AudioClip selectSE;               // �� �I����SE�i�J�[�\���ړ��Ȃǁj
    public AudioClip enterSE;                // �� ���艹SE�i�X�e�[�W�˓��j
    public AudioClip cancelSE;               // �� �L�����Z����SE�i�߂�j
    public AudioClip stageSelectBGMClip;    //BGM

    public StageData[] stages;
    public Transform cameraTransform;
    public Transform playerTransform;

    public float playerMoveSpeed = 3f;
    public float cameraFollowSpeed = 5f;
    public float stageEnterDistance = 2f;

    private int currentIndex = 0;
    private bool isAutoMoving = false;
    private Vector3 autoMoveTarget;

    private int touchingStageIndex = -1; // �v���C���[�����ݐG��Ă���X�e�[�W�i�Ȃ���� -1�j

    public FadeController fadeController; // �� Inspector�ɃA�T�C��
    public string backSceneName = "TitleScene"; // �� ESC�Ŗ߂�p

    private void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(stageSelectBGMClip); // AudioClip��p�ӂ���Inspector�Őݒ�
        }

        MovePlayerToStageInstant(currentIndex);
        UpdateStageLabels();

        // �t�F�[�h�C���i�J�n���j
        if (fadeController != null)
        {
            fadeController.FadeIn();
        }
    }

    private void Update()
    {
        HandleTriggerSelection();
        HandlePlayerInput();
        HandleAutoMove();
        HandleCameraFollow();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            TryEnterStage();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) // ESC or �}���{�^��
        {
            CancelAndGoBack(); // �����ʉ�
        }
    }

    private void UpdateCurrentStageByProximity()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < stages.Length; i++)
        {
            float dist = Vector3.Distance(
                new Vector3(stages[i].stageTransform.position.x, 0, 0),
                new Vector3(playerTransform.position.x, 0, 0)
            );

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestIndex = i;
            }
        }

        if (closestIndex != -1)
        {
            currentIndex = closestIndex;
        }
    }

    private void HandleTriggerSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // �����ʉ��Đ�
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // �����ʉ��Đ�
        }
    }

    private void HandlePlayerInput()
    {
        if (isAutoMoving) return;

        float h = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(h) > 0.1f)
        {
            Vector3 move = new Vector3(h, 0, 0);
            playerTransform.position += move.normalized * playerMoveSpeed * Time.deltaTime;
        }

        UpdateCurrentStageByProximity(); // �� ������ currentIndex ���X�V�I
    }

    private void HandleAutoMove()
    {
        if (!isAutoMoving) return;

        Vector3 dir = autoMoveTarget - playerTransform.position;
        dir.y = 0; // �����͖���
        float dist = dir.magnitude;

        if (dist > 0.05f)
        {
            playerTransform.position += dir.normalized * playerMoveSpeed * Time.deltaTime;
        }
        else
        {
            playerTransform.position = autoMoveTarget;
            isAutoMoving = false;
            UpdateStageLabels();
        }
    }

    private void CancelAndGoBack()
    {
        PlaySE(cancelSE); // ���L�����Z����
        if (fadeController != null)
        {
            fadeController.FadeOut(() =>
            {
                SceneManager.LoadScene(backSceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(backSceneName);
        }
    }

    private void HandleCameraFollow()
    {
        Vector3 targetPos = playerTransform.position + new Vector3(0, 2, -5);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * cameraFollowSpeed);
    }

    private void StartAutoMoveToStage(int index)
    {
        autoMoveTarget = new Vector3(
            stages[index].stageTransform.position.x,
            playerTransform.position.y,
            playerTransform.position.z // Z�����ɂ͈ړ����Ȃ��i���ړ��̂݁j
        );
        isAutoMoving = true;
        HideAllLabels();
    }

    private void MovePlayerToStageInstant(int index)
    {
        playerTransform.position = new Vector3(
            stages[index].stageTransform.position.x,
            playerTransform.position.y,
            playerTransform.position.z
        );
    }

    private void TryEnterStage()
    {
        if (touchingStageIndex >= 0 && touchingStageIndex < stages.Length)
        {
            PlaySE(enterSE); // �����艹

            string sceneName = stages[touchingStageIndex].sceneName;
            if (!string.IsNullOrEmpty(sceneName))
            {
                if (fadeController != null)
                {
                    fadeController.FadeOut(() =>
                    {
                        SceneManager.LoadScene(sceneName);
                    });
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
        else
        {
            Debug.Log("�ǂ̃X�e�[�W�ɂ��ڐG���Ă��܂���");
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

    private void UpdateStageLabels()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i].labelUI != null)
            {
                stages[i].labelUI.SetActive(i == touchingStageIndex);
            }
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

    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (other == stages[i].stageCollider)
            {
                touchingStageIndex = i;
                UpdateStageLabels(); // ���� ���x���X�V
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (other == stages[i].stageCollider && touchingStageIndex == i)
            {
                touchingStageIndex = -1;
                UpdateStageLabels(); // ���� ��\���ɍX�V
                return;
            }
        }
    }

    private void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}