using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneLooper : MonoBehaviour
{
    public float delay = 10.0f;
    public string[] SceneNames;

    private void Start()
    {
        Invoke("LoadNextScene", delay);
    }

    void LoadNextScene()
    {
        string CurrentScene = SceneManager.GetActiveScene().name;

        int CurrentIndex = System.Array.IndexOf(SceneNames, CurrentScene);

        int NextIndex = (CurrentIndex + 1) % SceneNames.Length;

        SceneManager.LoadScene(SceneNames[NextIndex]);
    }
}