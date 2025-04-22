//======================================================
// PlayerScore�X�N���v�g
// �쐬�ҁF�X�e
// �ŏI�X�V���F4/22
//
// [Log]4/22 ���{�@�v���C���[�̃X�R�A�Ǘ���UI�ւ̔��f���s��
//======================================================
using UnityEngine;
using TMPro; // TextMeshPro�p�̖��O���

public class PlayerScore : MonoBehaviour
{
    // ���݂̃X�R�A
    public int Score = 0;

    // �X�R�A��\������TextMeshProUGUI�i�C���X�y�N�^�[�Őݒ�j
    public TextMeshProUGUI ScoreText;

    void Start()
    {
        // �Q�[���J�n���ɃX�R�A�\����������
        UpdateScoreUI();
    }


    // �w�肵���|�C���g���X�R�A�ɉ��Z
    public void AddScore(int amount)
    {
        Score += amount;

        // �X�R�A��UI���X�V
        UpdateScoreUI();

        // �f�o�b�O�p���O
        Debug.Log($"{gameObject.name} �̌��݃X�R�A: {Score}");
    }

    // UI�e�L�X�g���ŐV�̃X�R�A�ɍX�V
    private void UpdateScoreUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score: {Score}";
        }
    }
}
