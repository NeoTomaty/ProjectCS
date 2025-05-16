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
    public AudioSource audioSource;          // ← Inspectorでアサイン

    public AudioClip selectSE;               // ← 選択音SE（カーソル移動など）
    public AudioClip enterSE;                // ← 決定音SE（ステージ突入）
    public AudioClip cancelSE;               // ← キャンセル音SE（戻る）

    public StageData[] stages;
    public Transform cameraTransform;
    public Transform playerTransform;

    public float playerMoveSpeed = 3f;
    public float cameraFollowSpeed = 5f;
    public float stageEnterDistance = 2f;

    private int currentIndex = 0;
    private bool isAutoMoving = false;
    private Vector3 autoMoveTarget;

    private int touchingStageIndex = -1; // プレイヤーが現在触れているステージ（なければ -1）

    public FadeController fadeController; // ← Inspectorにアサイン
    public string backSceneName = "TitleScene"; // ← ESCで戻る用

    private bool isInputLocked = false; // ← 追加

    [SerializeField] private PauseManager pauseManager;

    private void Start()
    {
        MovePlayerToStageInstant(currentIndex);
        UpdateStageLabels();

        // フェードイン（開始時）
        if (fadeController != null)
        {
            fadeController.FadeIn();
        }
    }

    private float inputCooldownAfterUnpause = 0.2f; // ポーズ解除後に待つ時間（秒）
    private float unpauseTimer = 0f;
    private bool wasPausedLastFrame = false;

    private void Update()
    {
        // ポーズ状態の取得
        bool isPaused = pauseManager != null && pauseManager.IsPaused();

        // ポーズ解除された直後ならタイマー開始
        if (wasPausedLastFrame && !isPaused)
        {
            unpauseTimer = inputCooldownAfterUnpause;
        }
        wasPausedLastFrame = isPaused;

        // クールタイム中は入力無効
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
        bool keyboardSubmit = Keyboard.current != null &&
            (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

        bool gamepadSubmit = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame; // Aボタン

        return keyboardSubmit || gamepadSubmit;
    }

    private bool IsCancelPressed()
    {
        bool keyboardCancel = Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame;

        bool gamepadCancel = Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame; // Bボタン

        return keyboardCancel || gamepadCancel;
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
        if (Gamepad.current == null) return;

        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // ★効果音再生
        }

        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            currentIndex = (currentIndex + 1) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // ★効果音再生
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // ★効果音再生
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
            StartAutoMoveToStage(currentIndex);
            PlaySE(selectSE); // ★効果音再生
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

            // 前方にWallタグがあれば進めない
            Ray ray = new Ray(playerTransform.position, move.normalized);
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.6f))
            {
                playerTransform.position = nextPos;
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                // Wall に当たったので移動しない
            }
            else
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
        dir.y = 0; // 高さは無視
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
        isInputLocked = true; // ← 操作ロック
        PlaySE(cancelSE);

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
            playerTransform.position.z // Z方向には移動しない（横移動のみ）
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
            isInputLocked = true; // ← 操作ロック
            PlaySE(enterSE);

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
            Debug.Log("どのステージにも接触していません");
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
                UpdateStageLabels(); // ←★ ラベル更新
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
                UpdateStageLabels(); // ←★ 非表示に更新
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