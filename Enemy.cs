using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }
    private float meleeRange = 4;
    private float throwRange = 12;
//-------------------------------------------------------------------------------------------------  
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }
//-------------------------------------------------------------------------------------------------  
    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }



//-------------------------------------------------------------------------------------------------  
	public override void Start () 
    {
        base.Start();
        ChangeState(new IdleState());
		if (boss) {
			meleeRange = 7;
			health = 50;
			meleeRange = 15;
		}
	}
//-------------------------------------------------------------------------------------------------
	void Update () 
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
	}
//-------------------------------------------------------------------------------------------------
  private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }
//-------------------------------------------------------------------------------------------------
    public void ChangeState( IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }
//-------------------------------------------------------------------------------------------------
    public void Move()
    {
		if (!Attack && !myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("R_Attack") && !myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("R_Suvis"))
        {
            myAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
    }
//-------------------------------------------------------------------------------------------------
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;

    }
//-------------------------------------------------------------------------------------------------
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }
//-------------------------------------------------------------------------------------------------

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        if(!IsDead)
        {
            myAnimator.SetTrigger("damage");
        }
        else
        {
			DieCollider ();
			myAnimator.SetTrigger("die");
            yield return null; 
        }
    }

}
