//====================================================
// �X�N���v�g���FAnimationFinishTrigger
// �쐬�ҁF�X�e
// ���e�F�t�B�j�b�V�����̃A�j���[�V�����I���g���K�[
// [Log]
// 06/13�@�X�e �J�����̐���t���O�ǉ�
// 06/19�@�X�e  SE�Đ��@�\��ǉ�
// 06/27  �����@�^�[�Q�b�g��ύX����֐���ǉ�
// 07/11�@�X�e  �^�O�ɂ��ǉ��^�[�Q�b�g�ւ̏����@�\��ǉ�

//Snack���A�Z�b�g����̃v���n�u�Œu���ĂȂ��V�[���̓^�O��Snack���Ă�������
//�V���A���C�Y����Ă�ꍇ��Player�v���n�u����Model�ɂ��Ă�AnimationFinishTrigger�X�N���v�g�̒ǉ��^�[�Q�b�g���ڂ�Snack�Ɠ���Ă�������

//====================================================

using UnityEngine;
using System.Collections;

public class AnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject snackObject;

    [Header("�J�����A�g")]
    [SerializeField] private CameraFunction cameraFunction;

    [SerializeField] private Vector3 specialViewPosition = new Vector3(0, 2, -4);

    [Header("�G�t�F�N�g")]
    [Tooltip("�q�b�g���ɍĐ�����G�t�F�N�g")]
    [SerializeField] private GameObject hitEffectPrefab;

    [Tooltip("�G�t�F�N�g�Đ��ʒu�i��Fsnack�̈ʒu�j")]
    [SerializeField] private Transform effectSpawnPoint;

    [Header("SE")]
    [Tooltip("�q�b�g���ɍĐ�����SE")]
    [SerializeField] private AudioClip hitSound;

    [Tooltip("SE���Đ�����AudioSource")]
    [SerializeField] private AudioSource audioSource;

    [Header("�q�b�g�X�g�b�v����")]
    [Tooltip("�q�b�g���ɑS�̂��~�߂�b��")]
    [SerializeField] private float hitStopDuration = 0.5f;

    [Tooltip("��~�Ώۂ�Animator�i��F�v���C���[�j")]
    [SerializeField] private Animator playerAnimator;

    [Header("�ǉ��^�[�Q�b�g")]
    [Tooltip("snackObject�ȊO�Ƀq�b�g�X�g�b�v����������I�u�W�F�N�g�̃^�O")]
    [SerializeField] private string additionalTargetTag = "Snack";

    public void OnKickImpact()
    {
        Debug.Log("�L�b�N�����������^�C�~���O�ŌĂяo���ꂽ");
        StartCoroutine(HitStopRoutine());
    }

    private IEnumerator HitStopRoutine()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (hitEffectPrefab != null && effectSpawnPoint != null)
        {
            Instantiate(hitEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;

        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }

        // 1. �C���X�y�N�^�[�Őݒ肵����v�^�[�Q�b�g�̃q�b�g�X�g�b�v������
        if (snackObject != null)
        {
            snackObject.GetComponent<BlownAway_Ver3>()?.EndHitStop();
        }

        // 2. �ǉ��ŁA�w��^�O�����S�ẴI�u�W�F�N�g�̃q�b�g�X�g�b�v������
        if (!string.IsNullOrEmpty(additionalTargetTag))
        {
            GameObject[] additionalTargets = GameObject.FindGameObjectsWithTag(additionalTargetTag);
            foreach (var target in additionalTargets)
            {
                // ��v�^�[�Q�b�g�Əd�����ď������Ȃ��悤�Ƀ`�F�b�N
                if (target != snackObject)
                {
                    target.GetComponent<BlownAway_Ver3>()?.EndHitStop();
                }
            }
        }

        // �J�����̓��ꎋ�_����
        cameraFunction?.StopSpecialView();
    }

    public void SetTargetObject(GameObject target)
    {
        snackObject = target;
    }
}