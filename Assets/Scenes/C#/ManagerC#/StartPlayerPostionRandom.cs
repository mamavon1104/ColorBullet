using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartPlayerPostionRandom : MonoBehaviour
{
    public GameObject[] players;
    Transform[] playerGameObjectArray; //プレイヤーがステージにどれくらいあるのか把握、番号の把握に使用。

    Transform[] playerSettingPos;
    private void Awake()
    { 
        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            var player = Instantiate(players[i]);
            player.transform.parent = gameObject.transform;
        }
    }
    private void Start()
    {
        var playerPostionsObj = GameObject.FindGameObjectsWithTag("SettingPos");
        playerSettingPos = new Transform[playerPostionsObj.Length];

        #region 普通にプレイヤーが開始した直後の移動にも使えそう、初期化プログラム
        playerGameObjectArray = new Transform[transform.childCount]; //実際の配列の初期化

        var childTemporaryArray = new Transform[playerGameObjectArray.Length]; //仮の配列の初期化
        var numOfTimesPutChild = new Dictionary<int, bool>(); //番号が被っているかどうかの確認[randomNum]を渡すことでfalseがだったらまだ使われていない。

        var i = 0; //現在のteleportArrayの番号;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //仮のPlayer全部入れる。
            numOfTimesPutChild.Add(Num, false); //ゲームObjectの番号(PlayerArray)の数だけ追加。
        }
        while (i < childTemporaryArray.Length)
        {
            int randomNum;//ランダム番目の数字
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //番号を変更する。
                playerSettingPos[i] = playerPostionsObj[randomNum].transform;

            } while (numOfTimesPutChild[randomNum] == true); //もしtrue(番号が被っていたら)

            playerGameObjectArray[i] = childTemporaryArray[randomNum];
            numOfTimesPutChild[randomNum] = true;
            i++;
        };
        #endregion

        for (int j = 0; j < playerSettingPos.Length; j++)
        {
            if (j == GameMaster.setPlayersNumMaster) 
                break;
            playerGameObjectArray[j].position = playerSettingPos[j].position;
        }
    }
}
