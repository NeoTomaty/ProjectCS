//======================================================
// OptionManager �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/6
//
// [Log]5/6 �{�с@�I�v�V������ʂ�����
//      5/8 �{�с@�J�������x�󂯓n����ǉ�
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
   
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider seVolumeSlider;

    [SerializeField] private CameraFunction cameraFunction;
    [SerializeField] private VolumeSetting volumeSetting;


    private void Start()
    {
        // �X���C�_�[�ɕۑ��ςݐݒ�l�𔽉f
        bgmVolumeSlider.value = GameSettingsManager.Instance.BgmVolume;
        seVolumeSlider.value = GameSettingsManager.Instance.SeVolume;
        sensitivitySlider.value = GameSettingsManager.Instance.Sensitivity;

        // volumeSetting ���ݒ肳��Ă���ꍇ�̂ݔ��f
        if (volumeSetting != null)
        {
            volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
        }

        // cameraFunction ���ݒ肳��Ă���ꍇ�̂ݔ��f
        if (cameraFunction != null)
        {
            cameraFunction.SetRatio(sensitivitySlider.value);
        }
    }

   

    public float GetBgmVolume()
    {
        return GameSettingsManager.Instance.BgmVolume;
    }
    public float GetSeVolume()
    {
        return GameSettingsManager.Instance.SeVolume;
    }

    public float GetSensitivity()
    {
        return GameSettingsManager.Instance.Sensitivity;
    }



}