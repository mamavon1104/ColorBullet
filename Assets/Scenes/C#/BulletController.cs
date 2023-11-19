using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BulletController : MonoBehaviour
{
    //玉が跳ね返れる回数
    public int canBounce;
    //玉が進む速度
    public float bulletSpeed = GameMaster.setBulletSpeedMaster;
    //玉の大きさ。一般的に0.1～1、アイテム所有時にデカくしたり相手の玉を自分のものにする時とかにこれを取得する可能性がある
    public float dicidedSize = 0.5f;
    //こいつのRb
    [SerializeField] Rigidbody2D bulletRigidbody;
    //その玉を所有しているプレイヤーを表す変数 ⇒　複数人対戦。
    GameObject player;
    //玉の衝突時出てくるエフェクトと親
    [SerializeField] GameObject bulletEffect;
    [SerializeField] Color thisBulletEffectColor;

    void Start()
    {
        Debug.Log(gameObject);
        canBounce = GameMaster.setCanBounceMaster;
        GameMaster.AddGameObjectID(gameObject, 1);　//銃弾とIDを渡す。
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int otherID;

        if (other.gameObject.tag != "Wall") //当たった相手のID決め。
        {
            Debug.Log(other.gameObject.name);
            otherID = GameMaster.GetGameObjectID(other.gameObject);//壁じゃないならDictionaryにあるはず！！

            //Debug.Log(other.gameObject.tag);
            //Debug.Log(other.gameObject.name);
        }
        else otherID = -1; //壁は-1

        if (otherID == 0 && other.gameObject.tag != gameObject.tag) //もし相手のプレイヤー(ID : 0)かつ、自分のチーム以外のタグだったら
        {
            GameMaster.scoreManagerMaster.AddScorePlayer(gameObject.tag);//タグを渡して点数の処理
            GameMaster.audioManagerMaster.HitPlayerAudio();
            StartCoroutine(MakeEffectAndDelete());
            gameObject.SetActive(false);
        }
        else if (otherID == 1 && other.gameObject.tag != gameObject.tag) //もし相手の弾丸(ID : 1)かつ、自分のチーム以外のタグだったら
        {
            //自分の番号が大きいかどうかのbool型で条件分岐し始めます。

            //もし自分の方が大きかったら
            if (GameMaster.IsBulletNewerBool(gameObject,other.gameObject) == true)
            {
                other.gameObject.SetActive(false); //相手のゲームオブジェクトを削除
                GameMaster.SettingWhenDeleteGameObject(other.gameObject); //他のオブジェクトをList,Dictionary的にも削除

                //Clone
                GameObject cloneGameObject = Instantiate(gameObject, other.transform.position, gameObject.transform.rotation); //自分をクローンする。
                cloneGameObject.name = gameObject.name;
                cloneGameObject.GetComponent<BulletController>().FirstAddForceFromOther(gameObject); //AddForceを行う。
                cloneGameObject.transform.parent = gameObject.transform.parent; //自分を所有してるからのオブジェクトに移動させる。

                //Effect
                StartCoroutine(MakeEffectAndDelete());

                GameMaster.audioManagerMaster.ChangeOtherBulletAudio();
            }
            else return; //相手に殺されるのを待ってろ中古品が。(相手のボールから上のif文が発動される。)
        }
        else if(canBounce > 0)//壁とか色々。アイテムとかも含めて,跳ね返ることが出来るなら跳ね返る。
        {
            GameMaster.audioManagerMaster.BounceAudio();
        }
        else //跳ねれないし、もう無理;-;
        {
            gameObject.SetActive(false); //この世から消滅;
            GameMaster.SettingWhenDeleteGameObject(gameObject);//このオブジェクトをList,Dictionary的にも削除
        }
        canBounce--;
    }
    private IEnumerator MakeEffectAndDelete()
    {
        var cloneEffectObj = Instantiate(bulletEffect, transform.position, Quaternion.identity);
        var thisVFXEffect = cloneEffectObj.GetComponent<VisualEffect>();
        thisVFXEffect.SetVector4("EffectColor", thisBulletEffectColor);
        yield return new WaitForSeconds(0.8f);
        cloneEffectObj.SetActive(false) ;
    }
    public void FirstAddForceFromOther(GameObject orderObject)　//インスタンス化された時に使う、引数で相手から出す方向へのVector3値をいただく
    {
        bulletRigidbody.AddForce(orderObject.transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    public void FindMyPlayerAndSetSize(GameObject shotPlayer)
    {
        player = shotPlayer;
        gameObject.transform.localScale = player.transform.localScale * dicidedSize;
    }
    public void CreateBulletEffect(Transform orderTrans)
    {
        var effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
        effect.transform.parent = orderTrans;
        var thisVFXEffect = effect.GetComponent<VisualEffect>();
        thisVFXEffect.SetVector4("EffectColor", thisBulletEffectColor);
    }
}
