using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //TODO:
    private enum Direction
    {
        Up,Right,Left
    }

    private Rigidbody2D rb;
    private Vector2 destination;
    private Vector2 touchPositon;
    private SpriteRenderer sr;
    private PlayerInput playerinput;
    private BoxCollider2D colloder;

    private Direction dir;

    private Animator anim;

    [Header("�÷�")]
    public int stepPoint;
    private int pointResult;

    public float jumpDistance;
    private float moveDistance;
    private bool buttonHold;
    private bool isJump;
    private bool canJump;
    private bool isDead;

    //�ж���ײ��ⷵ�ص�����
    private RaycastHit2D[] result = new RaycastHit2D[2];


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerinput = GetComponent<PlayerInput>();
        colloder = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            rb.position = Vector2.Lerp(transform.position, destination, 0.134f);
        }
    }

    private void Update()
    {
        

        if (canJump)
        {
            TriggerJump();
        }
        if (isDead)//�����Ͳ��ܿ�����
        {
            DisableInput();
            return;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !isJump)
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.1f,Vector2.zero,result);

            bool inWater = true;

            foreach (var hit in result)
            {
                if (hit.collider == null) continue; //û��⵽�����ײ
                if (hit.collider.CompareTag("Wood"))//��ľ����
                {
                    // TODO:����ľ��
                    inWater = false;
                    transform.parent = hit.collider.transform;
                }
            }

            if (inWater && !isJump)
            {
                //��Ϸ����
                Debug.Log("Gameover");

                isDead = true;
            }

        }


        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("Gameover");
            isDead = true;

        }

        if (!isJump && other.CompareTag("Obstacle"))
        {
            Debug.Log("Gameover");
            isDead = true;
        }
        if (isDead)
        {
            EventHandler.CallGameOverEvent();
            colloder.enabled = false;
        }
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Wood"))//����ľ��
    //    {
    //        transform.parent = null;
    //    }
    //}


    #region Input ����ص�����


    public void Jump(InputAction.CallbackContext context)
    {   //TODO: ִ����Ծ ����Ծ�ľ��룬��¼������������Ծ����Ч


        if (context.performed && !isJump)//����֮����ִ����������
        {
            moveDistance = jumpDistance;

            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
            //Debug.Log("Jump"+" "+moveDistance);

            AudioManager.instance.SetJumpClip(0);
        }

        if (dir == Direction.Up && context.performed && !isJump)
        {
            pointResult += stepPoint;
        }
    }


    public void LongJump(InputAction.CallbackContext context)
    {

        if (context.performed && !isJump)
        {
            buttonHold = true;
            moveDistance = jumpDistance * 2;

            AudioManager.instance.SetJumpClip(1);

        }

        if (context.canceled && buttonHold && !isJump)
        {
            //Debug.Log("Long Jump"+" "+moveDistance);
            if (dir == Direction.Up)
            {
                pointResult += stepPoint * 2;
            }

            buttonHold = false;
            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;

        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            touchPositon = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());//��������ֵתΪ��������

            var offset = ((Vector3)touchPositon - transform.position).normalized;//normalized��ʾֻ�ܷ���0~1

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;

            }
        }
    }

    #endregion

    /// <summary>
    /// ����ִ����Ծ����
    /// </summary>
    private void TriggerJump()
    {
        //����ƶ����� ���Ŷ���
        canJump = false;
        // TODO�������л����Ҷ���
        switch (dir)
        {
            case Direction.Up:
                anim.SetBool("isSide",false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = Vector3.one;

                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1,1,1);

                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;

                break;
        }

        anim.SetTrigger("Jump");

    }


    #region �����¼�


    public void JumpAnimationEvent()
    {

        //������Ծ��Ч
        AudioManager.instance.PlayJumpFX();

        isJump = true;
        transform.parent = null;//����ľ��

        sr.sortingLayerName = "Front";
    }

    public void FinishJumpAnimationEvent()
    {
        isJump = false;

        sr.sortingLayerName = "Middle";

        if (dir == Direction.Up && !isDead)
        {
            // TODO:�÷� ������ͼ���ؼ��



            EventHandler.CallGetPointEvent(pointResult);

            //Debug.Log(pointResult); //�÷�
        }
    }

    #endregion

    private void DisableInput()
    {
        playerinput.enabled = false;
    }
}
