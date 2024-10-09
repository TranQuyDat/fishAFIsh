using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwimState : IState
{
    PlayerController player;
    Rigidbody2D rb;
    float speed;
    Animator ani;
    Transform surFaceSea;
    public SwimState(Rigidbody2D rb, PlayerController player)
    {
        this.player = player;
        this.rb = rb;
        this.speed = player.speed;
        this.ani = player.ani;

        this.surFaceSea = player.surFaceSea;
    }
    public void Enter()
    {
        StateManager.stateTyle = StateTyle.swim;
        ani.SetBool("isSwim",true);
    }

    public void Execute()
    {
        //dk chuyen sang jump
        if(rb.transform.position.y > (surFaceSea.position.y + 1f))
        {
            StateManager.changeState(new JumpState(rb, player));
            return;
        }

        //hd swim
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;

        Vector2 dir = (cursorPos - rb.transform.position).normalized;
        dir = Vector2.ClampMagnitude(dir, 1f);
        Debug.Log(dir);
        rb.position += dir * speed * Time.deltaTime;
        //rb.position = Vector2.Lerp(rb.position, cursorPos, speed * Time.deltaTime); ;
        float dis = dir.magnitude;
        
        // dk chuyen sang Idle
        if (dis > 0.3f) return;
        StateManager.changeState(new IdleState(rb, player));

    }

    public void Exit()
    {
        ani.SetBool("isSwim", false);
    }
}
