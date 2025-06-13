//======================================================
// [GameClearSequence_Ver2]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/13
// 
// [Log]
// 06/13�@�r��@�X�i�b�N�̒��g����юU��G�t�F�N�g�̍Đ�����������
//======================================================
using UnityEngine;

// �V�����N���A���o
public class GameClearSequence_Ver2 : MonoBehaviour
{
    // �G�t�F�N�g
    [Header("�G�t�F�N�g�̐ݒ�")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    // �p�[�e�B�N���̃��b�V��
    [Header("�p�[�e�B�N���̃��b�V��")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    // �p�[�e�B�N���̐���
    [Header("�p�[�e�B�N���̃p�����[�^")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float Speed = 1.0f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    // �N���A���o�J�n
    public void Play()
    {
        // �G�t�F�N�g���Đ�
        if (SnackEffect == null) return;

        // �G�t�F�N�g����
        GameObject Effect = Instantiate(SnackEffect, new Vector3(0f, 0f, 0f), Quaternion.identity);

        // �G�t�F�N�g�T�C�Y�ݒ�
        Effect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

        // �p�[�e�B�N�����b�V���ݒ�
        ParticleSystem PS = Effect.GetComponent<ParticleSystem>();
        var PSRenderer = PS.GetComponent<ParticleSystemRenderer>();
        PSRenderer.mesh = ParticleMesh;
        PSRenderer.material = ParticleMaterial;

        // �p�[�e�B�N���p�����[�^�ݒ�
        var PSMain = PS.main;
        PSMain.startSize = Size;    // �T�C�Y
        PSMain.startSpeed = Speed;  // �ˏo���x
        var Rotation = PS.rotationOverLifetime; // ��]���x
        float min = RotateSpeedMIN * Mathf.Deg2Rad;
        float max = RotateSpeedMAX * Mathf.Deg2Rad;
        Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.z = new ParticleSystem.MinMaxCurve(min, max);


        PS.Play();

        // �J��������
    }

    // �N���A���o�I��
    public void End()
    {
        // �N���A���o���I�����鏈���������ɋL�q
        // �Ⴆ�΁AUI�̔�\���⎟�̃V�[���ւ̑J�ڂȂ�
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
