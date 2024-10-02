using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimState : IState
{
    Rigidbody2D rb;
    float speed;
    Animator ani;
    public SwimState(Rigidbody2D rb, float speed, Animator ani)
    {
        this.rb = rb;
        this.speed = speed;
        this.ani = ani;
    }
    public void Enter()
    {
        StateManager.stateTyle = StateTyle.swim;
    }

    public void Execute()
    {
        ani.SetBool("isSwim",true);
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;

        Vector2 dir = (cursorPos - rb.transform.position).normalized;
        
        rb.position += dir * speed * Time.deltaTime;
    }

    public void Exit()
    {
        ani.SetBool("isSwim", false);
        StateManager.stateTyle = StateTyle.idle;
    }
}
