using UnityEngine;

public class BGMPlayer : MonoBehaviour
{

    public AudioClip BGMClip;    //BGM
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(BGMClip); // AudioClipを用意してInspectorで設定
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
