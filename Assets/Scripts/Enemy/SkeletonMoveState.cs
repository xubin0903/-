using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    public SkeletonMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName) : base(_enemy, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.isMove = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.isMove = false;
    }
}
