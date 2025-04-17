//====================================================
// �X�N���v�g���FLifeManager
// �쐬�ҁF�|��
// ���e�F���C�t�Ǘ�
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/08 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int Life = 5;        // ���C�t
    public Text LifeText;       // �e�L�X�gUI

    void Start()
    {
        // �e�L�X�g�̍X�V
        UpdateLifeText();
    }

    public void DecreaseLife()
    {
        // ���C�t�͂O�����ɂȂ�Ȃ�
        Life = Mathf.Max(Life - 1, 0);
        // �e�L�X�g�̍X�V
        UpdateLifeText();
    }

    void UpdateLifeText()
    {
        // ���C�t�l�̍X�V
        LifeText.text = "Life: " + Life.ToString();
    }
}
