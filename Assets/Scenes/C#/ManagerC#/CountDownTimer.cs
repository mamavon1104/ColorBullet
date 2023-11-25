using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    TextManager textManager;
    private void Awake()
    {
        GameMaster.canNotPlayersMove = true; 
        textManager= GetComponent<TextManager>();
    }
    private void Start()
    {
        StartCoroutine(FirstCountDown());
    }
    IEnumerator FirstCountDown()
    {
        GameMaster.canNotPlayersMove = true;
        for (int i = 0; i < 4; i++)//[3][2][1][Go]‚ÌŽl‰ñ•ª
        {
            textManager.WriteCountDown(i);
            GameMaster.audioManagerMaster.CountDownAudio(i);
            yield return new WaitForSeconds(1);
            textManager.WriteCountDown(i);
        }
        GameMaster.canNotPlayersMove = false;
    }
}
