//======================================================
// MenuSlector �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/23
// 
// [Log]4/23 �{�с@�ŏ��ɑI�΂�Ă���{�^�������蓖�Ă�
//                 
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
