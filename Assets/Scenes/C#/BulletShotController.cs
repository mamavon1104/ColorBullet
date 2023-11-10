using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BulletShotController : MonoBehaviour
{
    //セットする時間,(玉を発射 /玉を回復)を元に戻す値。
    [SerializeField] float setShotCoolTime;
    [SerializeField] float setRestrationCoolTime;

    //銃弾のプレファブ
    [SerializeField] GameObject _mbullet;

    //銃弾が発射されるノズルと、空のオブジェクト(自分の弾を保管しておく場所);
    [SerializeField] Transform nozzle;
    [SerializeField] Transform bulletEmptyParent;

    //打ったときの反動
    [SerializeField] float recoil;

    //玉を出せる回数
    public int canShot;

    //ゲーム開始時に所有している段数を保存する変数
    private int bulletNum;

    //自分のリジッドボディー
    Rigidbody2D playerRB;

    //タイマー、これが0以下になったとき(玉を発射することが可能 /玉を回復する事が可能に)
    private float shotTimer = 0.5f;
    private float restorationTimer = 1.8f;

    //打ってないと回復する時間が速くなる。という設定のため
    private int doNotShot = 0;

    //全てを牛耳る、GameDirector様のとその他取得に使用。
    TextManager textManager;

    //InputAction;
    InputAction shotAction;

    //mygamePad
    Gamepad mypad;

    private void Awake()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        shotAction = gameObject.GetComponent<PlayerInput>().actions["Shot"];

        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        textManager = GameObject.FindGameObjectWithTag("GameDirector").GetComponent<TextManager>();
        bulletNum = GameMaster.setCanShotBulletMaster;
        canShot = bulletNum;
    }
    private void Start()
    {
        mypad = GameMaster.gamepadMaster[GameMaster.GetTeamID(transform.tag)];
    }
    private void OnEnable()
    {
        shotAction.performed += OnPushShot;
    }
    private void OnDisable()
    {
        shotAction.performed -= OnPushShot;
    }
    void Update()
    {
        if (shotTimer > 0)
            shotTimer -= Time.deltaTime;

        if (GameMaster.canNotPlayersMove == true && SceneManager.GetActiveScene().buildIndex != 0)
            return;

        if (SceneManager.GetActiveScene().buildIndex != 0)
            RestorationShotTimes(false);
    }

    private void RestorationShotTimes(bool wasShot)　//回復するための関数。Shot()をした場合にtrueを渡すことで連続して回復したときの速度よりも遅くする。
    {
        if (!wasShot && canShot < bulletNum) // Update内(打ってない)時に呼び出された場合 & 最大弾数以上じゃないよね？
        {
            if (restorationTimer > 0)
                restorationTimer -= Time.deltaTime; //タイマーをどんどん減らしていきます。
            else if (restorationTimer <= 0)  //打たずにタイマーを減らせたら！
            {
                if(textManager != null)
                   textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, true);
                canShot++; //回数を増やします。
                restorationTimer = setRestrationCoolTime;
                doNotShot++;
                GameMaster.audioManagerMaster.RestorationAudio();
                for (int i = 1; i < bulletNum; i++)
                {
                    if (doNotShot == i)
                    {
                        restorationTimer = setRestrationCoolTime / i + (0.2f * i);
                    }
                }
            }
        }
        else //打ってしまった場合 / 最大弾数の場合。
        {
            doNotShot = 0;
            restorationTimer = setRestrationCoolTime;
        }
    }
    public void OnPushShot(InputAction.CallbackContext context)
    {
        if (GameMaster.canNotPlayersMove)
            return;

        //修正を入れに来た。
        //多分GameScene以外ならと言うif文の分岐か？　ちょっとひどいコードすぎるけど絶対直す。
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (shotTimer <= 0 && canShot > 0)
            {
                RestorationShotTimes(true); //打ちましたのでtrueを送る
                GameObject bullet = Instantiate(_mbullet, nozzle.transform.position, transform.rotation); //Objectを生成する。
                bullet.name = _mbullet.name;
                bullet.transform.parent = bulletEmptyParent;
                BulletController bulletCtrl = bullet.GetComponent<BulletController>();
                bulletCtrl.FirstAddForceFromOther(nozzle.gameObject); //AddForceを行う。
                GameMaster.audioManagerMaster.ShotAudio();

                playerRB.AddForce(-transform.right * recoil, ForceMode2D.Impulse); //打ったときに後ろ側に引く

                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                canShot--; //弾数減らす
                if (textManager == null)
                    return;

                textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, false);
                shotTimer = setShotCoolTime; //クールタイムにする
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (shotTimer <= 0 && canShot > 0)
            {
                RestorationShotTimes(true); //打ちましたのでtrueを送る
                GameObject bullet = Instantiate(_mbullet, nozzle.transform.position, transform.rotation); //Objectを生成する。
                bullet.name = _mbullet.name;
                bullet.transform.parent = bulletEmptyParent;
                BulletController bulletCtrl = bullet.GetComponent<BulletController>();
                bulletCtrl.FirstAddForceFromOther(nozzle.gameObject); //AddForceを行う。
                GameMaster.audioManagerMaster.ShotAudio();

                playerRB.AddForce(-transform.right * recoil, ForceMode2D.Impulse); //打ったときに後ろ側に引く

                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                canShot--; //弾数減らす
                if (textManager == null)
                    return;

                textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, false);
                shotTimer = setShotCoolTime; //クールタイムにする
            }
        }
    }
}
