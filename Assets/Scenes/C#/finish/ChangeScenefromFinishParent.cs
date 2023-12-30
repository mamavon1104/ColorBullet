using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenefromFinishParent : MonoBehaviour
{
    bool[] children;
    GamePadPlayerNum gamePadPlayerNum = null;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            gamePadPlayerNum = GetComponent<GamePadPlayerNum>();
            children = new bool[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = false;
            }
        }
        else
        {
            children = new bool[GameMaster.setPlayersNumMaster];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = false;
            }
        }
    }
    public void ChangeFalse(Transform orderChild)
    {
        var num = -1;
        for (int i = 0; i < children.Length; i++)
        {
            if (orderChild == transform.GetChild(i))
            {
                num = i;
                break;
            }
        }
        children[num] = false;
    }

    public void ChangeTrue(Transform orderChild)
    {
        var num = -1;
        for (int i = 0; i < children.Length; i++)
        {
            if(orderChild == transform.GetChild(i))
            {
                num = i;
                break;
            }
        }

        children[num] = true;
        CheckAllIsTrue();
    }
    void CheckAllIsTrue()
    {
        int trueNum = 0;
        if (gamePadPlayerNum != null)
        {
            for (int i = 0; i < gamePadPlayerNum.playerNum; i++)
            {
                if (children[i] == true)
                    trueNum++;
                else
                    break;
            }
        }
        else
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] == true)
                    trueNum++;
                else
                    break;
            }
        }

        if (trueNum != GameMaster.setPlayersNumMaster)
            return;

        if (gamePadPlayerNum != null)
            gamePadPlayerNum.CanChange = false;
    
        StartCoroutine(ChangeScene());
    }
    IEnumerator ChangeScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
           GameObject.FindGameObjectWithTag("GameDirector").GetComponent<FirstSetDirector>().GameMasterSet();

        yield return new WaitForSeconds(1);
        GameMaster.audioManagerMaster.CheckAllAudio();
        yield return new WaitForSeconds(1.5f);
        if(SceneManager.GetActiveScene().name == "GameTitle")
        {
            Debug.Log(GameMaster.SceneManagerMaster);
            GameMaster.SceneManagerMaster.GamePlay();
            if (gamePadPlayerNum != null)
                gamePadPlayerNum.CanChange = true;
        }
        else 
        {
            GameMaster.DeleteStaticMember();
            GameMaster.SceneManagerMaster.ReturnGameTitle();
        }
    }
}