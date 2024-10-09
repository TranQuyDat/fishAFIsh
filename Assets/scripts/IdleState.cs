using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    PlayerController player;
    Animator ani;
    Rigidbody2D rb;
    Vector2 cursorPos;
    public IdleState(Rigidbody2D rb,PlayerController player)
    {
        this.player = player;
        this.ani = player.ani;
        this.rb = rb;
    }
    public void Enter()
    {
        StateManager.stateTyle = StateTyle.idle;
    }

    public void Execute()
    {
        //dk chuyen sang swim
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dis = ((Vector2)rb.transform.position - cursorPos).magnitude;


        if (dis <= 0.3f) return;
        StateManager.changeState(new SwimState(rb, player));
       
    }

    public void Exit()
    {
        
    }
}
