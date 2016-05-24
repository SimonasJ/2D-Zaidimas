using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState 
{
    private Enemy enemy;
    private float patrolTimer = 0;
    private float patrolDuration = 10;

//-------------------------------------------------------------------------------------------------
	void IEnemyState.Enter(Enemy enemy)
	{
		this.enemy = enemy;
	}
//-------------------------------------------------------------------------------------------------
    void IEnemyState.Execute()
    {
		
		Patrol();
        enemy.Move();
        if (enemy.Target != null && enemy.InThrowRange)
            enemy.ChangeState(new RangedState());
    }
//-------------------------------------------------------------------------------------------------
    void IEnemyState.Exit()
    {
        
    }
//-------------------------------------------------------------------------------------------------
    void IEnemyState.OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "edge")
        {
            enemy.ChangeDirection();
        }
    }
//-------------------------------------------------------------------------------------------------
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration)
        {
			enemy.myAnimator.SetFloat("speed", 0);
			enemy.ChangeState(new IdleState());
        }
    }
//-------------------------------------------------------------------------------------------------

}
