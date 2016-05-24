using UnityEngine;
using System.Collections;

public class IdleState : IEnemyState 
{
    private Enemy enemy;
    private float idleTimer = 0;
    private float idleDuration = 3;

//-------------------------------------------------------------------------------------------------
    void IEnemyState.Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
//-------------------------------------------------------------------------------------------------
    void IEnemyState.Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
//-------------------------------------------------------------------------------------------------
    void IEnemyState.Exit()
    {
        
    }
//-------------------------------------------------------------------------------------------------
    void IEnemyState.OnTriggerEnter(Collider2D other)
    {
        
    }
//-------------------------------------------------------------------------------------------------
    private void Idle()
    {
        enemy.myAnimator.SetFloat("speed", 0);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
//-------------------------------------------------------------------------------------------------
}
