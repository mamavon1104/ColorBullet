using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //0番目、これだけは忘れるな。
    private GameObject crown;
    private bool crownYFlip;
    //PlayerとCrownのSpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer crownSpriteRenderer;

    //プレイヤーのリジッドボディー
    private Rigidbody2D rb;

    //基本的なスピード
    public float moveSpeed = 12.5f;

    //ダッシュスピード、ダッシュキーを押したときの進み具合。
    public float dashSpeed = 7.5f;

    //タイマーをセットする時間。
    public float setDashTime;

    //ダッシュの時のタイマー
    private float dashTimer = 0.0f;

    InputAction dash, move,look;
    Vector2 moveVec,lookVec;
    //自分の文字列InputManager
    InputPlayerSetString myStrings;

    //mygamePad
    InputDevice mypad;
    private void Awake()
    {
        myStrings = gameObject.GetComponent<InputPlayerSetString>();
        Debug.Log(myStrings);
        GameMaster.AddGameObjectID(gameObject, 0);
        var thisInput = gameObject.GetComponent<PlayerInput>().currentActionMap;
        dash = thisInput["Dash"];
        move = thisInput["Move"];
        look = thisInput["Look"];
    }
    private void Start()
    {
        InvokeRepeating("SettingPad", 0f, 1f); //1秒に一回実行
        crown = transform.GetChild(0).gameObject;
        crownSpriteRenderer = crown.GetComponent<SpriteRenderer>();
        rb = transform.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        dash.performed += OnDashPlayer;
        move.performed += OnMovePlayer;
        move.canceled += OnMovePlayer;
        look.performed += OnRotatePlayer;
        look.canceled += OnRotatePlayer;
    }
    private void SettingPad()
    {
        mypad = GameMaster.inputDeviceMaster[GameMaster.GetTeamID(tag)];
    }
    private void Update()
    {
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
        
        Flip();

        rb.AddForce(moveVec * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        if (lookVec.sqrMagnitude > 0)
        {
            //倒した方向に向く
            var rotateAngle = -Vector2.SignedAngle(lookVec, Vector2.right);

            //倒した方向に回転する。
            transform.eulerAngles = Vector3.forward * rotateAngle;
        }
    }
    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        if (GameMaster.canNotPlayersMove == true || context.control.device != mypad)
            return;

        moveVec = context.ReadValue<Vector2>();
    }
    public void OnRotatePlayer(InputAction.CallbackContext context)
    {
        if (GameMaster.canNotPlayersMove == true || context.control.device != mypad)
            return;

        lookVec = context.ReadValue<Vector2>();
    }
    public void OnDashPlayer(InputAction.CallbackContext context)
    {
        if (GameMaster.canNotPlayersMove == true)
            return;

        if (context.control.device == mypad)
        {
            dashTimer = setDashTime;
            GameMaster.audioManagerMaster.DashAudio();
            rb.AddForce(lookVec * dashSpeed, ForceMode2D.Impulse);
        }
    }
    public void Flip()
    {
        var angle = transform.eulerAngles.z;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        transform.eulerAngles = new Vector3(0f, 0f, angle);

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
        {
            playerSpriteRenderer.flipY = true;
            crownSpriteRenderer.flipY = true;
            if (crownYFlip == true)//一回だけ反転したい。
                return;
            crown.transform.localPosition = new Vector3(crown.transform.localPosition.x, -crown.transform.localPosition.y);
            crownYFlip = true;
        }
        else
        {
            playerSpriteRenderer.flipY = false;
            crownSpriteRenderer.flipY = false;
            if (crownYFlip == false)//一回だけ反転したい。
                return;
            crown.transform.localPosition = new Vector3(crown.transform.localPosition.x, -crown.transform.localPosition.y);
            crownYFlip = false;
        }
        //Debug.Log("hello Bug " + transform.eulerAngles.z);
    }

    public void SetActiveCrown(bool isThisNumber1)
    {
        if (isThisNumber1)
        {
            crown.SetActive(true); //王冠を表示
        }
        else
        {
            crown.SetActive(false);//王冠を非表示
        }
    }
}
