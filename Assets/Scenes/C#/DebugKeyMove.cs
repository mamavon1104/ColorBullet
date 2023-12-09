using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeyMove : MonoBehaviour
{
    //基本的なスピード
    public float thisPlayerSpeed = 12.5f;
    //プレイヤーのリジッドボディー
    private Rigidbody2D rb;
    //ダッシュスピード、ダッシュキーを押したときの進み具合。
    [SerializeField] float dashSpeed;
    //没になるかもしれないけど、回転スピード...
    [SerializeField] float rotateSpeed;
    //ダッシュの時のタイマー
    private float dashTimer = 0.0f;
    //タイマーをセットする時間。
    public float setDashTime;
    //全てを牛耳る、GameDirector様の取得に使用。
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameDirector").GetComponent<AudioManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        {
            Destroy(gameObject.GetComponent<PlayerController>()); //Keyがtrueならあっちを消す
        }
    }

    private void Update()
    {
        Move();
        Dash();
    }
    private void Move()
    {
        Vector2 moveDirection = CaluculationAndGetMoveDirection();
        Vector2 rotateDirection = CaluculationAndGetRotateDirection();
        rb.AddForce(moveDirection * thisPlayerSpeed * Time.deltaTime, ForceMode2D.Impulse);
        if (rotateDirection.sqrMagnitude > 0)
        {
            //倒した方向に向く
            var rotateAngle = -Vector2.SignedAngle(rotateDirection, Vector2.up);

            //倒した方向に回転する。
            transform.eulerAngles = Vector3.forward * rotateAngle;

            ////ずっと押す事で回転し続ける
            //transform.Rotate(Vector3.forward, rotateAngle * rotateSpeed, Space.World);
        }
    }
    private Vector2 CaluculationAndGetMoveDirection()
    {
        float moveVertical = Input.GetAxisRaw("Debug Key WS");
        float moveHorizontal = Input.GetAxisRaw("Debug Key AD");
        return new Vector2(moveHorizontal, moveVertical);
    }

    private Vector2 CaluculationAndGetRotateDirection()
    {
        float rotateVertical = Input.GetAxisRaw("Debug Key IK");
        float rotateHorizontal = Input.GetAxisRaw("Debug Key JL");
        return new Vector2(rotateHorizontal, rotateVertical);
    }
    public void Dash()
    {
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
        else if ((Input.GetKeyDown(KeyCode.H)))
        {
            dashTimer = setDashTime;
            audioManager.DashAudio();
            rb.AddForce(CaluculationAndGetMoveDirection() * dashSpeed, ForceMode2D.Impulse);
        }
    }
}
