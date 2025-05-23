//====================================================
// �X�N���v�g���FMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̎����O�i�ړ�
// �ŏI�X�V���F05/15
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� Update���Ɉړ������ǉ�
// 03/31 ���� �X�N���v�g���ύX AutoMovePlayer��MovePlayer
// 04/08 ���� �������̕Ǌђʖh�~����������
// 04/09 �|�� AutoRapidMove�Ή�����悤�ɃX�N���v�g���C��
// 05/07 �r�� LiftingJump�̂��蔲�����[�h�ɑΉ�����悤�ɕύX
// 05/15 �r�� MoveSpeedMultiplier�ϐ���ǉ����A�ړ������ɑ��x�̕␳���悹����悤�ɕύX
//====================================================
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
    private LiftingJump LiftingJump; // ���t�e�B���O�W�����v�N���X

    private Vector3 MoveDirection;    // ���݂̐i�s����
    // ���̃X�N���v�g����i�s�������擾���邽�߂̃v���p�e�B                                  
    public Vector3 GetMoveDirection => MoveDirection;

    [SerializeField]
    private float RayDistance = 10.0f;
    [SerializeField]
    private float HitStopDuration = 0.1f;  // �q�b�g�X�g�b�v�̎���
    private float HitStopTimer = 0.0f;  // �q�b�g�X�g�b�v�̃^�C�}�[

    private bool IsHitStopActive = false; // �q�b�g�X�g�b�v�����ǂ���
    public bool GetIsHitStopActive => IsHitStopActive;

    // �ړ����x�̔{��
    [System.NonSerialized] public float MoveSpeedMultiplier = 1f; // PlayerSpeedManager�̃X�s�[�h�l��ς����ɑ��x��ς��������ߒǉ�

    private Rigidbody Rb;

    //SE���Đ����邽�߂�AudioSource
    [SerializeField] private AudioSource audioSource;

    //�ړ��J�n��ɖ炷SE
    [SerializeField] private AudioClip MoveStartSE;

    //
    private bool WasMoving = false;

    // ���̃X�N���v�g����i�s������ݒ肷�邽�߂̃Z�b�^�[
    public void SetMoveDirection(Vector3 NewDirection)
    {
        MoveDirection = NewDirection;
    }

    private string WallTag = "Wall";   // �ǂ̃^�O

    void Start()
    {
        MoveDirection = transform.forward;

        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }

        LiftingJump = GetComponent<LiftingJump>();
        if (LiftingJump == null)
        {
            Debug.LogWarning("LiftingJump�X�N���v�g��Player�ɃA�^�b�`����Ă��܂���B");
        }

        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        if (IsHitStopActive)
        {
            // �q�b�g�X�g�b�v���L���ȏꍇ�A�ړ����~�߂�
            if (HitStopTimer > 0.0f)
            {
                HitStopTimer -= Time.deltaTime;  // �^�C�}�[������
                return;  // �q�b�g�X�g�b�v���͈ړ����~
            }
            else
            {
                IsHitStopActive = false;
            }
        }

        if (PlayerSpeedManager == null) return;

        // ���C�L���X�g�ŕǂ����o
        // ���蔲�����L���ȏꍇ�̓��C�L���X�g�ɂ��Փ˔�����s��Ȃ�
        RaycastHit hit;
        if (!LiftingJump.IsIgnore && Physics.Raycast(transform.position, MoveDirection, out hit, RayDistance))
        {
            // �Փ˂����I�u�W�F�N�g�̃^�O���m�F
            if (hit.collider.CompareTag(WallTag))  // �ǃ^�O�Ɉ�v���邩�m�F
            {
                // ���C���ǂɏՓ˂����ꍇ�A���̒n�_�Ɉړ�
                // �߂荞�񂾕��������߂����߁A�Փ˓_����ړ������̔��Ε����ɏ������炷
                Vector3 PushBackDirection = -MoveDirection.normalized;  // ���Ε���
                float radius = GetComponent<SphereCollider>().radius;
                transform.position = hit.point + PushBackDirection * radius;  // �v���C���[���a���̉����߂�
            }
            else
            {
                // �i�s�������擾���A���̕����ֈړ�
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
            }
        }
        else
        {
            // �i�s�������擾���A���̕����ֈړ�
            if (LiftingJump.IsLiftingPart)
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
            }
            else
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, Rb.linearVelocity.y, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
            }

        }

        // ������i�s�����ɍ��킹��
        transform.forward = MoveDirection;
    }

    // �q�b�g�X�g�b�v�����s����
    public void StartHitStop()
    {
        HitStopTimer = HitStopDuration;
        IsHitStopActive = true;
    }
}