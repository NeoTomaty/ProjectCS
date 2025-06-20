//====================================================
// �X�N���v�g���FAnimationFinishTrigger
// �쐬�ҁF�X�e
// ���e�F�t�B�j�b�V�����̃A�j���[�V�����I���g���K�[
// [Log]
// 06/13�@�X�e �J�����̐���t���O�ǉ�
// 06/19�@�X�e  SE�Đ��@�\��ǉ�
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

    public void OnKickImpact()
    {
        Debug.Log("�L�b�N�����������^�C�~���O�ŌĂяo���ꂽ");
        StartCoroutine(HitStopRoutine());
    }

    private IEnumerator HitStopRoutine()
    {
        // SE�Đ��ƃG�t�F�N�g����
        // SE�Đ�
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // �G�t�F�N�g����
        if (hitEffectPrefab != null && effectSpawnPoint != null)
        {
            Instantiate(hitEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        }

        // �A�j���[�V������~�iplayer�j
        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        // �q�b�g�X�g�b�v
        Time.timeScale = 0f;

        // ���A���^�C���ő҂i�^�C���X�P�[��0�ł������悤�Ɂj
        yield return new WaitForSecondsRealtime(hitStopDuration);

        // �q�b�g�X�g�b�v����
        Time.timeScale = 1f;

        // �A�j���[�V�����ĊJ
        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f;
        }

        // snack ���̃q�b�g�X�g�b�v����
        snackObject.GetComponent<BlownAway_Ver3>()?.EndHitStop();

        // �J�����̓��ꎋ�_����
        cameraFunction?.StopSpecialView();
    }
}