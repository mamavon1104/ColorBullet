using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    [SerializeField] float gameTimer;
    TextManager textManager;

    bool isThisGameFinished = false;

    private void Start()
    {
        gameTimer = GameMaster.setGameTimerMaster;
        textManager = gameObject.GetComponent<TextManager>();
        textManager.WriteGameTimer(gameTimer);
    }
    void Update()
    {
        if (GameMaster.canNotPlayersMove == true)
            return;
        if (gameTimer > 0f)
        {
            gameTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        textManager.WriteGameTimer(gameTimer);
        if (gameTimer <= 0)
        {
            gameTimer = 0;
            if (isThisGameFinished == true)
                return;

            GameMaster.canNotPlayersMove = true;
            isThisGameFinished = true;
            StartCoroutine(textManager.WriteFinishText());
            GameMaster.SetWhenFinishedScores(gameObject.GetComponent<ScoreManager>().playersPoint);
            GameMaster.audioManagerMaster.FinishAudio();
        }
    }
}
