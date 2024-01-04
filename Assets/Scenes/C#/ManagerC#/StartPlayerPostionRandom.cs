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


        #region ���ʂɃv���C���[���J�n��������̈ړ��ɂ��g�������A�������v���O����
        var childTemporaryArray = new Transform[transform.childCount]; //���̔z��̏�����
        var numOfTimesPutChild = new Dictionary<int, bool>(); //�ԍ�������Ă��邩�ǂ����̊m�F[randomNum]��n�����Ƃ�false����������܂��g���Ă��Ȃ��B

        

        var teleportArrayNumNow = 0; //���݂�teleportArray�̔ԍ�;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //����Player�S�������B
            numOfTimesPutChild.Add(Num, false); //�Q�[��Object�̔ԍ�(PlayerArray)�̐������ǉ��B
        }
        while (teleportArrayNumNow < childTemporaryArray.Length)
        {
            int randomNum;//�����_���Ԗڂ̐���
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //�ԍ���ύX����B
                playerSettingPos[teleportArrayNumNow] = playerPostionsObj[randomNum].transform;

            } while (numOfTimesPutChild[randomNum] == true); //����true(�ԍ�������Ă�����)

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
