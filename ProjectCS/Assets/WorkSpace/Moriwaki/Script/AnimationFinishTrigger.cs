using UnityEngine;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;  // �� snackObject ���`

    public void OnKickImpact()
    {
        Debug.Log("�L�b�N�����������^�C�~���O�ŌĂяo���ꂽ");
        // snack�̃X�N���v�g���Q�Ƃ��A�q�b�g�X�g�b�v������
        snackObject.GetComponent<BlownAway_Ver3>().EndHitStop();
    }
}