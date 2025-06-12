//====================================================
// �X�N���v�g���FUIOutlineGlow
// �쐬�ҁF����
// ���e�FUI�ɃA�E�g���C����_�ł�����
// �ŏI�X�V���F06/01
// 
// [Log]
// 06/01 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;
using UnityEngine.UI;

public class UIOutlineGlow : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Color glowColor = Color.cyan;  // ���̐F
    private Shadow outline;       // UI�ɒǉ�����Shadow�R���|�[�l���g


    private void Start()
    {
        outline = GetComponent<Shadow>();
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * speed * Mathf.PI * 2) + 1f) / 2f;
        outline.effectColor = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
    }
}