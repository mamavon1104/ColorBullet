using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeyMove : MonoBehaviour
{
    //��{�I�ȃX�s�[�h
    public float thisPlayerSpeed = 12.5f;
    //�v���C���[�̃��W�b�h�{�f�B�[
    private Rigidbody2D rb;
    //�_�b�V���X�s�[�h�A�_�b�V���L�[���������Ƃ��̐i�݋�B
    [SerializeField] float dashSpeed;
    //�v�ɂȂ邩������Ȃ����ǁA��]�X�s�[�h...
    [SerializeField] float rotateSpeed;
    //�_�b�V���̎��̃^�C�}�[
    private float dashTimer = 0.0f;
    //�^�C�}�[���Z�b�g���鎞�ԁB
    public float setDashTime;
    //�S�Ă�������AGameDirector�l�̎擾�Ɏg�p�B
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameDirector").GetComponent<AudioManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        {
            Destroy(gameObject.GetComponent<PlayerController>()); //Key��true�Ȃ炠����������
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
            //�|���������Ɍ���
            var rotateAngle = -Vector2.SignedAngle(rotateDirection, Vector2.up);

            //�|���������ɉ�]����B
            transform.eulerAngles = Vector3.forward * rotateAngle;

            ////�����Ɖ������ŉ�]��������
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
