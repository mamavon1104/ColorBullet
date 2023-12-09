using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleporterParent : MonoBehaviour
{
    Transform[] teleportGameObjectArray; //�e���|�[�g���X�e�[�W�ɂǂꂭ�炢����̂��c���A�ԍ��̔c���Ɏg�p�B
    
    Dictionary<Transform, int> teleportIDDicitonary = new Dictionary<Transform, int>(); //ID���q�I�u�W�F�N�g��Transform�ŊǗ�    
    Dictionary<Transform, TeleporterChild> teleportChildDictionary = new Dictionary<Transform, TeleporterChild>(); //TransformChildCS���q�I�u�W�F�N�g��Transform�ŊǗ�
    bool[] isNowMovingVFXbool; //���̃y�A�̃e���|�[�^�[������VFX�͓����Ă��邩�ǂ���������B

    float[] teleportTimer;//�e���|�[�g�̃^�C�}�[�z��
    float setTeleportTime;//�Z�b�g���鎞�ԁB

    bool areTeleporterOddNumBool = false; //�e���|�[�g���Ċ�H
    [SerializeField] Material[] teleporterMat;//�e���|�[�^�[��p�}�e���A���̔�
    [SerializeField] Color[] childVFXColors;
    private void Awake()
    {
        setTeleportTime = GameMaster.setTeleportTimeMaster;
        #region ���ʂɃv���C���[���J�n��������̈ړ��ɂ��g�������A�������v���O�����B����C����
        Debug.Log(transform.childCount);
        teleportGameObjectArray = new Transform[transform.childCount]; //���ۂ̔z��̏�����

        var childTemporaryArray = new Transform[teleportGameObjectArray.Length]; //���̔z��̏�����
        var numOfTimesPutChild = new Dictionary<int, bool>(); //�ԍ�������Ă��邩�ǂ����̊m�F[randomNum]��n�����Ƃ�false����������܂��g���Ă��Ȃ��B

        var i = 0; //���݂�teleportArray�̔ԍ�;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //���̔��Ƀe���|�[�g�S�������B
            numOfTimesPutChild.Add(Num, false); //�Q�[��Object�̔ԍ�(teleportArray)�̐������ǉ��B

            //Debug.Log(Num);
            //Debug.Log(Num.ToString() + numOfTimesPutChild[i]);
        }
        while (i < childTemporaryArray.Length)
        {
            int randomNum;//�����_���Ԗڂ̐���
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //�ԍ���ύX����B
                //Debug.Log(numOfTimesPutChild[randomNum] + " randomNum : " + randomNum);

            } while (numOfTimesPutChild[randomNum] == true); //����true(�����g�p�ς�)�Ȃ�J��Ԃ��B

            teleportGameObjectArray[i] = childTemporaryArray[randomNum];

            numOfTimesPutChild[randomNum] = true;
            i++;
        };

        if (!areTeleporterOddNumBool && !GameMaster.doSetAllTeleportersSame) teleportTimer = new float[(teleportGameObjectArray.Length / 2)]; //�y�A���Ƃ̃^�C�}�[�����
        else teleportTimer = new float[1]; //�������肽��

        isNowMovingVFXbool = new bool[teleportTimer.Length];//�e���|�[�g�^�C�}�[��0�ɂȂ����瓮���A��������������̂ł����̐��̗ʂ����K�v�B
        #endregion
    }
    private void Start()
    {
        for (int i = 0; i < teleportGameObjectArray.Length; i++)
        {
            //teleportObj[i],�ƁA���ꂪ���R���|�[�l���g������B
            teleportChildDictionary.Add(teleportGameObjectArray[i], teleportGameObjectArray[i].GetComponent<TeleporterChild>());
        }
        if (teleportGameObjectArray.Length % 2 == 0)//�e���|�[�g�̐��������������ꍇ
        {
            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportIDDicitonary.Add(teleportGameObjectArray[i], i / 2); // 0 0 1 1 2...�Ƃ���������
                teleportGameObjectArray[i].GetComponent<Renderer>().material = teleporterMat[i / 2];
                teleportChildDictionary[teleportGameObjectArray[i]].VFXColorSetting(childVFXColors[i / 2]);
            }
            areTeleporterOddNumBool = false;
        }
        else if ((GameMaster.doSetAllTeleportersSame == true) || (teleportGameObjectArray.Length % 2 != 0))//��������ꍇ�A�������͑S������bool�����Ă����ꍇ
        {
            var thisGameColors = Random.Range(0, teleporterMat.Length);
            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportIDDicitonary.Add(teleportGameObjectArray[i], 0);//�ԍ�0�����ׂĂɓ����(�S�����ʂ̃e���|�[�g)
                teleportChildDictionary[teleportGameObjectArray[i]].VFXColorSetting(childVFXColors[thisGameColors]);
                teleportGameObjectArray[i].GetComponent<Renderer>().material = teleporterMat[thisGameColors];
            }
            areTeleporterOddNumBool = true;
        }
        
    }
    private void Update()
    {
        for (int i = 0; i < teleportTimer.Length; i++)
        {
            if (isNowMovingVFXbool[i] == true)//������T��...�B
                continue; //�A��B
            if (teleportTimer[i] < 0.0f)//�����������炷�K�v������������
            {
                isNowMovingVFXbool[i] = true;//�����n�߂܂�����
                Debug.Log(teleportGameObjectArray[i * 2].name);
                Debug.Log(FindSameMyID(teleportGameObjectArray[i * 2]).name);
                teleportChildDictionary[teleportGameObjectArray[i * 2]].VFXBoolSetting(isNowMovingVFXbool[i]);
                teleportChildDictionary[FindSameMyID(teleportGameObjectArray[i * 2])].VFXBoolSetting(isNowMovingVFXbool[i]);
                continue; //�A��B
            }
            else teleportTimer[i] -= Time.deltaTime;
        }
    }

    public void onCollisionEnterChildren(Transform orderTeleporter, Transform other)//�q�������������Ƃ��ɌĂ΂��
    {
        Transform otherTeleporter = FindSameMyID(orderTeleporter);
        MoveToOtherTeleporter(otherTeleporter, other);
        SettingVFXMoving(orderTeleporter,otherTeleporter);
    }
    void MoveToOtherTeleporter(Transform getOtherTransform, Transform otherPos)//���̃e���|�[�^�[�B
    {
        if (teleportTimer[teleportIDDicitonary[getOtherTransform]] <= 0.0f)
        {
            teleportTimer[teleportIDDicitonary[getOtherTransform]] = setTeleportTime;
            otherPos.position = getOtherTransform.position;
            GameMaster.audioManagerMaster.TeleporterAudio();
        }
    }
    void SettingVFXMoving(Transform orderTrans,Transform otherTrans)
    {
        if (areTeleporterOddNumBool == true)
        {
            isNowMovingVFXbool[0] = false;//0�Ԃ����Ȃ��̂�0�̃}�W�b�N�i���o�[�Ŏg�p

            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportChildDictionary[teleportGameObjectArray[i]].VFXBoolSetting(isNowMovingVFXbool[0]);//0�Ԃ����Ȃ��̂�...
            }
        }
        else
        {
            isNowMovingVFXbool[teleportIDDicitonary[orderTrans]] = false;
            teleportChildDictionary[orderTrans].VFXBoolSetting(isNowMovingVFXbool[teleportIDDicitonary[orderTrans]]);
            teleportChildDictionary[otherTrans].VFXBoolSetting(isNowMovingVFXbool[teleportIDDicitonary[otherTrans]]);
        }
    }
    Transform FindSameMyID(Transform teleporter)
    {
        Transform returnTransform;
        var sameIDTeleporter = new List<Transform>();
        foreach (Transform addTransformKey in teleportIDDicitonary.Keys)
        {
            Debug.Log(teleportIDDicitonary[addTransformKey]);
            if ((teleportIDDicitonary[addTransformKey] == teleportIDDicitonary[teleporter]) && (teleporter != addTransformKey)) //�����҂Ɠ���ID�������҂ƈႤTransform�Ȃ�
            {
                sameIDTeleporter.Add(addTransformKey);
                if (areTeleporterOddNumBool == false)//���������Ȃ���ł���H
                {
                    break;�@//�����ȊO�͂���Ȃ�
                }
            }
        }
        if (areTeleporterOddNumBool == false)
        {
            returnTransform = sameIDTeleporter[0];//�����ԍ��Ŏ����͏����̂�0�Ԗڂ̂����������Ȃ��B
        }
        else
        {
            returnTransform = sameIDTeleporter[Random.Range(0, sameIDTeleporter.Count)];//�����e���|�[�^�[�������_���ɑI��
        }

        return returnTransform;
    }
}
