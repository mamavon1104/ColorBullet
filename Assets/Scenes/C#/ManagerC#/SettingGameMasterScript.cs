using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingGameMasterScript : MonoBehaviour
{
    private void Awake()
    {
        GameMaster.gameDirectorMaster = gameObject;
        GameMaster.scoreManagerMaster = gameObject.GetComponent<ScoreManager>();
        GameMaster.audioManagerMaster = gameObject.GetComponent<AudioManager>();
        GameMaster.SceneManagerMaster = gameObject.GetComponent<ChangeSceneManager>();
}
}
