using UnityEngine;

public class BGMPlayer : MonoBehaviour
{

    public AudioClip BGMClip;    //BGM
    [Range(0.0f, 1.0f)]
    [SerializeField] private float BGMVolume = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(BGMClip, BGMVolume); // AudioClipÇópà”ÇµÇƒInspectorÇ≈ê›íË
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
