//======================================================
// StageName�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/5
// 
// [Log]5/5 �{�с@�X�e�[�W���\��
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    [SerializeField] private Text StageName;

    // �O������Ăяo���ĕ������\�����郁�\�b�h
    public void ShowMessage(string message)
    {
        if (StageName != null)
        {
            StageName.text = message;
        }
    }
}
