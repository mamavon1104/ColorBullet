using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    //�m�Y��
    GameObject nozzle;
    //�ʂ̃v���t�@�u
    [SerializeField] GameObject bulletPrefab;
    //�ʂ̑łԊu�̃Z�b�g�ƃ^�C�}�[
    [SerializeField] float setShotTime;
    private float cannonShotTimer;
    //���������������ǂ���
    [SerializeField] bool isCollision = false;
    //�o�����ʂ̃R���g���[���[
    private GameObject bulletGameObject;
    private BulletController bulletController;
    //�ŏ��ɓ��͂��ꂽ�v���C���[�̒ePrefab�̃��X�g,FirstSetDirector������ƕ�����ʂ�S�܂�
    public GameObject[] shotBulletType = new GameObject[4];
    //�ʂ̐e���Ǘ�����privateTransform;
    private Transform bulletParent;
    private void OnCollisionEnter2D(Collision2D other)
    {
        //�ŏ��ɕǂ�Prefab�ɐG���̂�
        if (other.gameObject.tag == "Wall")
            return; //�Ԃ��܂��B

        Debug.Log(other.gameObject.tag);
        
        //�����������Ă���ʂ̃^�O(������������)�Ɠ��������ʂ������^�O�Ȃ�
        if (other.gameObject.tag == gameObject.tag)
            return; //�Ԃ��܂��B

        //���̃I�u�W�F�N�g��ID��1����Ȃ�(�ʈȊO)�Ȃ�
        if (GameMaster.GetGameObjectID(other.gameObject) != 1)
            return; //�Ԃ��܂��B


        //����̃}�e���A��
        var otherColor = other.gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = otherColor;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = otherColor;

        //�łv���t�@�u��ς���
        bulletPrefab = SetCannonPrefabWithTag(other.transform);
        bulletParent = other.transform.parent.transform;
        InstantiateBullet();
        //�����܂��������ĂȂ��̂Ȃ�
        if (isCollision == false)
        {
            isCollision = true; //������܂����t���O
        }
    }
    private void Start()
    {
        nozzle = transform.GetChild(0).gameObject;
        setShotTime = GameMaster.setCannonShotCoolTimeMaster;
        cannonShotTimer = setShotTime; //�ŏ��ɂ��߁[
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
