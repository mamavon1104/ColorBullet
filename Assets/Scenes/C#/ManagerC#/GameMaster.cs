using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameMaster
{
    //こいつを修正しないと
    public static Dictionary<int, InputDevice> gamepadMaster = new Dictionary<int, InputDevice>();


    public static int setCanShotBulletMaster;//試合で最大何連続まで打てるか
    public static float setBulletSpeedMaster; //銃弾の速度
    public static float setDashTimeMaster; //どのくらいの時間でダッシュが可能になるか
    public static float setTeleportTimeMaster; //テレポートする時間
    public static float setCannonShotCoolTimeMaster; //Cannonの発射する速度
    public static int setCanBounceMaster; // 玉の跳ねる回数。
    public static bool doSetAllTeleportersSame; //全てのテレポーターのIDを同じにするか。
    public static int setPlayersNumMaster;
    public static int setGameTimerMaster;

    public static bool canNotPlayersMove;

    public static GameObject gameDirectorMaster;
    public static AudioManager audioManagerMaster;
    public static ScoreManager scoreManagerMaster;
    public static ChangeSceneManager SceneManagerMaster;

    public static int[] thisGamePlayersScore;

    static Dictionary<string, int> teamIDDictionary = new Dictionary<string, int>(); // team ⇒ IDv1

    static Dictionary<GameObject, int> gameObjectIDDictionary = new Dictionary<GameObject, int>();// GameObject ⇒ IDv2
    /*
        IDを管理するDictionary変数、以下IDがあらわすObject
        {
            ID -01 : Wall(いらない子),
            ID  00 : Player,
            ID  01 : Bullet,
            ID  02 : Cannon,
            ID  03 : Teleport
        }

        この時はenumも知らなかったのかな
    　　でも気にせずコード書いていた時が懐かしいな
    */
    static List<GameObject> bulletOlderList = new List<GameObject>(); //どっちのほうが速く打たれたかを保存、IndexOf等で速さを取得

    static public void AddGamePad(Dictionary<int,InputDevice> dic)
    {
        gamepadMaster = dic;
    }
    static public void AddTeamID(string team)
    {
        if (teamIDDictionary.ContainsKey(team))
            return;
        teamIDDictionary.Add(team,teamIDDictionary.Count); //PlayerのTagとIDでの管理の準備。
    }
    static public int GetTeamID(string team)
    {
        return teamIDDictionary[team];
    }
    public static void AddGameObjectID(GameObject getGameObject, int gameObjectID) //新入りを追加する関数
    {
        //初期化するときにこいつにこれを渡す。
        gameObjectIDDictionary.Add(getGameObject, gameObjectID);
        //もしIDの番号が銃弾だったら
        if (gameObjectID == 1)
            bulletOlderList.Add(getGameObject); // 銃弾のListも追加します。
    }
    public static int GetGameObjectID(GameObject getGameObject) //番号を取得する関数
    {
        if(gameObjectIDDictionary.ContainsKey(getGameObject) == false)
        {
            return -1; //ないなら仕方ない。壁判定を返す。
        }
        else
        {
            return gameObjectIDDictionary[getGameObject];
        }
    }
    public static bool IsBulletNewerBool(GameObject toOrderBulletobject, GameObject otherBulletObject) //どっちのほうが新しいか見る関数
    {
        int toOrderBulletNum = bulletOlderList.IndexOf(toOrderBulletobject); //注文(これを呼び出してくる)オブジェクトの番号取得
        int otherBulletNum = bulletOlderList.IndexOf(otherBulletObject);　　 //ぶつかったオブジェクトの番号取得

        bool amInewerThanOther;  //返す奴

        if (toOrderBulletNum > otherBulletNum) //もし自分の番号の方が大きかったら(新しい方が大きい)
        {
            amInewerThanOther = true; //新しいよ
        }
        else //もし相手より大きくなかったら
        {
            amInewerThanOther = false;//中古品が
        }
        return amInewerThanOther;　//返します
    }

    public static IEnumerator SettingWhenDeleteGameObject(GameObject deletedGameObject) //お亡くなりになられた際の設定をいたします。とても長生きでしたね。
    {
        yield return new WaitForSeconds(2.0f);
        if (gameObjectIDDictionary[deletedGameObject] == 1)//もし彼らが銃弾として生きたのならば
            bulletOlderList.Remove(deletedGameObject); //銃弾Listからも存在を剥奪。
        gameObjectIDDictionary.Remove(deletedGameObject); //ゲームオブジェクトとしてのIDを剥奪
    }
    public static void DeleteStaticMember()
    {
        //書記か
        bulletOlderList.Clear();
        gameObjectIDDictionary.Clear();
    }
}
