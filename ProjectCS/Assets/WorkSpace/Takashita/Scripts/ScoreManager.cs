//====================================================
// �X�N���v�g���FScoreManager
// �쐬�ҁF����
// ���e�F�X�e�[�W�S�̂̃X�R�A���Ǘ�����X�N���v�g
// �ŏI�X�V���F07/15
// 
// [Log]
// 06/20 ���� �X�N���v�g�쐬
// 07/15 ���� �ۑ��`����JSON�ɕύX
//====================================================
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int[] StageScores = new int[9];

    private const string FileName = "scores.json";

    [System.Serializable]
    private class ScoreData
    {
        public int[] StageScores;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadJSON();
    }

    public void SetStageScore(int arrayNum, int score)
    {
        if (arrayNum >= StageScores.Length)
        {
            Debug.LogError("�͈͊O�̐��l���n����܂���");
            return;
        }

        StageScores[arrayNum] = score;
    }

    public int GetStageScore(int arrayNum)
    {
        if (arrayNum >= StageScores.Length)
            return -1;

        return StageScores[arrayNum];
    }

    private void OnApplicationQuit()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("�G�f�B�^�Đ����̂��߁AJSON�ۑ��̓X�L�b�v����܂���");
            return;
        }

        SaveJSON();
    }

    private void LoadJSON()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, FileName);

        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("�G�f�B�^�Đ����̂��߁AJSON�ǂݍ��݂̓X�L�b�v����܂���");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("scores.json �����݂��܂���B�V�����쐬���ď��������܂��B");
            SaveJSON(); // ����ۑ�
            return;
        }

        string json = File.ReadAllText(filePath);
        ScoreData data = JsonUtility.FromJson<ScoreData>(json);

        if (data != null && data.StageScores != null)
        {
            int count = Mathf.Min(StageScores.Length, data.StageScores.Length);
            for (int i = 0; i < count; i++)
            {
                StageScores[i] = Mathf.Clamp(data.StageScores[i], 0, 99999);
            }

            Debug.Log("�X�R�A��JSON����ǂݍ��݂܂���");
        }
        else
        {
            Debug.LogWarning("JSON�̓ǂݍ��݂Ɏ��s���܂����B���������܂��B");
            SaveJSON();
        }
#else
        Debug.LogWarning("���̋@�\�̓X�^���h�A�����iPC�r���h�j��p�ł��B");
#endif
    }

    private void SaveJSON()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, FileName);

        ScoreData data = new ScoreData { StageScores = StageScores };
        string json = JsonUtility.ToJson(data, true); // true�Ő��`�i���₷���j

        File.WriteAllText(filePath, json);

        Debug.Log($"�X�R�A��JSON�ŕۑ����܂���: {filePath}");
#else
        Debug.LogWarning("���̋@�\�̓X�^���h�A�����iPC�r���h�j��p�ł��B");
#endif
    }
}
