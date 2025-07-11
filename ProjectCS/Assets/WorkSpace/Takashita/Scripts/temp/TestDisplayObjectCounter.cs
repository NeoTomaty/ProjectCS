using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestDisplayObjectCounter : MonoBehaviour
{
    [SerializeField] private Text text;

    void Update()
    {
        int testcount1 = Object.FindObjectsByType<MovePlayer>(FindObjectsSortMode.None).Length;
        int testcount2 = Object.FindObjectsByType<BlownAway_Ver3>(FindObjectsSortMode.None).Length;
        text.text = CountAllObjectsInHierarchy().ToString() + "/" + testcount1 + "/" + testcount2;
    }

    int CountAllObjectsInHierarchy()
    {
        int total = 0;
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject root in rootObjects)
        {
            total += CountRecursively(root);
        }

        return total;
    }

    int CountRecursively(GameObject obj)
    {
        int count = 1; // 自分自身をカウント

        foreach (Transform child in obj.transform)
        {
            count += CountRecursively(child.gameObject);
        }

        return count;
    }
}
