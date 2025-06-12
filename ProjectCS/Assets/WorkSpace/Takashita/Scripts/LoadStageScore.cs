//====================================================
// �X�N���v�g���FLoadStageScore
// �쐬�ҁF����
// ���e�FCSV����X�R�A��ǂݎ��A�ێ�����
// �ŏI�X�V���F06/12
// 
// [Log]
// 06/12 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

// CSV�t�@�C������X�e�[�W���Ƃ̃X�R�A�i6���j��ǂݍ���Ŋi�[����R���|�[�l���g
public class LoadStageScore : MonoBehaviour
{
    // �X�e�[�W�X�R�A���i�[����z��i�ő�6�j
    [SerializeField] private int[] StageScores = new int[6];

    // �C���X�y�N�^�[����A�^�b�`����CSV�t�@�C���iTextAsset�j
    [SerializeField] private TextAsset ScoreCSV;

    // �Q�[���J�n����CSV��ǂݍ���
    void Start()
    {
        LoadCSV();
    }

    // CSV�t�@�C����ǂݍ��݁A�X�R�A�� StageScores �z��Ɋi�[����
    private void LoadCSV()
    {
        // CSV���ݒ肳��Ă��Ȃ��ꍇ�̓G���[���O���o���ďI��
        if (ScoreCSV == null)
        {
            Debug.LogError("CSV�t�@�C�����A�^�b�`����Ă��܂���B");
            return;
        }

        // ���s�ŕ������čs�P�ʂɎ��o���i��s�͏��O�j
        string[] lines = ScoreCSV.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // �ő��6�܂œǂݍ���
        for (int i = 0; i < Mathf.Min(StageScores.Length, lines.Length); i++)
        {
            // ���l�Ƀp�[�X�ł�����i�[�i�ő�l99999�܂Ő����j
            if (int.TryParse(lines[i], out int score))
            {
                StageScores[i] = Mathf.Clamp(score, 0, 99999);
                Debug.Log(StageScores[i]); // �f�o�b�O�\���i�K�v�ɉ����č폜�j
            }
            else
            {
                // ���l�łȂ��s���������ꍇ�͌x�����O
                Debug.LogWarning($"�X�R�A�̓ǂݎ�莸�s: �s{i + 1} = {lines[i]}");
            }
        }
    }

    // �w�肳�ꂽ�C���f�b�N�X�̃X�e�[�W�X�R�A���擾
    /// <param name="arrayNum">�X�e�[�W�ԍ��i0�`5�j</param>
    /// <returns>�X�R�A�i�͈͊O�̏ꍇ�� -1�j</returns>
    public int GetStageScore(int arrayNum)
    {
        if (arrayNum >= StageScores.Length)
            return -1;

        return StageScores[arrayNum];
    }
}
