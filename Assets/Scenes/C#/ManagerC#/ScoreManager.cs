using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    [SerializeField] GameObject playersMaster;
    
    PlayerController[] playerControllers;//�����̂���
    private int bestScore = 0; //�ő�l
    public int[] playersPoint; //���݂̓_��
    bool[] isThisPlayerNumber1;//true�Ȃ�ꗬ�A��������΋�

    private int Number1PlayersNum;//1�ʂ����l���܂����H
    
    TextManager textManager;
    private void Start()
    {
        Number1PlayersNum = GameMaster.setPlayersNumMaster; //�l���A�_������Ԃł����l�B
        textManager = gameObject.GetComponent<TextManager>();
        
        playersPoint = new int[Number1PlayersNum];
        isThisPlayerNumber1 = new bool[Number1PlayersNum];
        playerControllers = new PlayerController[Number1PlayersNum];

        for (int i = 0; i < isThisPlayerNumber1.Length; i++)
        {
            playersPoint[i] = 0;
            isThisPlayerNumber1[i] = true;//�ŏ��͑S���������Ă�B
            playerControllers[i] = FindPlayerContoroller(i);
        }

        for (int i = 0; i < playerControllers.Length; i++)
        {
            Debug.Log("<color=yellow>" + playersMaster.transform.GetChild(i).name + "</color>");
            Debug.Log(playersMaster);
            Debug.Log("<color=yellow>" + playerControllers[i] +"</color>");
        }
    }

    public void AddScorePlayer(string team)
    {
        if (GameMaster.canNotPlayersMove == true)
            return;

        var playerNum = GameMaster.GetTeamID(team);   
        var _bestScore = bestScore;
        var _Number1PlayersNum = Number1PlayersNum;
        playersPoint[playerNum]++;
        bestScore = FindBestScore();


        Debug.Log("<color=cyan>" + _Number1PlayersNum  + "</color>");
        Debug.Log("<color=cyan>" + Number1PlayersNum + "</color>");
        Debug.Log("<color=blue>" + bestScore + "</color>");

        //�������ő�ɂȂ�A�_���̕ϓ����N���ĂȂ��B�������̓x�X�g�X�R�A�̐l�����ς������(������ς�����)
        if ((playersPoint[playerNum] == bestScore && (bestScore == _bestScore)) || Number1PlayersNum != _Number1PlayersNum)
        {
            for (int i = 0; i < isThisPlayerNumber1.Length; i++)
            {
                if (playersPoint[i] < bestScore) isThisPlayerNumber1[i] = false;
                else isThisPlayerNumber1[i] = true;
                playerControllers[i].SetActiveCrown(isThisPlayerNumber1[i]);
            }
            textManager.SetActiveCrown(isThisPlayerNumber1);
        }
        textManager.WritePlayersScore(playerNum, playersPoint[playerNum]);
    }
    int FindBestScore()
    {
        int maxNum = playersPoint[0];
        Number1PlayersNum = 0;
        
        for (int i = 1; i < playersPoint.Length; i++)
        {
            if (playersPoint[i] > maxNum)
            {
                maxNum = playersPoint[i];
            }
        }

        //�ő�lMax�����܂�����
        for (int i = 0; i < playersPoint.Length; i++)
        {
            Debug.Log("<color=red>" + playersPoint[i] + "</color>");
            if (playersPoint[i] == maxNum)
            {
                Number1PlayersNum++;
            }
        }

        return maxNum;//��ʂ��܂��_���𑝂₵����_bestScore+1�_�����A���Ă��� = (_best != best) = ���̃v���C���[�͓��������ɂ͂Ȃ�Ȃ��B
    }

    PlayerController FindPlayerContoroller(int playersNum)
    {
        PlayerController playerController = null;
        Transform playerParent = playersMaster.transform.GetChild(playersNum);
        for (int i = 0; i < playerParent.childCount; i++)
        {
            if (GameMaster.GetGameObjectID(playerParent.GetChild(i).gameObject) == 0)
            {
                playerController = playerParent.GetChild(i).GetComponent<PlayerController>();
                break;
            }
        }
        return playerController;
    }
}
