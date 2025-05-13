using UnityEngine;

public class BGMPlayer : MonoBehaviour
{

    public AudioClip BGMClip;    //BGM
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(BGMClip); // AudioClip��p�ӂ���Inspector�Őݒ�
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
