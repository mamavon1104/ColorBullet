using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartPlayerPostionRandom : MonoBehaviour
{
    public GameObject[] players;
    Transform[] playerGameObjectArray; //�v���C���[���X�e�[�W�ɂǂꂭ�炢����̂��c���A�ԍ��̔c���Ɏg�p�B

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

        #region ���ʂɃv���C���[���J�n��������̈ړ��ɂ��g�������A�������v���O����
        playerGameObjectArray = new Transform[transform.childCount]; //���ۂ̔z��̏�����

        var childTemporaryArray = new Transform[playerGameObjectArray.Length]; //���̔z��̏�����
        var numOfTimesPutChild = new Dictionary<int, bool>(); //�ԍ�������Ă��邩�ǂ����̊m�F[randomNum]��n�����Ƃ�false����������܂��g���Ă��Ȃ��B

        var i = 0; //���݂�teleportArray�̔ԍ�;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //����Player�S�������B
            numOfTimesPutChild.Add(Num, false); //�Q�[��Object�̔ԍ�(PlayerArray)�̐������ǉ��B
        }
        while (i < childTemporaryArray.Length)
        {
            int randomNum;//�����_���Ԗڂ̐���
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //�ԍ���ύX����B
                playerSettingPos[i] = playerPostionsObj[randomNum].transform;

            } while (numOfTimesPutChild[randomNum] == true); //����true(�ԍ�������Ă�����)

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
