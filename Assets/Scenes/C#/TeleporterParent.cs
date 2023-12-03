using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleporterParent : MonoBehaviour
{
    Transform[] teleportGameObjectArray; //テレポートがステージにどれくらいあるのか把握、番号の把握に使用。
    
    Dictionary<Transform, int> teleportIDDicitonary = new Dictionary<Transform, int>(); //IDを子オブジェクトのTransformで管理    
    Dictionary<Transform, TeleporterChild> teleportChildDictionary = new Dictionary<Transform, TeleporterChild>(); //TransformChildCSを子オブジェクトのTransformで管理
    bool[] isNowMovingVFXbool; //このペアのテレポーターが持つVFXは動いているかどうかを見る。

    float[] teleportTimer;//テレポートのタイマー配列
    float setTeleportTime;//セットする時間。

    bool areTeleporterOddNumBool = false; //テレポートって奇数？
    [SerializeField] Material[] teleporterMat;//テレポーター専用マテリアルの箱
    [SerializeField] Color[] childVFXColors;
    private void Awake()
    {
        setTeleportTime = GameMaster.setTeleportTimeMaster;
        #region 普通にプレイヤーが開始した直後の移動にも使えそう、初期化プログラム。後日任せた
        Debug.Log(transform.childCount);
        teleportGameObjectArray = new Transform[transform.childCount]; //実際の配列の初期化

        var childTemporaryArray = new Transform[teleportGameObjectArray.Length]; //仮の配列の初期化
        var numOfTimesPutChild = new Dictionary<int, bool>(); //番号が被っているかどうかの確認[randomNum]を渡すことでfalseがだったらまだ使われていない。

        var i = 0; //現在のteleportArrayの番号;
        for (int Num = 0; Num < childTemporaryArray.Length; Num++)
        {
            childTemporaryArray[Num] = transform.GetChild(Num); //仮の箱にテレポート全部入れる。
            numOfTimesPutChild.Add(Num, false); //ゲームObjectの番号(teleportArray)の数だけ追加。

            //Debug.Log(Num);
            //Debug.Log(Num.ToString() + numOfTimesPutChild[i]);
        }
        while (i < childTemporaryArray.Length)
        {
            int randomNum;//ランダム番目の数字
            do
            {
                randomNum = Random.Range(0, childTemporaryArray.Length); //番号を変更する。
                //Debug.Log(numOfTimesPutChild[randomNum] + " randomNum : " + randomNum);

            } while (numOfTimesPutChild[randomNum] == true); //もしtrue(もう使用済み)なら繰り返し。

            teleportGameObjectArray[i] = childTemporaryArray[randomNum];

            numOfTimesPutChild[randomNum] = true;
            i++;
        };

        if (!areTeleporterOddNumBool && !GameMaster.doSetAllTeleportersSame) teleportTimer = new float[(teleportGameObjectArray.Length / 2)]; //ペアごとのタイマーを作る
        else teleportTimer = new float[1]; //一つだけ作りたい

        isNowMovingVFXbool = new bool[teleportTimer.Length];//テレポートタイマーが0になったら動き、当たったら消すのでこいつの数の量だけ必要。
        #endregion
    }
    private void Start()
    {
        for (int i = 0; i < teleportGameObjectArray.Length; i++)
        {
            //teleportObj[i],と、それが持つコンポーネントを入れる。
            teleportChildDictionary.Add(teleportGameObjectArray[i], teleportGameObjectArray[i].GetComponent<TeleporterChild>());
        }
        if (teleportGameObjectArray.Length % 2 == 0)//テレポートの数が偶数だった場合
        {
            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportIDDicitonary.Add(teleportGameObjectArray[i], i / 2); // 0 0 1 1 2...という初期化
                teleportGameObjectArray[i].GetComponent<Renderer>().material = teleporterMat[i / 2];
                teleportChildDictionary[teleportGameObjectArray[i]].VFXColorSetting(childVFXColors[i / 2]);
            }
            areTeleporterOddNumBool = false;
        }
        else if ((GameMaster.doSetAllTeleportersSame == true) || (teleportGameObjectArray.Length % 2 != 0))//奇数だった場合、もしくは全部同じboolがついていた場合
        {
            var thisGameColors = Random.Range(0, teleporterMat.Length);
            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportIDDicitonary.Add(teleportGameObjectArray[i], 0);//番号0をすべてに入れる(全部共通のテレポート)
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
            if (isNowMovingVFXbool[i] == true)//もう二週目...。
                continue; //帰れ。
            if (teleportTimer[i] < 0.0f)//もしもう減らす必要が無かったら
            {
                isNowMovingVFXbool[i] = true;//動き始めましたよ
                Debug.Log(teleportGameObjectArray[i * 2].name);
                Debug.Log(FindSameMyID(teleportGameObjectArray[i * 2]).name);
                teleportChildDictionary[teleportGameObjectArray[i * 2]].VFXBoolSetting(isNowMovingVFXbool[i]);
                teleportChildDictionary[FindSameMyID(teleportGameObjectArray[i * 2])].VFXBoolSetting(isNowMovingVFXbool[i]);
                continue; //帰れ。
            }
            else teleportTimer[i] -= Time.deltaTime;
        }
    }

    public void onCollisionEnterChildren(Transform orderTeleporter, Transform other)//子供が当たったときに呼ばれる
    {
        Transform otherTeleporter = FindSameMyID(orderTeleporter);
        MoveToOtherTeleporter(otherTeleporter, other);
        SettingVFXMoving(orderTeleporter,otherTeleporter);
    }
    void MoveToOtherTeleporter(Transform getOtherTransform, Transform otherPos)//他のテレポーター。
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
            isNowMovingVFXbool[0] = false;//0番しかないので0のマジックナンバーで使用

            for (int i = 0; i < teleportGameObjectArray.Length; i++)
            {
                teleportChildDictionary[teleportGameObjectArray[i]].VFXBoolSetting(isNowMovingVFXbool[0]);//0番しかないので...
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
            if ((teleportIDDicitonary[addTransformKey] == teleportIDDicitonary[teleporter]) && (teleporter != addTransformKey)) //注文者と同じIDかつ注文者と違うTransformなら
            {
                sameIDTeleporter.Add(addTransformKey);
                if (areTeleporterOddNumBool == false)//偶数個しかないんでしょ？
                {
                    break;　//こいつ以外はいらない
                }
            }
        }
        if (areTeleporterOddNumBool == false)
        {
            returnTransform = sameIDTeleporter[0];//同じ番号で自分は除くので0番目のこいつしかいない。
        }
        else
        {
            returnTransform = sameIDTeleporter[Random.Range(0, sameIDTeleporter.Count)];//同じテレポーターをランダムに選ぶ
        }

        return returnTransform;
    }
}
