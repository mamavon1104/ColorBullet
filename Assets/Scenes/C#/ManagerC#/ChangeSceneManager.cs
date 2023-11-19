using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    private bool canGotoGame = false;
    public bool CanGotoGame
    {
        get { return canGotoGame; }
        set { canGotoGame = value; }
    }
    [SerializeField] GameObject CantGoUI;
    private void Awake()
    {
        GameMaster.SceneManagerMaster = this;
    }
    public void GoGameResult()
    {
        SceneManager.LoadScene("GameResult");
    }

    public void GamePlay()
    {
        if (canGotoGame)
            SceneManager.LoadScene("PlayGame");
        else
        {
            gameObject.GetComponent<UIAudioManager>().CantAudio();
            StartCoroutine(SetUIinCantGo());
        }
    }
    IEnumerator SetUIinCantGo()
    {
        CantGoUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        CantGoUI.SetActive(false);
    }
    public void ReturnGameTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }
    public void GoSettingScene()
    {
        SceneManager.LoadScene("SetScene");
    }
}
