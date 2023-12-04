using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public void GoGameResult()
    {
        SceneManager.LoadScene("GameResult");
    }
    public void GamePlay()
    {
       SceneManager.LoadScene("PlayGame");
    }
    public void ReturnGameTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
}
