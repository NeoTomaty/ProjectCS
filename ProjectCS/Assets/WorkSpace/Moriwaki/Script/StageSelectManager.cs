//====================================================
// スクリプト名：StageSelectManager
// 作成者：森脇
// 内容：ステージセレクト
// 最終更新日：05/03
//
// [Log]
// 05/03 森脇 スクリプト作成
//====================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class StageData
    {
        public Transform stageTransform;   // ステージの位置
        public string sceneName;           // 遷移先のシーン名
        public GameObject labelUI;         // ステージ名を表示するUI（Canvasなど）
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
            UpdateStageLabels(); // 即表示
        }
        else
        {
            float distance = Vector3.Distance(cameraTransform.position, cameraTargetPosition);

            if (distance > 0.1f)
            {
                isCameraMoving = true;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, Time.deltaTime * cameraMoveSpeed);
                HideAllLabels(); // 移動中はUI非表示
            }
            else
            {
                if (isCameraMoving)
                {
                    // 初めて到達したときだけラベル表示
                    isCameraMoving = false;
                    cameraTransform.position = cameraTargetPosition; // ぴったり合わせる
                    UpdateStageLabels();
                }
            }
        }

        // LookAt削除済み、カメラ向きは固定
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