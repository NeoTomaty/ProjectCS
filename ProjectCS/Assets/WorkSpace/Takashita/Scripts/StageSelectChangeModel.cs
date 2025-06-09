//====================================================
// �X�N���v�g���FStageSelectChangeModel
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g��2�̃��f����؂�ւ���X�N���v�g
// �ŏI�X�V���F06/09
// 
// [Log]
// 06/09 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class StageSelectChangeModel : MonoBehaviour
{

    [SerializeField] private GameObject Model1;
    [SerializeField] private GameObject Model2;

    void Start()
    {
        Model1.SetActive(true);
        Model2.SetActive(false);
    }

    public void SetChangeModel(bool isSpecialMode)
    {
        Model1.SetActive(isSpecialMode);
        Model2.SetActive(!isSpecialMode);
    }
}
