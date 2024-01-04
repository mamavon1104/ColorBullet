using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    [SerializeField] GameObject playersMaster;
    
    PlayerController[] playerControllers;//王冠のため
    private int bestScore = 0; //最大値
    bool[] isThisPlayerNumber1;//trueなら一流、ちがければ愚民
    int[] playersPoint; //現在の点数

    private int Number1PlayersNum;//1位が何人いますか？
    
    TextManager textManager;
    private void Start()
    {
        Number1PlayersNum = GameMaster.setPlayersNumMaster; //人数、点数が一番でかい人。
        textManager = gameObject.GetComponent<TextManager>();
        
        playersPoint = new int[GameMaster.setPlayersNumMaster];
        isThisPlayerNumber1 = new bool[GameMaster.setPlayersNumMaster];
        playerControllers = new PlayerController[GameMaster.setPlayersNumMaster];

        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            playersPoint[i] = 0;
            isThisPlayerNumber1[i] = true;//最初は全員王冠ついてる。
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
        //(bestScoreが同じで点数が追い付いた時)もしくはベストスコアの人数が変わったら(王冠を変えたい)
        if ((playersPoint[playerNum] == bestScore && (bestScore == _bestScore)) || Number1PlayersNum != _Number1PlayersNum)
        {
            for (int i = 0; i < Number1PlayersNum; i++)
            {
                if (playersPoint[i] < bestScore) isThisPlayerNumber1[i] = false;
                else isThisPlayerNumber1[i] = true;

                Debug.Log(playerControllers[i]);
                Debug.Log(bestScore);

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

        //最大値Maxが決まった後
        for (int i = 0; i < playersPoint.Length; i++)
        {
            Debug.Log("<color=red>" + playersPoint[i] + "</color>");
            if (playersPoint[i] == maxNum)
            {
                Number1PlayersNum++;
            }
        }

        return maxNum;//一位がまた点数を増やしたら_bestScore+1点数が帰ってくる = (_best != best) = 他のプレイヤーは同じ数字にはならない。
    }

    PlayerController FindPlayerContoroller(int playersNum)
    {
        PlayerController playerController = null;
        Transform playerParent = playersMaster.transform.GetChild(playersNum).GetChild(0);
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
    public void SetGameMasterScore()
    {
        GameMaster.thisGamePlayersScore = GetComponent<ScoreManager>().playersPoint;
    }
}
