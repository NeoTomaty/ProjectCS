//====================================================
// �X�N���v�g���FScoreManager
// �쐬�ҁF����
// ���e�F�X�e�[�W�S�̂̃X�R�A���Ǘ�����X�N���v�g
// �ŏI�X�V���F06/20
// 
// [Log]
// 06/20 ���� �X�N���v�g�쐬
//====================================================
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // �X�e�[�W�X�R�A���i�[����z��
    [SerializeField] private int[] StageScores = new int[9];



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCSV();
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
            Debug.Log("�G�f�B�^�Đ����̂��߁ACSV�t�@�C���ۑ��̓X�L�b�v����܂���");
            return;
        }
        SaveCSV();
    }

    private void LoadCSV()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, "scores.csv");

        // �G�f�B�^���s���Ȃ�CSV�������X�L�b�v
        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("�G�f�B�^�Đ����̂��߁ACSV�t�@�C�������̓X�L�b�v����܂���");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("scores.csv �����݂��܂���B�V�����쐬���ď��������܂��B");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < StageScores.Length; i++)
                {
                    StageScores[i] = 0;
                    writer.WriteLine(0);
                }
            }

            Debug.Log($"scores.csv �� {filePath} �ɏ��������܂���");
            return;
        }

        // �ǂݍ��ݏ���
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < Mathf.Min(StageScores.Length, lines.Length); i++)
        {
            if (int.TryParse(lines[i], out int score))
            {
                StageScores[i] = Mathf.Clamp(score, 0, 99999);
            }
            else
            {
                StageScores[i] = 0;
                Debug.LogWarning($"�s{i + 1} �ǂݎ�莸�s �� 0�ŏ�����");
            }
        }

        Debug.Log("�X�R�A��ǂݍ��݂܂���");
#else
    Debug.LogWarning("���̋@�\�̓X�^���h�A�����iPC�r���h�j��p�ł��B");
#endif
    }

    private void SaveCSV()
    {
#if UNITY_STANDALONE
        // YourGame_Data �t�H���_�̐e�� exe �̂���ꏊ
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, "scores.csv");

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach (int score in StageScores)
            {
                writer.WriteLine(score);
            }
        }

        Debug.Log($"exe�Ɠ����ꏊ�ɃX�R�A��ۑ����܂���: {filePath}");
#else
    Debug.LogWarning("���̋@�\�̓X�^���h�A�����iPC�r���h�j��p�ł��B");
#endif
    }
}
