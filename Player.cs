using UnityEngine;
using System.Collections;
using System;

public class Player : Character
{
    private Player instance;
    public Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float doubleJumpForce;
    [SerializeField]
    private float immortalTime;
    public Rigidbody2D MyRigidbody { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    private int JumpNr;
    private Vector2 startPos;
    private bool immortal = false;
    private SpriteRenderer spriteRenderer;
    private GameMaster2 gm;
    private bool attack;
    private bool jumpAttack;
	private int value;
	private int who;
    //--------------------------------------------------------------------------------------------------
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster2>();
    }
    //--------------------------------------------------------------------------------------------------
    void Update()
    {
		if (gm.health < 0)
			gm.health = 0;
		if (transform.position.y <= -14f)
        {
            MyRigidbody.velocity = Vector2.zero;
            transform.position = startPos;
        }
		OnGround = IsGrounded();
		HandleInput();
    }
    //--------------------------------------------------------------------------------------------------
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");    
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleLayers();
        HandleAttacks();
        ResetValues();
    }
    //--------------------------------------------------------------------------------------------------
    public override bool IsDead
    {
        get
        { 
        	return gm.health <= 0; 
        }
    }
    //--------------------------------------------------------------------------------------------------
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Spygliai")
        {
            if (!IsDead && !immortal)
            {
				who = 0;
				gm.health -= 10;
				StartCoroutine(TakeDamage());
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            if (!IsDead)
            {
                myAnimator.SetTrigger("damage");
				if(who == 0)
					MyRigidbody.AddForce(new Vector2(1, 600));
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                myAnimator.SetTrigger("die");
                MyRigidbody.velocity = Vector2.zero;
                yield return new WaitForSeconds(1);
                Application.LoadLevel("gameover");
            }
        }
	}
	//--------------------------------------------------------------------------------------------------
    private void HandleMovement(float horizontal)
    {
        if (!IsDead)
        {
            if (MyRigidbody.velocity.y < 0)
            {
                myAnimator.SetBool("land", true);
            }

            if (Jump && JumpNr == 1 && !OnGround)
            {
                Vector3 v = MyRigidbody.velocity;
                v.y = 0;
                MyRigidbody.velocity = v;
                MyRigidbody.AddForce(new Vector2(0, doubleJumpForce));
                myAnimator.ResetTrigger("land");
                myAnimator.SetTrigger("jump");
                JumpNr++;
            }
            if (Jump && OnGround)
            {
                OnGround = false;
                MyRigidbody.AddForce(new Vector2(0, jumpForce));
                myAnimator.SetTrigger("jump");
                JumpNr++;
            }
			if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Suvis") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("JumpAttack"))
            {
                MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
            }
			if (!Attack && !Slide && (OnGround || airControl) && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Suvis"))
            {
                MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
            }
            myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }
    //--------------------------------------------------------------------------------------------------		
	private void HandleAttacks()
	{
		if (attack && OnGround && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("JumpAttack"))
		{
			myAnimator.SetTrigger("attack");
			MyRigidbody.velocity = Vector2.zero;
		}
		if (jumpAttack && !OnGround && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("M_jumpataka"))
		{
			myAnimator.SetBool("jumpAttack", true);
		}
		if (!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("M_jumpataka"))
		{
			myAnimator.SetBool("jumpAttack", false);
		}
	}
	//--------------------------------------------------------------------------------------------------
	private void HandleInput()
	{
		if (!IsDead)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump = true;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				myAnimator.SetBool("slide", true);
			}
			else
				myAnimator.SetBool("slide", false);
			if (Input.GetKeyDown(KeyCode.V))
			{
					myAnimator.SetTrigger ("suvis");
					if (OnGround)
						MyRigidbody.velocity = Vector2.zero;
			}
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				attack = true;
				jumpAttack = true;
			}
		}
	}
	//--------------------------------------------------------------------------------------------------
    private void Flip(float horizontal)
    {
        if (!IsDead && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
            {
                ChangeDirection();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    private void ResetValues()
    {
        attack = false;
        Slide = false;
        Jump = false;
        jumpAttack = false;
    }
    //--------------------------------------------------------------------------------------------------
    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y == 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        JumpNr = 0;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    //--------------------------------------------------------------------------------------------------
    private void HandleLayers()
    {
        if (!OnGround)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }

    }
    //--------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coins"))
        {
            Destroy(col.gameObject);
            gm.points += 1;
            if (gm.points % 20 == 0 && gm.points != 0)
                gm.health += 5;
        }
        if (col.CompareTag("EnemyKnife"))
        {
            if (!IsDead && !immortal)
            {
                who = 1;
                gm.health -= 10;
                Destroy(col.gameObject);
                StartCoroutine(TakeDamage());
            }
        }
        if (col.CompareTag("BossEnemyKnife"))
        {
            if (!IsDead && !immortal)
            {
                who = 1;
                gm.health -= 15;
                Destroy(col.gameObject);
                StartCoroutine(TakeDamage());
            }
        }
        if (col.CompareTag("EnemyMelee"))
        {
            if (!IsDead && !immortal)
            {
                who = 1;
                gm.health -= 10;
                StartCoroutine(TakeDamage());
            }
        }
        if (col.CompareTag("BossEnemyMelee"))
        {
            if (!IsDead && !immortal)
            {
                who = 1;
                gm.health -= 15;
                StartCoroutine(TakeDamage());
            }
        }
        if (col.CompareTag("Ending0"))
        {
            Application.LoadLevel("2_Ziema_IV");
        }
        if (col.CompareTag("Ending1"))
        {
            Application.LoadLevel("3_Miskas");
        }
        if (col.CompareTag("Ending2"))
        {
            Application.LoadLevel("Winner");
        }
    }
    //--------------------------------------------------------------------------------------------------
    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.15f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.15f);
        }
    }
	//--------------------------------------------------------------------------------------------------
    internal static float GetFlost(string p)
    {
        throw new NotImplementedException();
    }


}
//--------------------------------------------------------------------------------------------------