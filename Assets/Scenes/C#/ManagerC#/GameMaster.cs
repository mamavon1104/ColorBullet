using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameMaster
{
    //�������C�����Ȃ���
    public static Dictionary<int, InputDevice> gamepadMaster = new Dictionary<int, InputDevice>();


    public static int setCanShotBulletMaster;//�����ōő剽�A���܂őłĂ邩
    public static float setBulletSpeedMaster; //�e�e�̑��x
    public static float setDashTimeMaster; //�ǂ̂��炢�̎��ԂŃ_�b�V�����\�ɂȂ邩
    public static float setTeleportTimeMaster; //�e���|�[�g���鎞��
    public static float setCannonShotCoolTimeMaster; //Cannon�̔��˂��鑬�x
    public static int setCanBounceMaster; // �ʂ̒��˂�񐔁B
    public static bool doSetAllTeleportersSame; //�S�Ẵe���|�[�^�[��ID�𓯂��ɂ��邩�B
    public static int setPlayersNumMaster;
    public static int setGameTimerMaster;

    public static bool canNotPlayersMove;

    public static GameObject gameDirectorMaster;
    public static AudioManager audioManagerMaster;
    public static ScoreManager scoreManagerMaster;
    public static ChangeSceneManager SceneManagerMaster;

    public static int[] thisGamePlayersScore;

    static Dictionary<string, int> teamIDDictionary = new Dictionary<string, int>(); // team �� IDv1

    static Dictionary<GameObject, int> gameObjectIDDictionary = new Dictionary<GameObject, int>();// GameObject �� IDv2
    /*
        ID���Ǘ�����Dictionary�ϐ��A�ȉ�ID������킷Object
        {
            ID -01 : Wall(����Ȃ��q),
            ID  00 : Player,
            ID  01 : Bullet,
            ID  02 : Cannon,
            ID  03 : Teleport
        }

        ���̎���enum���m��Ȃ������̂���
    �@�@�ł��C�ɂ����R�[�h�����Ă�����������������
    */
    static List<GameObject> bulletOlderList = new List<GameObject>(); //�ǂ����̂ق��������ł��ꂽ����ۑ��AIndexOf���ő������擾

    static public void AddGamePad(Dictionary<int,InputDevice> dic)
    {
        gamepadMaster = dic;
    }
    static public void AddTeamID(string team)
    {
        if (teamIDDictionary.ContainsKey(team))
            return;
        teamIDDictionary.Add(team,teamIDDictionary.Count); //Player��Tag��ID�ł̊Ǘ��̏����B
    }
    static public int GetTeamID(string team)
    {
        return teamIDDictionary[team];
    }
    public static void AddGameObjectID(GameObject getGameObject, int gameObjectID) //�V�����ǉ�����֐�
    {
        //����������Ƃ��ɂ����ɂ����n���B
        gameObjectIDDictionary.Add(getGameObject, gameObjectID);
        //����ID�̔ԍ����e�e��������
        if (gameObjectID == 1)
            bulletOlderList.Add(getGameObject); // �e�e��List���ǉ����܂��B
    }
    public static int GetGameObjectID(GameObject getGameObject) //�ԍ����擾����֐�
    {
        if(gameObjectIDDictionary.ContainsKey(getGameObject) == false)
        {
            return -1; //�Ȃ��Ȃ�d���Ȃ��B�ǔ����Ԃ��B
        }
        else
        {
            return gameObjectIDDictionary[getGameObject];
        }
    }
    public static bool IsBulletNewerBool(GameObject toOrderBulletobject, GameObject otherBulletObject) //�ǂ����̂ق����V����������֐�
    {
        int toOrderBulletNum = bulletOlderList.IndexOf(toOrderBulletobject); //����(������Ăяo���Ă���)�I�u�W�F�N�g�̔ԍ��擾
        int otherBulletNum = bulletOlderList.IndexOf(otherBulletObject);�@�@ //�Ԃ������I�u�W�F�N�g�̔ԍ��擾

        bool amInewerThanOther;  //�Ԃ��z

        if (toOrderBulletNum > otherBulletNum) //���������̔ԍ��̕����傫��������(�V���������傫��)
        {
            amInewerThanOther = true; //�V������
        }
        else //����������傫���Ȃ�������
        {
            amInewerThanOther = false;//���Õi��
        }
        return amInewerThanOther;�@//�Ԃ��܂�
    }

    public static IEnumerator SettingWhenDeleteGameObject(GameObject deletedGameObject) //���S���Ȃ�ɂȂ�ꂽ�ۂ̐ݒ���������܂��B�ƂĂ��������ł����ˁB
    {
        yield return new WaitForSeconds(2.0f);
        if (gameObjectIDDictionary[deletedGameObject] == 1)//�����ނ炪�e�e�Ƃ��Đ������̂Ȃ��
            bulletOlderList.Remove(deletedGameObject); //�e�eList��������݂𔍒D�B
        gameObjectIDDictionary.Remove(deletedGameObject); //�Q�[���I�u�W�F�N�g�Ƃ��Ă�ID�𔍒D
    }
    public static void DeleteStaticMember()
    {
        //���L��
        bulletOlderList.Clear();
        gameObjectIDDictionary.Clear();
    }
}
