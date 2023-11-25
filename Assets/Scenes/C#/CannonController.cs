using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    //ノズル
    GameObject nozzle;
    //玉のプレファブ
    [SerializeField] GameObject bulletPrefab;
    //玉の打つ間隔のセットとタイマー
    [SerializeField] float setShotTime;
    private float cannonShotTimer;
    //もう当たったかどうか
    [SerializeField] bool isCollision = false;
    //出した玉のコントローラー
    private GameObject bulletGameObject;
    private BulletController bulletController;
    //最初に入力されたプレイヤーの弾Prefabのリスト,FirstSetDirectorを見ると分かる通り４つまで
    public GameObject[] shotBulletType = new GameObject[4];
    //玉の親を管理するprivateTransform;
    private Transform bulletParent;
    private void OnCollisionEnter2D(Collision2D other)
    {
        //最初に壁かPrefabに触れるので
        if (other.gameObject.tag == "Wall")
            return; //返します。

        Debug.Log(other.gameObject.tag);
        
        //もし今持っている玉のタグ(を持った自分)と当たった玉が同じタグなら
        if (other.gameObject.tag == gameObject.tag)
            return; //返します。

        //他のオブジェクトのIDが1じゃない(玉以外)なら
        if (GameMaster.GetGameObjectID(other.gameObject) != 1)
            return; //返します。


        //相手のマテリアル
        var otherColor = other.gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = otherColor;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = otherColor;

        //打つプレファブを変える
        bulletPrefab = SetCannonPrefabWithTag(other.transform);
        bulletParent = other.transform.parent.transform;
        InstantiateBullet();
        //もしまだ当たってないのなら
        if (isCollision == false)
        {
            isCollision = true; //当たりましたフラグ
        }
    }
    private void Start()
    {
        nozzle = transform.GetChild(0).gameObject;
        setShotTime = GameMaster.setCannonShotCoolTimeMaster;
        cannonShotTimer = setShotTime; //最初にだめー
        isCollision = false;
    }
    private void Update()
    {
        if (isCollision == false)
            return;
        CannonShot();
    }
    private void CannonShot()
    {
        if (cannonShotTimer > 0)
            cannonShotTimer -= Time.deltaTime;
        else if(bulletGameObject.activeSelf == false)
        {
            InstantiateBullet();
        }
    }
    private void InstantiateBullet()
    {
        cannonShotTimer = setShotTime;
        bulletGameObject = Instantiate(bulletPrefab, nozzle.transform.position, Quaternion.identity);
        bulletGameObject.name = bulletPrefab.name;
        bulletGameObject.transform.parent = bulletPrefab.transform.parent;
        bulletGameObject.transform.parent = bulletParent;
        bulletController = bulletGameObject.GetComponent<BulletController>();
        bulletController.FirstAddForceFromOther(nozzle);
    }
    private GameObject SetCannonPrefabWithTag(Transform orderTransform)
    {
        GameObject returnBulletPrefab = null;
        switch (orderTransform.tag)
        {
            case "Team1":
                returnBulletPrefab = shotBulletType[0];
                break;
            case "Team2":
                returnBulletPrefab = shotBulletType[1];
                break;
            case "Team3":
                returnBulletPrefab = shotBulletType[2];
                break;
            case "Team4":
                returnBulletPrefab = shotBulletType[3];
                break;
        }
        if (returnBulletPrefab == null) Debug.Log("aaaa");
        gameObject.tag = orderTransform.gameObject.tag;
        return returnBulletPrefab;
    }
}
