//======================================================
// ResultpointDisplay �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/23
// 
// [Log]4/23 �{�с@�|�C���g�\����ǉ�
//                 
//======================================================

using UnityEngine;
using UnityEngine.UI;

public class ResultPointDisplay : MonoBehaviour
{
    public Text pointText;

    // ���ŊO������󂯎��|�C���g�l
    public int receivedPoint =200;

    void Start()
    {
        // �|�C���g���܂��n����Ă��Ȃ���΃f�t�H���g�\��
        if (receivedPoint < 0)
        {
            pointText.text = "�|�C���g: �擾��...";
        }
        else
        {
            DisplayPoint();
        }
    }

    // ���X�N���v�g����Ăяo����悤�ɂ��Ă���
    public void SetPoint(int point)
    {
        receivedPoint = point;
        DisplayPoint();
    }

    void DisplayPoint()
    {
        pointText.text = "�|�C���g: " + receivedPoint.ToString();
    }
}
