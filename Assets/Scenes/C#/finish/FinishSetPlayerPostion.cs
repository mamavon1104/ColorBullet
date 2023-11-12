using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishSetPlayerPostion : MonoBehaviour
{
    [SerializeField] GameObject[] playersPrafab;//Prefab
    [SerializeField] Transform[] playerTrans;//外
    [SerializeField] GameObject[] textObjects;
    [SerializeField] Material[] playerMaterials;
    [SerializeField] Color[] outLineColors;
    [SerializeField] TMP_FontAsset[] fontAssets;
    private void Start()
    {
        var playersPosition = new Transform[GameMaster.setPlayersNumMaster];
        var players = new GameObject[GameMaster.setPlayersNumMaster];

        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            playersPosition[i] = transform.GetChild(i);
        }

        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            Vector3 playerposVector3 = playerTrans[i].position;

            players[i] = Instantiate(playersPrafab[i], playerposVector3, Quaternion.identity);
            players[i].transform.parent = transform;
            GameObject.Destroy(players[i].transform.GetChild(1).GetComponent<PlayerController>());
            players[i].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }

        StartCoroutine(SetActiveTextAndPlayer());
        
        // 内部関数なのはスコープの関係上らしい。
        // 2023 11月七日　絶望している。
        IEnumerator SetActiveTextAndPlayer()
        {
            // 4つのint型配列を作成
            int[] scores = GameMaster.thisGamePlayersScore;
            int[] playersNum = new int[scores.Length];
            for (int i = 0; i < playersNum.Length; i++)
            {
                playersNum[i] = i;
            }
            // 配列を降順にソート
            int[] sortedScores = scores;
            Array.Sort(sortedScores,playersNum);
            Array.Reverse(sortedScores);
            Array.Reverse(playersNum);

            // 順位を格納する配列を作成
            int[] ranks = new int[GameMaster.setPlayersNumMaster];
            bool[] evenScore = new bool[ranks.Length];
            // 最初の要素は必ず1位とする
            ranks[0] = 1;

            // 次の要素から順番に比較して順位を決める
            for (int i = 1; i < GameMaster.setPlayersNumMaster; i++)
            {
                // 前の要素と同じスコアなら同じ順位とする
                if (sortedScores[i] == sortedScores[i - 1])
                {
                    ranks[i] = ranks[i - 1];
                    evenScore[i] = true;
                }
                // 前の要素よりスコアが低ければ、インデックス+1が順位となる
                else
                {
                    ranks[i] = i + 1;
                    evenScore[i] = false;
                }
            }

            // 順位とスコアとプレイヤー名を表示する,Textとかの関係のためここから
            for (int i = GameMaster.setPlayersNumMaster - 1; i > -1; i--)
            {
                // プレイヤー名はPlayer + インデックス+1とする
                string playerName = "Player " + playersNum[i];

                // 一つ前前の要素とスコアが異なる場合は1秒待機する
                if (i + 1 != GameMaster.setPlayersNumMaster)
                {
                    if (sortedScores[i] != sortedScores[(i + 1)])
                    {
                        yield return new WaitForSeconds(1.0f);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1.0f);
                }
                // 順位、スコア、プレイヤー名を表示する処理
                Debug.Log("<color=red>{0}位: {1}点 {2}" + ranks[i] + " " + sortedScores[i] + " " + playerName + "</color>");

                
                //textObject[下の方]をPlayer番号に書き換える
                textObjects[i].GetComponent<TextMeshProUGUI>().text = sortedScores[i].ToString() + " point";
                textObjects[i].transform.GetChild(0).GetComponent<Image>().material = playerMaterials[playersNum[i]];
                textObjects[i].transform.GetChild(1).GetComponent<TMP_Text>().font = fontAssets[playersNum[i]];
                textObjects[i].GetComponent<TMP_Text>().font = fontAssets[playersNum[i]];
                textObjects[i].SetActive(true);
                if (ranks[i] == 1)
                {
                    textObjects[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    players[playersNum[i]].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                }
                players[playersNum[i]].transform.position = playersPosition[i].position;
                if (!evenScore[i])
                {
                    GameMaster.audioManagerMaster.FinishAudioRank(ranks[i] - 1);
                }
            }
            GameMaster.canNotPlayersMove = false;
        }
    }
}