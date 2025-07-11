//====================================================
// �X�N���v�g���FObjectGravity
// �쐬�ҁF����
// ���e�F�I�u�W�F�N�g�̏d�͂��Ǘ�����N���X
// �ŏI�X�V���F05/23
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/27 �r�� �A�N�e�B�u�t���O��ǉ� 
// 05/09 ���� �������x�̐����ǉ�
// 05/23 ���� �X�i�b�N����SE����
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // �d�͂̑傫��

    [SerializeField]
    private float MaxFallSpeed = 20.0f; // �ő嗎�����x�i���̒l�j

    private Rigidbody Rb;    // �I�u�W�F�N�g��Rigidbody

    public bool IsActive = true;

    //�������ɍĐ�������ʉ�(AudioClip)
    [SerializeField] private AudioClip FallSE;

    //���ʉ����Đ����邽�߂�AudioSource�R���|�[�l���g
    private AudioSource Audio;

    //���ݗ��������ǂ����������t���O
    private bool IsFalling = false;

    //�����������łɍĐ��������ǂ����̃t���O
    private bool HasPlayedFallSE = false;

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾

        //AudioSource���擾
        Audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsActive) return;
        // �d��
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // �������x�𐧌�
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;
        }

        //���ȏ�̉��������x������Ƃ��A�������Ɣ���
        if (Rb.linearVelocity.y < -1.0f)
        {
            IsFalling = true;
        }
    }

    //���̃I�u�W�F�N�g�ƏՓ˂����Ƃ��ɌĂ΂��
    private void OnCollisionEnter(Collision collision)
    {
        //�������ł܂�SE���Đ����Ă��Ȃ��Ƃ��A���ʉ����Đ�
        if(IsFalling && !HasPlayedFallSE)
        {
            if (FallSE != null && Audio != null)
            {
                //���ʉ���1�񂾂��Đ�
                Audio.PlayOneShot(FallSE);
            }

            //�Đ��ς݃t���O�𗧂Ă�
            HasPlayedFallSE = true;
        }

        //�Փ˂����̂ŗ�����Ԃ�����
        IsFalling = false;
    }

    //�Փ˂��I�������Ƃ��ɌĂ΂��
    private void OnCollisionExit(Collision collision)
    {
        //���̗����ɔ����ăt���O�����Z�b�g
        HasPlayedFallSE = false;
    }

    public float GetGravityScaleY()
    {
        return GravityScale.y;
    }
}
