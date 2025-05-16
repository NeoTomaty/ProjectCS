//====================================================
// スクリプト名：StageSelectManager
// 作成者：森脇
// 内容：ステージセレクト
// 最終更新日：05/07
//
// [Log]
// 05/03 森脇 スクリプト作成
// 05/07 森脇 音
//====================================================

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public AudioSource audioSource;
    public AudioClip selectSE;
    public AudioClip enterSE;
    public AudioClip cancelSE;

    public StageData[] stages;
    public Transform cameraTransform;
    public Transform playerTransform;

    public float playerMoveSpeed = 3f;
    public float cameraFollowSpeed = 5f;

    private int currentIndex = 0;
    private bool isAutoMoving = false;
    private Vector3 autoMoveTarget;

    private int touchingStageIndex = -1;

    public string backSceneName = "TitleScene";

    private bool isInputLocked = false;

    [SerializeField] private PauseManager pauseManager;

    private float inputCooldownAfterUnpause = 0.2f;
    private float unpauseTimer = 0f;
    private bool wasPausedLastFrame = false;

    private void Start()
    {
        MovePlayerToStageInstant(currentIndex);
        UpdateStageLabels();
        // FadeManagerが自動でFadeInしてくれるので不要
    }

    private void Update()
    {
        bool isPaused = pauseManager != null && pauseManager.IsPaused();

        if (wasPausedLastFrame && !isPaused)
        {
            unpauseTimer = inputCooldownAfterUnpause;
        }
        wasPausedLastFrame = isPaused;

        if (unpauseTimer > 0f)
        {
            unpauseTimer -= Time.deltaTime;
            return;
        }

        if (isInputLocked) return;

        if (!isPaused)
        {
            HandleTriggerSelection();
            HandlePlayerInput();
            HandleAutoMove();
            HandleCameraFollow();

            if (IsSubmitPressed())
            {
                TryEnterStage();
            }

            if (IsCancelPressed())
            {
                CancelAndGoBack();
            }
        }
    }

    private bool IsSubmitPressed()
    {
        return (Keyboard.current != null &&
                (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)) ||
               (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
    }

    private bool IsCancelPressed()
    {
        return (Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame) ||
               (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame);
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
        if (Gamepad.current == null && !Keyboard.current.rightArrowKey.wasPressedThisFrame && !Keyboard.current.leftArrowKey.wasPressedThisFrame)
            return;

        if ((Gamepad.current != null && Gamepad.current.dpad.left.wasPressedThisFrame) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE);
        }

        if ((Gamepad.current != null && Gamepad.current.dpad.right.wasPressedThisFrame) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE);
        }
    }

    private void HandlePlayerInput()
    {
        if (isAutoMoving) return;

        float h = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(h) > 0.1f)
        {
            Vector3 move = new Vector3(h, 0, 0);
            Vector3 nextPos = playerTransform.position + move.normalized * playerMoveSpeed * Time.deltaTime;

            Ray ray = new Ray(playerTransform.position, move.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.6f) || !hit.collider.CompareTag("Wall"))
            {
                playerTransform.position = nextPos;
            }
        }

        UpdateCurrentStageByProximity();
    }

    private void HandleAutoMove()
    {
        if (!isAutoMoving) return;

        Vector3 dir = autoMoveTarget - playerTransform.position;
        dir.y = 0;
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
        isInputLocked = true;
        PlaySE(cancelSE);

        FadeManager fade = FindFirstObjectByType<FadeManager>();
        if (fade != null)
        {
            fade.FadeToScene(backSceneName);
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
            playerTransform.position.z
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
            isInputLocked = true;
            PlaySE(enterSE);

            string sceneName = stages[touchingStageIndex].sceneName;
            if (!string.IsNullOrEmpty(sceneName))
            {
                FadeManager fade = FindFirstObjectByType<FadeManager>();
                if (fade != null)
                {
                    fade.FadeToScene(sceneName);
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
        else
        {
            Debug.Log("どのステージにも接触していません");
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
                UpdateStageLabels();
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
                UpdateStageLabels();
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
