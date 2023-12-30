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

    [SerializeField]
    bool[] nowTruebool = new bool[4];
    public int playerNum = 0;
    int beforeNum = 0;
    InputAction Submit, Cancel;
    UIAudioManager audioManager;
    [SerializeField] GameObject obj1;
    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            nowTruebool[i] = false;
        }
        var director = GameObject.FindGameObjectWithTag("GameDirector");

        audioManager = director.GetComponent<UIAudioManager>();
        Submit = director.GetComponent<PlayerInput>().actions["Submit"]; // Å© "Move" ActionÇóòópÇ∑ÇÈÅB
        Cancel = director.GetComponent<PlayerInput>().actions["Cancel"]; // Å© "ButtonA" ActionÇóòóp
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
        if (beforeNum != playerNum)
        {
            beforeNum = playerNum;
            GameMaster.setPlayersNumMaster = playerNum;
        }
    }
    private void OnPushSubmitMotion(InputAction.CallbackContext context)
    {
        Debug.Log("a");
        if (!CanChange)
            return;

        Debug.Log("b");
        for (int i = 0; i < GameMaster.inputDeviceMaster.Count; i++)
        {
            Debug.Log("c");
            if (context.control.device == GameMaster.inputDeviceMaster[i] && !nowTruebool[i])
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
        Debug.Log("e " + GameMaster.inputDeviceMaster.Count);
    }
    private void OnPushCancelMotion(InputAction.CallbackContext context)
    {
        if (!CanChange)
            return;

        for (int i = 0; i < GameMaster.inputDeviceMaster.Count; i++)
        {
            if (i > 3)
                return;

            if (context.control.device == GameMaster.inputDeviceMaster[i] && nowTruebool[i])
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
            else if (context.control.device == GameMaster.inputDeviceMaster[i] && !nowTruebool[i])
            {
                for (int j = 0; j < GameMaster.inputDeviceMaster.Count; j++)
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
