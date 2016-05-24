using UnityEngine;
using System.Collections;

public class MeleeState : IEnemyState
{
//-------------------------------------------------------------------------------------------------
    private Enemy enemy;
    private float  attackTimer;
    private float  attackCoolDown = 3;
    private bool canAttack = true;
//-------------------------------------------------------------------------------------------------

    void IEnemyState.Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
   void IEnemyState.Execute()
    {
        Attack();
        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
		if (enemy.InMeleeRange)
			enemy.myAnimator.SetFloat("speed", 0);
    }
    void IEnemyState.Exit()
    {
      
    }

    void IEnemyState.OnTriggerEnter(Collider2D other)
    {
       
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }
        if (canAttack)
        {
			canAttack = false;
            enemy.myAnimator.SetTrigger("attack");
        }
    }
}
