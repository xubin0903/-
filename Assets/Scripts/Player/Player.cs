using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player :PublicCharacter
{
    [Header("��������")]
    [SerializeField] private float beginspeed;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float xInput;
    
    
    [Header("���")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isDashable;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashCooldownTimer;
    [Header("����")]
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackDuration;
    [SerializeField] private bool isAttack;
    [SerializeField] private int comobatCount;
    [Header("����")]
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slideDuration;
    [SerializeField] private bool isSlide;
    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingCooldownTimer;
    
    
   
   

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {

        //�������
        CheckInput();
        //�ٶ�����
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        
        if (isMove == true && currentspeed < maxspeed)
        {
            currentspeed += Time.deltaTime * acceleration;
            acceleration += Time.deltaTime * 1.5f;


        }
        if (isMove == false)
        {
            currentspeed = beginspeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            acceleration = 1;
            animator.SetBool("isMove", false);
        }
        CollisionCheck();
        AnimationControl();
        //���
        if (dashTime>0)
        {
            dashTime -= Time.deltaTime;
            currentspeed = dashSpeed;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            if (dashTime<=0)
            {
                currentspeed = beginspeed;
                isDash = false;
            }
        }
        //�����ȴ
        if (!isDashable)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer<=0)
            {
                isDashable = true;
            }
        }
        //�������
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {

                comobatCount = 0;
            }
        }
        if (isDash)
        {
            DashMent();
        }
        //������ȴ
        if (slidingCooldownTimer > 0)
        {
            slidingCooldownTimer -= Time.deltaTime;
               
        }

    }
    private void CheckInput()
    {
        //�ƶ�
        isMove = Input.GetButtonDown("Horizontal") || Input.GetButton("Horizontal");
        if (isMove == true && !isDash && !isAttack)
        {
            Move();
        }
        //��Ծ
        if (Input.GetButtonDown("Jump")&&isGrounded)
        {
            Jump();

        }
        //���
        if (Input.GetKeyDown(KeyCode.LeftShift)&&isDashable)
        {
            Dash();
        }
        //����
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isGrounded)
        {
            Attack();
        }
        //����
        if (Input.GetKeyDown(KeyCode.C)&&isGrounded)
        {
            Slide();
        }
    }

    private void Slide()
    {
        if (isSlide)
        {
            return;
        }
        if (slidingCooldownTimer > 0)
        {
            return;
        }
        isSlide = true;
        currentspeed=slidingSpeed;
        //����Э��
        StartCoroutine(SlideStart(slideDuration));
    }

    private IEnumerator SlideStart(float slideDuration)
    {
        yield return new WaitForSeconds(slideDuration);
        SlideOver();
    }

    private void Move()
    {
        
        xInput = Input.GetAxis("Horizontal");
        if (xInput > 0)
        {
            faceDir = 1;

        }
        else if (xInput < 0)
        {
            faceDir = -1;
        }
        
        rb.velocity = new Vector2( xInput * currentspeed, rb.velocity.y);
        
        //�ƶ�����
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);//��ת
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AttackOver();
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x+groundCheckRadius,transform.position.y,transform.position.z), transform.position + Vector3.down * groundCheckDistance+Vector3.right*groundCheckRadius);
    }
    protected override void CollisionCheck()
    {
      isGrounded = Physics2D.Raycast(new Vector2(transform.position.x+groundCheckRadius,transform.position.y), Vector2.down, groundCheckDistance, groundLayer);
      
    }
   private void AnimationControl()
    {
        animator.SetBool("isMove", isMove);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isDash", isDash);
        animator.SetBool("isAttack", isAttack);
        animator.SetInteger("comobatCount", comobatCount);
        animator.SetBool("isSlide", isSlide);
    }   
    private void Dash()
    {
        dashTime=dashDuration;
        isDash = true;
        isDashable = false;
        dashCooldownTimer = dashCooldown;
        if (!isMove)
        {
            rb.velocity = new Vector2(faceDir * currentspeed, rb.velocity.y);
        }
    }
    private void Attack()
    {
        isAttack = true;
        attackTimer = attackDuration;
        rb.velocity = new Vector2(0, rb.velocity.y);
       
        

    }
    private void DashMent()
    {
        rb.velocity = new Vector2(faceDir * currentspeed, rb.velocity.y);
    }
    public void SlideOver()
    {
        isSlide = false;
        currentspeed = beginspeed;
        //���л�����ȴ
        slidingCooldownTimer = slidingCooldown; 
    }

    public void AttackOver()
    {
        isAttack = false;
        comobatCount++;
        if (comobatCount > 2)
        {
            comobatCount = 0;
        }
        
    }
}
