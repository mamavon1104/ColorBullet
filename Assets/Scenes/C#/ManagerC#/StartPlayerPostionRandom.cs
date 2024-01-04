using System.Collections.Generic;
using UnityEngine;

public class StartPlayerPostionRandom : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    Transform[] playerSettingPos;
    [SerializeField] GameObject[] playerPostionsObj;
    Transform[] playersInstantite = new Transform[GameMaster.setPlayersNumMaster];
    private void Awake()
    {
        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            playersInstantite[i] = Instantiate(players[i], Vector3.one, Quaternion.identity).transform;
            playersInstantite[i].parent = transform.GetChild(i).transform;
        }
    }
    private void Start()
    {
        playerPostionsObj = GameObject.FindGameObjectsWithTag("SettingPos");
        playerSettingPos = new Transform[playerPostionsObj.Length];


        #region 普通にプレイヤーが開始した直後の移動にも使えそう、初期化プログラム
        var childTemporaryArray = new Transform[transform.childCount]; //仮の配列の初期化
        var numOfTimesPutChild = new Dictionary<int, bool>(); //番号が被っているかどうかの確認[randomNum]を渡すことでfalseがだったらまだ使われていない。

        

        var teleportArrayNumNow = 0; //現在のteleportArrayの番号;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //仮のPlayer全部入れる。
            numOfTimesPutChild.Add(Num, false); //ゲームObjectの番号(PlayerArray)の数だけ追加。
        }
        while (teleportArrayNumNow < childTemporaryArray.Length)
        {
            int randomNum;//ランダム番目の数字
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //番号を変更する。
                playerSettingPos[teleportArrayNumNow] = playerPostionsObj[randomNum].transform;

            } while (numOfTimesPutChild[randomNum] == true); //もしtrue(番号が被っていたら)

            numOfTimesPutChild[randomNum] = true;
            teleportArrayNumNow++;
        };
        #endregion
        

        for (int i = 0; i < playerSettingPos.Length; i++)
        {
            if (i == GameMaster.setPlayersNumMaster) 
                break;

            playersInstantite[i].position = playerSettingPos[i].position;
        }
    }
}
