using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{

    public Text planetsDestroyedText; // 壊した惑星数の表示用Text
    public Text distanceTraveledText; // 飛距離の表示用Text

    private int planetsDestroyed;
    private float distanceTraveled;


    private void Update()
    {
      
    }

    // 外部から呼び出して値を設定するメソッド
    public void SetDestroyPlanet(int destroyed)
    {
        planetsDestroyed = destroyed;

        // テキストに反映
        if (planetsDestroyedText != null)
            planetsDestroyedText.text = "壊した惑星数: " + planetsDestroyed.ToString();

    }

    public void SetDiatance( float distance)
    {
       
        distanceTraveled = distance;

        if (distanceTraveledText != null)
            distanceTraveledText.text = "飛距離: " + distanceTraveled.ToString("F1") + " m";
    }
}
