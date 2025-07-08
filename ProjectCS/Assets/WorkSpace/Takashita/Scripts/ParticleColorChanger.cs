using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
    public Color newColor = Color.cyan; // ç∑Çµë÷Ç¶ÇΩÇ¢êF
    private const float WhiteTolerance = 0.01f; // îíêFîªíËÇÃãñóeåÎç∑

    void Start()
    {
       SetColor(newColor);
    }

    public void SetColor(Color NewColor)
    {
        NewColor.a = 1f;

        ParticleSystem[] allParticles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in allParticles)
        {

            // Color over Lifetime
            var col = ps.colorOverLifetime;
            if (col.enabled)
            {
                Gradient original = col.color.gradient;
                GradientColorKey[] originalColors = original.colorKeys;
                GradientAlphaKey[] originalAlphas = original.alphaKeys;

                GradientColorKey[] newColors = new GradientColorKey[originalColors.Length];

                for (int i = 0; i < originalColors.Length; i++)
                {
                    Color orig = originalColors[i].color;

                    bool isWhite = Mathf.Abs(orig.r - 1f) < WhiteTolerance &&
                                   Mathf.Abs(orig.g - 1f) < WhiteTolerance &&
                                   Mathf.Abs(orig.b - 1f) < WhiteTolerance;

                    Color newCol = isWhite ? orig : NewColor;
                    newCol.a = orig.a;

                    newColors[i] = new GradientColorKey(newCol, originalColors[i].time);
                }

                Gradient result = new Gradient();
                result.SetKeys(newColors, originalAlphas);
                col.color = result;
            }
        }
    }
}