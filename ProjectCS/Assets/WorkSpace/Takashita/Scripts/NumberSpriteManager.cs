using UnityEngine;
using UnityEngine.UI;

public class NumberSpriteManager : MonoBehaviour
{
    [SerializeField] private Sprite[] NumberSprites = new Sprite[10];
    private Image NumberImage;

    void Start()
    {
        NumberImage = GetComponent<Image>();
    }

    public void ChangeNumberSprite(int num)
    {
        if(num > 9 || num < 0)
        {
            num = 0;
            Debug.Log("ChangeNumberSprite‚É0‚©‚ç9ˆÈŠO‚ª“n‚³‚ê‚½‚½‚ß0‚É•ÏŠ·");
        }
        NumberImage.sprite = NumberSprites[num];
    }

    public void SetAlpha(float alpha)
    {
        Color color = NumberImage.color;
        color.a = alpha;
        NumberImage.color = color;
    }
}
