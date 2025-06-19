//BidirectionalRapidMove.cs
//�쐬��:��������
//�ŏI�X�V��:2025/06/10
//�A�^�b�`:Player�ɃA�^�b�`
//[Log]
//06/10�@�����@Player�������ړ��M�~�b�N�ɐG���Ƌ����ړ����鏈��
//06/19�@�����@�����ړ�����SE����

using UnityEngine;
using System.Collections;

public class BidirectionalRapidMove : MonoBehaviour
{
    //�v���C���[���i�ރ|�C���g(StartGate��EndGate)
    public Transform[] ForwardPoints;

    //�v���C���[���߂�|�C���g(EndGate��StartGate)
    public Transform[] BackwardPoints;

    //�v���C���[�̈ړ����x
    public float Speed = 10.0f;

    //���݋����ړ������ǂ����̃t���O
    private bool IsMoving = false;

    //�v���C���[��Rigidbody(������������p)
    private Rigidbody rb;

    //�v���C���[�̑���X�N���v�g(�����ړ��𐧌�)
    private MovePlayer PlayerController;

    [Header("�����ړ��M�~�b�N�ɐG�ꂽ�Ƃ���SE")]
    //�Đ�����SE
    public AudioClip MoveSE;

    //AudioSource�R���|�[�l���g
    private AudioSource audioSource;

    private void Start()
    {
        //Rigidbody��MovePlayer�X�N���v�g���擾
        rb = GetComponent<Rigidbody>();
        PlayerController = GetComponent<MovePlayer>();
        audioSource = GetComponent<AudioSource>();

        //ForwardPoints��BackwardPoints�̃R���C�_�[�𖳎��ݒ�
        IgnoreCollisions(ForwardPoints);
        IgnoreCollisions(BackwardPoints);
    }

    //�e�|�C���g�Ƃ̏Փ˂𖳎�����ݒ�
    private void IgnoreCollisions(Transform[] Points)
    {
        foreach (Transform Point in Points)
        {
            Collider PointCollider = Point.GetComponent<Collider>();
            Collider PlayerCollider = GetComponent<Collider>();

            if(PointCollider != null && PlayerCollider != null)
            {
                Physics.IgnoreCollision(PlayerCollider, PointCollider);
            }
        }
    }

    //�Q�[�g�ɓ������Ƃ��̏���
    private void OnTriggerEnter(Collider other)
    {
        //���łɈړ����A�܂��̓v���C���[�łȂ��Ƃ��͖���
        if(IsMoving || !gameObject.CompareTag("Player"))
        {
            return;
        }

        //StartGate�ɓ�������O�i���[�g��
        if(other.CompareTag("StartGate"))
        {
            StartCoroutine(MoveThroughPoints(ForwardPoints));
        }
        //EndGate�ɓ��������ރ��[�g��
        else if (other.CompareTag("EndGate"))
        {
            StartCoroutine(MoveThroughPoints(BackwardPoints));
        }
    }

    //�|�C���g�����Ɉړ�����R���[�`��
    private IEnumerator MoveThroughPoints(Transform[] Points)
    {
        //�ړ����t���O�𗧂Ă�
        IsMoving = true;

        //�v���C���[����𖳌���
        if(PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        //Rigidbody�̕����������~
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        //SE���Đ�
        if (audioSource != null && MoveSE != null)
        {
            audioSource.clip = MoveSE;
            audioSource.loop = true;
            audioSource.Play();
        }

        //�e�|�C���g�Ɍ������ď��Ɉړ�
        foreach (Transform Target in Points)
        {
            while (Vector3.Distance(transform.position, Target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                transform.position,
                Target.position,
                Speed * Time.deltaTime
                );
                yield return null;
            }
            //�e�|�C���g�ŏ����ҋ@
            yield return new WaitForSeconds(0.1f);
        }

        //SE���~
        if(audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        //�ړ�������A���������Ƒ�������ɖ߂�
        if(rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if(PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        //3�b��ɍĂшړ��\�ɂ���
        yield return new WaitForSeconds(3.0f);

        IsMoving = false;
    }
}