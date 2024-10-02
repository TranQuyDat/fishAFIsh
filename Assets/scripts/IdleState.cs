using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    PlayerController player;
    Animator ani;
    public IdleState(PlayerController player, Animator ani)
    {
        this.player = player;
        this.ani = ani;
    }
    public void Enter()
    {
        StateManager.stateTyle = StateTyle.idle;
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
