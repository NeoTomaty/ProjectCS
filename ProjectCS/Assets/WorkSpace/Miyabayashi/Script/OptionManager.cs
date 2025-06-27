//======================================================
// OptionManager スクリプト
// 作成者：宮林
// 最終更新日：5/6
//
// [Log]5/6 宮林　オプション画面を実装
//      5/8 宮林　カメラ感度受け渡しを追加
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
        // スライダーに保存済み設定値を反映
        bgmVolumeSlider.value = GameSettingsManager.Instance.BgmVolume;
        seVolumeSlider.value = GameSettingsManager.Instance.SeVolume;
        sensitivitySlider.value = GameSettingsManager.Instance.Sensitivity;

        // volumeSetting が設定されている場合のみ反映
        if (volumeSetting != null)
        {
            volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
        }

        // cameraFunction が設定されている場合のみ反映
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