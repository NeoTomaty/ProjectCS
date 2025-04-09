//�X�N���v�g��:SpeedManager.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/01
//[Log]
//2025/04/01�@��������@�v���C���[�̍ő呬�x��100�ȏ�ɂ��Ȃ�����

using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    //SpeedManager�̃C���X�^���X��ێ�����ÓI�v���p�e�B
    public static SpeedManager Instance { get; private set; }

    //�v���C���[�̍ő呬�x��ݒ肷��ϐ�
    public float MaxSpeed = 100.0f;

    //Awake���\�b�h�̓I�u�W�F�N�g���L���ɂȂ�Ƃ��ɌĂяo�����
    void Awake()
    {
        //�C���X�^���X���܂��ݒ肳��Ă��Ȃ��Ƃ�
        if(Instance == null)
        {
            //���̃I�u�W�F�N�g���C���X�^���X�Ƃ��Đݒ�
            Instance = this;

            //���̃I�u�W�F�N�g���V�[���ԂŔj������Ȃ��悤�ɐݒ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //���łɃC���X�^���X�����݂���Ƃ��A���̃I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }

    //�v���C���[�̑��x�𑝉������郁�\�b�h
    public void IncreaseSpeed(ref float Speed)
    {
        //���݂̑��x��5.0f�����Z���A�ő呬�x�𒴂��Ȃ��悤�ɐ���
        Speed = Mathf.Min(Speed + 5.0f, MaxSpeed);
        Debug.Log("������:" + Speed);
    }
}