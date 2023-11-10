using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BulletShotController : MonoBehaviour
{
    //�Z�b�g���鎞��,(�ʂ𔭎� /�ʂ���)�����ɖ߂��l�B
    [SerializeField] float setShotCoolTime;
    [SerializeField] float setRestrationCoolTime;

    //�e�e�̃v���t�@�u
    [SerializeField] GameObject _mbullet;

    //�e�e�����˂����m�Y���ƁA��̃I�u�W�F�N�g(�����̒e��ۊǂ��Ă����ꏊ);
    [SerializeField] Transform nozzle;
    [SerializeField] Transform bulletEmptyParent;

    //�ł����Ƃ��̔���
    [SerializeField] float recoil;

    //�ʂ��o�����
    public int canShot;

    //�Q�[���J�n���ɏ��L���Ă���i����ۑ�����ϐ�
    private int bulletNum;

    //�����̃��W�b�h�{�f�B�[
    Rigidbody2D playerRB;

    //�^�C�}�[�A���ꂪ0�ȉ��ɂȂ����Ƃ�(�ʂ𔭎˂��邱�Ƃ��\ /�ʂ��񕜂��鎖���\��)
    private float shotTimer = 0.5f;
    private float restorationTimer = 1.8f;

    //�ł��ĂȂ��Ɖ񕜂��鎞�Ԃ������Ȃ�B�Ƃ����ݒ�̂���
    private int doNotShot = 0;

    //�S�Ă�������AGameDirector�l�̂Ƃ��̑��擾�Ɏg�p�B
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

    private void RestorationShotTimes(bool wasShot)�@//�񕜂��邽�߂̊֐��BShot()�������ꍇ��true��n�����ƂŘA�����ĉ񕜂����Ƃ��̑��x�����x������B
    {
        if (!wasShot && canShot < bulletNum) // Update��(�ł��ĂȂ�)���ɌĂяo���ꂽ�ꍇ & �ő�e���ȏザ��Ȃ���ˁH
        {
            if (restorationTimer > 0)
                restorationTimer -= Time.deltaTime; //�^�C�}�[���ǂ�ǂ񌸂炵�Ă����܂��B
            else if (restorationTimer <= 0)  //�ł����Ƀ^�C�}�[�����点����I
            {
                if(textManager != null)
                   textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, true);
                canShot++; //�񐔂𑝂₵�܂��B
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
        else //�ł��Ă��܂����ꍇ / �ő�e���̏ꍇ�B
        {
            doNotShot = 0;
            restorationTimer = setRestrationCoolTime;
        }
    }
    public void OnPushShot(InputAction.CallbackContext context)
    {
        if (GameMaster.canNotPlayersMove)
            return;

        //�C�������ɗ����B
        //����GameScene�ȊO�Ȃ�ƌ���if���̕��򂩁H�@������ƂЂǂ��R�[�h�����邯�ǐ�Β����B
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (shotTimer <= 0 && canShot > 0)
            {
                RestorationShotTimes(true); //�ł��܂����̂�true�𑗂�
                GameObject bullet = Instantiate(_mbullet, nozzle.transform.position, transform.rotation); //Object�𐶐�����B
                bullet.name = _mbullet.name;
                bullet.transform.parent = bulletEmptyParent;
                BulletController bulletCtrl = bullet.GetComponent<BulletController>();
                bulletCtrl.FirstAddForceFromOther(nozzle.gameObject); //AddForce���s���B
                GameMaster.audioManagerMaster.ShotAudio();

                playerRB.AddForce(-transform.right * recoil, ForceMode2D.Impulse); //�ł����Ƃ��Ɍ�둤�Ɉ���

                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                canShot--; //�e�����炷
                if (textManager == null)
                    return;

                textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, false);
                shotTimer = setShotCoolTime; //�N�[���^�C���ɂ���
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (shotTimer <= 0 && canShot > 0)
            {
                RestorationShotTimes(true); //�ł��܂����̂�true�𑗂�
                GameObject bullet = Instantiate(_mbullet, nozzle.transform.position, transform.rotation); //Object�𐶐�����B
                bullet.name = _mbullet.name;
                bullet.transform.parent = bulletEmptyParent;
                BulletController bulletCtrl = bullet.GetComponent<BulletController>();
                bulletCtrl.FirstAddForceFromOther(nozzle.gameObject); //AddForce���s���B
                GameMaster.audioManagerMaster.ShotAudio();

                playerRB.AddForce(-transform.right * recoil, ForceMode2D.Impulse); //�ł����Ƃ��Ɍ�둤�Ɉ���

                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                canShot--; //�e�����炷
                if (textManager == null)
                    return;

                textManager.WritePlayerBullet(GameMaster.GetTeamID(gameObject.tag), canShot, false);
                shotTimer = setShotCoolTime; //�N�[���^�C���ɂ���
            }
        }
    }
}
