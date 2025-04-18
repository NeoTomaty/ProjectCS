//====================================================
// �X�N���v�g���FHeightManager
// �쐬�ҁF�|��
// ���e�FY���W���uHeight�F����m�v�`����UI�\��
// �ŏI�X�V���F04/16
// [Log]
// 04/16 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class HeightManager : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private Transform Object;     // �\������I�u�W�F�N�g��Transform
    [SerializeField] private Text HeightText;      // �\������UI�e�L�X�g�iLegacy UI�j

    [Header("�ݒ�")]
    [SerializeField] private string Prefix = "Height�F";  // �\������O�u���e�L�X�g
    [SerializeField] private string Unit = "m";           // �P�ʁim��km�Ȃǁj

    private float BaseHeight = 0f; // �V�[���J�n����Y���W��ێ�

    void Start()
    {
        if (Object == null)
        {
            Debug.LogError("�v���C���[�����ݒ�ł��B");
            enabled = false;
            return;
        }

        // �V�[���J�n���̍�������ɂ���
        BaseHeight = Object.position.y;
    }

    void Update()
    {
        if (HeightText == null) return;

        float currentHeight = Object.position.y;
        float relativeHeight = currentHeight - BaseHeight;

        // �����_1�ʂ܂ł̑��΍��x�\���i�����t���j
        HeightText.text = $"{Prefix} {relativeHeight:0.0} {Unit}";
    }

}
