//======================================================
// [GameClearSequence_Ver2]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/13
// 
// [Log]
// 06/13�@�r��@�X�i�b�N�̒��g����юU��G�t�F�N�g�̍Đ�����������
// 06/13�@�r��@�p�[�e�B�N���̎ˏo���x�Ɖ�]���x�������_���ɐݒ肷�鏈����ǉ�
//======================================================
using UnityEngine;

// �V�����N���A���o
public class GameClearSequence_Ver2 : MonoBehaviour
{
    [Header("�G�t�F�N�g�̐ݒ�")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    [Header("�p�[�e�B�N���̃��b�V��")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    [Header("�p�[�e�B�N���̃p�����[�^")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float SpeedMIN = 0.5f;
    [SerializeField] float SpeedMAX = 1.5f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    // �N���A���o�J�n
    public void Play()
    {
        if (SnackEffect == null || ParticleMesh == null || ParticleMaterial == null) return;

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
        // �T�C�Y
        PSMain.startSize = Size;

        // �ˏo���x
        float min = SpeedMIN;
        float max = SpeedMAX;
        PSMain.startSpeed = new ParticleSystem.MinMaxCurve(min, max);

        // ��]���x
        var Rotation = PS.rotationOverLifetime;
        min = RotateSpeedMIN * Mathf.Deg2Rad;
        max = RotateSpeedMAX * Mathf.Deg2Rad;
        Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.z = new ParticleSystem.MinMaxCurve(min, max);

        // �G�t�F�N�g�Đ�
        PS.Play();
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
