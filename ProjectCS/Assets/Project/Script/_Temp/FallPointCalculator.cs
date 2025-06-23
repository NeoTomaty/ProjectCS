//====================================================
// �X�N���v�g���FFallPointCalculator
// �쐬�ҁF����
// ���e�F��΂��Ώۂ̗����n�_���v�Z����N���X
// �ŏI�X�V���F05/14
// 
// [Log]
// 04/27 ���� �X�N���v�g�쐬
// 05/13 ���� �n�ʂƂ̓����蔻����^�O���烌�C���[�ɕύX
// 05/14 ���� �n�ʂ̍��W�ƃI�u�W�F�N�g�̗������W�𕪂��ď��������邱�ƂɕύX
// 06/13 ���� �X�i�b�N�������ɕK�v�ȃR���|�[�l���g���Q�Ƃ���SetTarget�֐���ǉ�
// 06/23 ���� ���炩���ߐݒ肳�ꂽ���[�v��ŗ����n�_���v�Z����悤�ɕύX
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. ���̃X�N���v�g�͔�΂��Ώۂ̃I�u�W�F�N�g�ɃA�^�b�`
// 2. LAManager�ɂ�LiftingArea�I�u�W�F�N�g�ɂ��Ă���LiftingAreaManager��ݒ�
// 3. �I�u�W�F�N�g���΂�����̃��[�v��̍��W�����肵���Ƃ��ɁACalculateGroundPoint���Ăяo���i���X�N���v�g����Q�Ɓj
// 4. BaseGroundLayerMask�Ƀx�[�X�ƂȂ�n�ʁi��ԉ��̒n�ʁj�̃��C���[��ݒ肷��
using UnityEngine;

public class FallPointCalculator : MonoBehaviour
{
    private string GroundTag = "Ground"; // �n�ʃI�u�W�F�N�g�̃^�O
    private Vector3 FallPoint;           // �����n�_
    private Vector3 GroundPoint;         // �n�ʂ̍��W
    private bool IsGround = false;       // �n�ʂɒ��n���Ă��邩�ǂ���
    private float SnackOffsetY = 0f;

    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManager���Q��
   
    [Tooltip("BaseGround�̃��C���[��ݒ�")]
    [SerializeField] private LayerMask BaseGroundLayerMask; // �x�[�X�ƂȂ�n�ʂ̃��C���[

    private BlownAway_Ver3 BAV3;

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManager���ݒ肳��Ă��܂���");

        //CalculateGroundPoint(); // test

        SnackOffsetY = GetComponent<Collider>().bounds.extents.y;

        BAV3 = GetComponent<BlownAway_Ver3>();

        // �N���[���ŕ������ꂽ�X�i�b�N�̂ݏ���̗����n�_�̌v�Z���s��
        if (gameObject.name.EndsWith("(Clone)"))
        {
            CalculateGroundPoint();
        }
    }

    public void SetTarget(LiftingAreaManager LAM)
    {
        LAManager = LAM;
    }

    // �X�i�b�N���n�ʂɐڂ��Ă��邩���t�e�B���O�G���A�O�̂Ƃ���
    // ���t�e�B���O�G���A�̈ʒu���Ē�������
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        if(!LAManager.GetIsTargetContacting())
        {
            CalculateGroundPoint(); // ���t�e�B���O�G���A�̃|�C���g���Čv�Z
            Debug.Log("���t�e�B���O�G���A�Čv�Z���s");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        IsGround = true; // �n�ʂɒ����Ă���
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        IsGround = false; // �n�ʂɒ����Ă��Ȃ�
    }

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = BAV3.NextWarpPosition;
        Vector3 direction = Vector3.down;

        // BaseGroundLayer�̂ݔ���
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, BaseGroundLayerMask)) // �ꉞ�������̃��C�͖����ɐݒ�
        {
            Debug.Log("�����n�_�F" + hit.point);
            GroundPoint = hit.point;

            // ���t�e�B���O�G���A�I�u�W�F�N�g���ړ�������
            LAManager.SetFallPoint(GroundPoint);
        }

        // �^�[�Q�b�g�I�u�W�F�N�g�̗����n�_���v�Z�i���ׂĂ̒n�ʂɑ΂��Ď��s�j
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag(GroundTag))
            {
               FallPoint = hit.point;
            }
        }
    }

    // FallPoint���擾�i�g�p���邩������Ȃ����ꉞ����Ă܂��j
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }

    // �n�ʂɒ����Ă���ǂ������擾
    public bool GetIsGround()
    {
        return IsGround;
    }

    public float GetDistanceToGround()
    {
        return (transform.position.y - SnackOffsetY) - GroundPoint.y;
    }
}
