using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{

    public Text planetsDestroyedText; // �󂵂��f�����̕\���pText
    public Text distanceTraveledText; // �򋗗��̕\���pText

    private int planetsDestroyed;
    private float distanceTraveled;


    private void Update()
    {
      
    }

    // �O������Ăяo���Ēl��ݒ肷�郁�\�b�h
    public void SetDestroyPlanet(int destroyed)
    {
        planetsDestroyed = destroyed;

        // �e�L�X�g�ɔ��f
        if (planetsDestroyedText != null)
            planetsDestroyedText.text = "�󂵂��f����: " + planetsDestroyed.ToString();

    }

    public void SetDiatance( float distance)
    {
       
        distanceTraveled = distance;

        if (distanceTraveledText != null)
            distanceTraveledText.text = "�򋗗�: " + distanceTraveled.ToString("F1") + " m";
    }
}
