using UnityEngine;

public class LoadStageScore : MonoBehaviour
{
    [SerializeField] private int[] StageScores = new int[6];
    [SerializeField] private TextAsset ScoreCSV;

    void Start()
    {
        LoadCSV();
    }

    private void LoadCSV()
    {
        if (ScoreCSV == null)
        {
            Debug.LogError("CSV�t�@�C�����A�^�b�`����Ă��܂���B");
            return;
        }

        string[] lines = ScoreCSV.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < Mathf.Min(StageScores.Length, lines.Length); i++)
        {
            if (int.TryParse(lines[i], out int score))
            {
                StageScores[i] = Mathf.Clamp(score, 0, 99999); // �O�̂��ߐ���
                Debug.Log(StageScores[i]);
            }
            else
            {
                Debug.LogWarning($"�X�R�A�̓ǂݎ�莸�s: �s{i + 1} = {lines[i]}");
            }
        }
    }

    public int GetStageScore(int arrayNum)
    {
        if (arrayNum >= StageScores.Length) return -1;

        return StageScores[arrayNum];
    }
}
