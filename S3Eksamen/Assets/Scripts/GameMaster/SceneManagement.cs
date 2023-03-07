using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    private void Awake()
    {
        if (Instance == null && Instance != this)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Goes to Scene set in parameter and depedning on LoadSceneMode
    /// </summary>
    /// <param name="sceneName">Name of scene to go to</param>
    /// <param name="sceneMode">LoadSceneMode to use</param>
    public void GoToScene(string sceneName, LoadSceneMode sceneMode)
    {
        StartCoroutine(LoadSceneAsync(sceneName, sceneMode));
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode sceneMode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, sceneMode);

        FirebaseData.SetFirebaseData();

        GameMasterData.SetGMInstance();

        while (!asyncLoad.isDone)
            yield return null;
    }
}
