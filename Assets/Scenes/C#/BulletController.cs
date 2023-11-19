using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BulletController : MonoBehaviour
{
    //�ʂ����˕Ԃ���
    public int canBounce;
    //�ʂ��i�ޑ��x
    public float bulletSpeed = GameMaster.setBulletSpeedMaster;
    //�ʂ̑傫���B��ʓI��0.1�`1�A�A�C�e�����L���Ƀf�J�������葊��̋ʂ������̂��̂ɂ��鎞�Ƃ��ɂ�����擾����\��������
    public float dicidedSize = 0.5f;
    //������Rb
    [SerializeField] Rigidbody2D bulletRigidbody;
    //���̋ʂ����L���Ă���v���C���[��\���ϐ� �ˁ@�����l�ΐ�B
    GameObject player;
    //�ʂ̏Փˎ��o�Ă���G�t�F�N�g�Ɛe
    [SerializeField] GameObject bulletEffect;
    [SerializeField] Color thisBulletEffectColor;

    void Start()
    {
        Debug.Log(gameObject);
        canBounce = GameMaster.setCanBounceMaster;
        GameMaster.AddGameObjectID(gameObject, 1);�@//�e�e��ID��n���B
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int otherID;

        if (other.gameObject.tag != "Wall") //�������������ID���߁B
        {
            Debug.Log(other.gameObject.name);
            otherID = GameMaster.GetGameObjectID(other.gameObject);//�ǂ���Ȃ��Ȃ�Dictionary�ɂ���͂��I�I

            //Debug.Log(other.gameObject.tag);
            //Debug.Log(other.gameObject.name);
        }
        else otherID = -1; //�ǂ�-1

        if (otherID == 0 && other.gameObject.tag != gameObject.tag) //��������̃v���C���[(ID : 0)���A�����̃`�[���ȊO�̃^�O��������
        {
            GameMaster.scoreManagerMaster.AddScorePlayer(gameObject.tag);//�^�O��n���ē_���̏���
            GameMaster.audioManagerMaster.HitPlayerAudio();
            StartCoroutine(MakeEffectAndDelete());
            gameObject.SetActive(false);
        }
        else if (otherID == 1 && other.gameObject.tag != gameObject.tag) //��������̒e��(ID : 1)���A�����̃`�[���ȊO�̃^�O��������
        {
            //�����̔ԍ����傫�����ǂ�����bool�^�ŏ������򂵎n�߂܂��B

            //���������̕����傫��������
            if (GameMaster.IsBulletNewerBool(gameObject,other.gameObject) == true)
            {
                other.gameObject.SetActive(false); //����̃Q�[���I�u�W�F�N�g���폜
                GameMaster.SettingWhenDeleteGameObject(other.gameObject); //���̃I�u�W�F�N�g��List,Dictionary�I�ɂ��폜

                //Clone
                GameObject cloneGameObject = Instantiate(gameObject, other.transform.position, gameObject.transform.rotation); //�������N���[������B
                cloneGameObject.name = gameObject.name;
                cloneGameObject.GetComponent<BulletController>().FirstAddForceFromOther(gameObject); //AddForce���s���B
                cloneGameObject.transform.parent = gameObject.transform.parent; //���������L���Ă邩��̃I�u�W�F�N�g�Ɉړ�������B

                //Effect
                StartCoroutine(MakeEffectAndDelete());

                GameMaster.audioManagerMaster.ChangeOtherBulletAudio();
            }
            else return; //����ɎE�����̂�҂��Ă뒆�Õi���B(����̃{�[��������if�������������B)
        }
        else if(canBounce > 0)//�ǂƂ��F�X�B�A�C�e���Ƃ����܂߂�,���˕Ԃ邱�Ƃ��o����Ȃ璵�˕Ԃ�B
        {
            GameMaster.audioManagerMaster.BounceAudio();
        }
        else //���˂�Ȃ����A��������;-;
        {
            gameObject.SetActive(false); //���̐��������;
            GameMaster.SettingWhenDeleteGameObject(gameObject);//���̃I�u�W�F�N�g��List,Dictionary�I�ɂ��폜
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
    public void FirstAddForceFromOther(GameObject orderObject)�@//�C���X�^���X�����ꂽ���Ɏg���A�����ő��肩��o�������ւ�Vector3�l����������
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
