//======================================================
// [NPC_TrailColor]
// çÏê¨é“ÅFêXòe
// ç≈èIçXêVì˙ÅF04/22
//
// [Log]
// 04/12Å@êXòe NPCÇÃãOê’Çé¿ëï
//======================================================

using UnityEngine;

public class NPC_TrailColor : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ReflectingNPC npc;

    [SerializeField] private Color lowSpeedColor = Color.blue;
    [SerializeField] private Color middleSpeedColor = Color.yellow;
    [SerializeField] private Color highSpeedColor = Color.red;
    [SerializeField] private float alpha = 1.0f;

    [SerializeField] private float lowToMidThreshold = 4f;
    [SerializeField] private float midToHighThreshold = 8f;

    private Gradient gradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;

    private void Start()
    {
        if (trailRenderer == null) return;

        gradient = new Gradient();

        colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f;
        colorKeys[1].time = 1.0f;

        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = alpha;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = alpha;
        alphaKeys[1].time = 1.0f;
    }

    private void Update()
    {
        if (trailRenderer == null || npc == null) return;

        float speed = npc.GetCurrentSpeed;

        if (speed < lowToMidThreshold)
        {
            colorKeys[0].color = lowSpeedColor;
            colorKeys[1].color = lowSpeedColor;
        }
        else if (speed < midToHighThreshold)
        {
            colorKeys[0].color = middleSpeedColor;
            colorKeys[1].color = middleSpeedColor;
        }
        else
        {
            colorKeys[0].color = highSpeedColor;
            colorKeys[1].color = highSpeedColor;
        }

        gradient.SetKeys(colorKeys, alphaKeys);
        trailRenderer.colorGradient = gradient;
    }
}