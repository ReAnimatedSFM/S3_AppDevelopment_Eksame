using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    public GameObject SelectionWindow;

    public GameObject HighscoreWindow;

    private void Awake()
    {
        OpenSelectionWindow();
    }

    public void LogOut()
    {
        SceneManagement.Instance.GoToScene("LoginScene", LoadSceneMode.Single);
        FirebaseManager.Instance.SignOut();
    }

    public void OpenSelectionWindow()
    {
        SelectionWindow.SetActive(true);
        HighscoreWindow.SetActive(false);
    }

    public void OpenHighscoreWindow()
    {
        HighscoreWindow.SetActive(true);
        SelectionWindow.SetActive(false);
    }

    public void StartGame()
    {
        SceneManagement.Instance.GoToScene("PlayScene", LoadSceneMode.Single);
    }
}
