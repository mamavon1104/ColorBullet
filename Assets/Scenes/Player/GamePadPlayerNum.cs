using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadPlayerNum : MonoBehaviour
{
    private bool canChannge = true;
    public bool CanChange
    {
        get { return this.canChannge; }
        set { this.canChannge = value; }
    }

    bool[] nowTruebool = new bool[4];
    public int playerNum = 0;
    int beforeNum = 0;
    InputAction Submit, Cancel;
    UIAudioManager audioManager;
    ChangeSceneManager changeSceneManager;
    [SerializeField] GameObject obj1;
    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            nowTruebool[i] = false;
        }
        var director = GameObject.FindGameObjectWithTag("GameDirector");

        audioManager = director.GetComponent<UIAudioManager>();
        changeSceneManager  = director.GetComponent<ChangeSceneManager>();
        Submit = director.GetComponent<PlayerInput>().actions["Submit"]; // ← "Move" Actionを利用する。
        Cancel = director.GetComponent<PlayerInput>().actions["Cancel"]; // ← "ButtonA" Actionを利用
    }
    private void OnEnable()
    {
        Submit.performed += OnPushSubmitMotion;
        Cancel.performed += OnPushCancelMotion;
    }
    private void OnDisable()
    {
        Submit.performed -= OnPushSubmitMotion;
        Cancel.performed -= OnPushCancelMotion;
    }
    private void Update()
    {
        if(beforeNum != playerNum)
        {
            beforeNum = playerNum;
            GameMaster.setPlayersNumMaster = playerNum;

            //ここのセットかな　修正

            changeSceneManager.CanGotoGame=  true;
            //changeSceneManager.CanGotoGame = false;
        }
    }
    private void OnPushSubmitMotion(InputAction.CallbackContext context)
    {

        Debug.Log("a");
        if (!CanChange)
            return;

        Debug.Log("b");
        for (int i = 0; i < GameMaster.gamepadMaster.Count; i++)
        {
            Debug.Log("c");
            if (context.control.device == GameMaster.gamepadMaster[i] && !nowTruebool[i])
            {
                Debug.Log("d");
                nowTruebool[i] = true;
                playerNum++;
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    switch (j)
                    {
                        case 0:
                            transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                            break;
                        case 1:
                            transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                            break;
                    }
                }
                break;
            }
        }
        Debug.Log("e " + GameMaster.gamepadMaster.Count);
    }
    private void OnPushCancelMotion(InputAction.CallbackContext context)
    {
        if (!CanChange)
            return;
        
        for (int i = 0; i < GameMaster.gamepadMaster.Count; i++)
        {
            if (i > 3)
                return;

            if (context.control.device == GameMaster.gamepadMaster[i] && nowTruebool[i])
            {
                nowTruebool[i] = false;
                playerNum--;
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    switch (j)
                    {
                        case 0:
                            transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                            break;
                        case 1:
                            transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                            break;
                    }
                }
                break;
            }
            else if (context.control.device == GameMaster.gamepadMaster[i] && !nowTruebool[i])
            {
                for (int j = 0; j < GameMaster.gamepadMaster.Count; j++)
                {
                    if (nowTruebool[j] == false)
                        continue;

                    nowTruebool[j] = false;
                    playerNum--;
                    for (int l = 0; l < transform.GetChild(l).childCount; l++)
                    {
                        switch (l)
                        {
                            case 0:
                                transform.GetChild(j).GetChild(l).gameObject.SetActive(false);
                                break;
                            case 1:
                                transform.GetChild(j).GetChild(l).gameObject.SetActive(true);
                                break;
                        }
                    }
                }
                obj1.SetActive(true);
                audioManager.DecisionAudio();
                transform.parent.gameObject.SetActive(false);
                break;
            }
        }
    }
}
