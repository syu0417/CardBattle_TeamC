using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize
{
    private const string InitializeSceneName = "AudioManager";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
        Debug.Log("RuntimeInitializeApplication");

        if (!SceneManager.GetSceneByName(InitializeSceneName).IsValid())
        {
            SceneManager.LoadScene(InitializeSceneName, LoadSceneMode.Additive);
        }
    }
}