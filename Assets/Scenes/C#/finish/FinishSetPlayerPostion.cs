using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishSetPlayerPostion : MonoBehaviour
{
    [SerializeField] GameObject[] playersPrafab;//Prefab
    [SerializeField] Transform[] playerTrans;//�O
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
        
        // �����֐��Ȃ̂̓X�R�[�v�̊֌W��炵���B
        // 2023 11�������@��]���Ă���B
        IEnumerator SetActiveTextAndPlayer()
        {
            // 4��int�^�z����쐬
            int[] scores = GameMaster.thisGamePlayersScore;
            int[] playersNum = new int[scores.Length];
            for (int i = 0; i < playersNum.Length; i++)
            {
                playersNum[i] = i;
            }
            // �z����~���Ƀ\�[�g
            int[] sortedScores = scores;
            Array.Sort(sortedScores,playersNum);
            Array.Reverse(sortedScores);
            Array.Reverse(playersNum);

            // ���ʂ��i�[����z����쐬
            int[] ranks = new int[GameMaster.setPlayersNumMaster];
            bool[] evenScore = new bool[ranks.Length];
            // �ŏ��̗v�f�͕K��1�ʂƂ���
            ranks[0] = 1;

            // ���̗v�f���珇�Ԃɔ�r���ď��ʂ����߂�
            for (int i = 1; i < GameMaster.setPlayersNumMaster; i++)
            {
                // �O�̗v�f�Ɠ����X�R�A�Ȃ瓯�����ʂƂ���
                if (sortedScores[i] == sortedScores[i - 1])
                {
                    ranks[i] = ranks[i - 1];
                    evenScore[i] = true;
                }
                // �O�̗v�f���X�R�A���Ⴏ��΁A�C���f�b�N�X+1�����ʂƂȂ�
                else
                {
                    ranks[i] = i + 1;
                    evenScore[i] = false;
                }
            }

            // ���ʂƃX�R�A�ƃv���C���[����\������,Text�Ƃ��̊֌W�̂��߂�������
            for (int i = GameMaster.setPlayersNumMaster - 1; i > -1; i--)
            {
                // �v���C���[����Player + �C���f�b�N�X+1�Ƃ���
                string playerName = "Player " + playersNum[i];

                // ��O�O�̗v�f�ƃX�R�A���قȂ�ꍇ��1�b�ҋ@����
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
                // ���ʁA�X�R�A�A�v���C���[����\�����鏈��
                Debug.Log("<color=red>{0}��: {1}�_ {2}" + ranks[i] + " " + sortedScores[i] + " " + playerName + "</color>");

                
                //textObject[���̕�]��Player�ԍ��ɏ���������
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