//======================================================
// �X�N���v�g���FSetFirstMenu
// �쐬�ҁF�{��
// �ŏI�X�V���F4/23
// 
// [Log]
// 4/23 �{�с@�ŏ��ɑI�΂�Ă���{�^�������蓖�Ă�
// 4/28 �|���@�X�N���v�g���ύX
// 
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;

public class SetFirstMenu : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
