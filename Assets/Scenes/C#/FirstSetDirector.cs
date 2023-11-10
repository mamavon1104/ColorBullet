using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstSetDirector : MonoBehaviour
{
    public int setBulletNum = 6;
    public float setDashTime;
    public float setCannonTime;
    public int setBounceBulletNum = 3;
    public bool setTeleporterBool;
    public float setTeleporterTime;
    public float setBulletSpeed;
    public int setGameTimer = 150;
    void Start()
    {
        GameMasterSet();
    }
    public void GameMasterSet()
    {
        GameMaster.setCanShotBulletMaster = setBulletNum;
        GameMaster.setDashTimeMaster = setDashTime;
        GameMaster.setCannonShotCoolTimeMaster = setCannonTime;
        GameMaster.setCanBounceMaster = setBounceBulletNum;
        GameMaster.doSetAllTeleportersSame = setTeleporterBool;
        GameMaster.setTeleportTimeMaster = setTeleporterTime;
        GameMaster.setBulletSpeedMaster = setBulletSpeed;
        GameMaster.setGameTimerMaster= setGameTimer;
    }
}
