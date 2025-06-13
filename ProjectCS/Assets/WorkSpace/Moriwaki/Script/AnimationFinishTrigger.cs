using UnityEngine;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;  // �� snackObject ���`

    // [�ǉ�] �J�����A�g�̂��߂̐ݒ�
    [Header("�J�����A�g")]
    [Tooltip("�J��������X�N���v�g")]
    [SerializeField] private CameraFunction cameraFunction;

    [Tooltip("���ʎ��_���̃v���C���[����̑��ΓI�ȃJ�����ʒu")]
    [SerializeField] private Vector3 specialViewPosition = new Vector3(0, 2, -4);

    public void OnKickImpact()
    {
        Debug.Log("�L�b�N�����������^�C�~���O�ŌĂяo���ꂽ");
        // snack�̃X�N���v�g���Q�Ƃ��A�q�b�g�X�g�b�v������
        snackObject.GetComponent<BlownAway_Ver3>().EndHitStop();
        if (cameraFunction != null)
        {
            cameraFunction.StopSpecialView();
        }
    }
}