using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerNum : MonoBehaviour
{
    int beforeNum = 0;
    int getNum = 0;
    [SerializeField] GameObject[] players;
    TextMeshProUGUI textMeshPro;
    ChangeSceneManager changeSceneManager;
    GamePadPlayerNum gamePadPlayerNum;
    private void Start()
    {
        var director = GameObject.FindGameObjectWithTag("GameDirector");
        gamePadPlayerNum = gameObject.GetComponent<GamePadPlayerNum>();
        changeSceneManager = director.GetComponent<ChangeSceneManager>();
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(gamePadPlayerNum != null)
        {
            if (!gamePadPlayerNum.CanChange)
                return;
        }

        getNum = GameMaster.inputDeviceMaster.Count;
        
        if (beforeNum != getNum)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i < getNum) players[i].SetActive(true);
                else players[i].SetActive(false);
            }

            if (textMeshPro == null)
                return;

            //C³

            textMeshPro.text = getNum.ToString();
            beforeNum = getNum;
        }
    }
}
