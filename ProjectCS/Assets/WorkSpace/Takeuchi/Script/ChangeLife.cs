using UnityEngine;
public class ChangeLife : MonoBehaviour
{
    public LifeManager LifeManager;
    public string TagName = "";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName))  // ‚Ü‚½‚Í‘¼‚ÌTag
        {
            LifeManager.DecreaseLife();
        }
    }
}
