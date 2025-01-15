using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    PlayerController player;
    Rigidbody2D rb;
    bool isJump = false;
    Transform surFaceSea;
    public JumpState(Rigidbody2D rb , PlayerController player )
    {
        this.player = player;
        this.rb = rb;
    }

    public void Enter()
    {
        StateManager.stateTyle = ActionType.jump;
    }

    public void Execute()
    {
        this.surFaceSea = player.surFaceSea;
        //dk de chuyen sang swim
        if (rb.transform.position.y < (surFaceSea.position.y - 0.5f))
        {
            StateManager.changeState(new SwimState(rb,player));
            return;
        }

        //hd jump
        if (isJump) return;
        //Debug.Log("jump");
        Vector2 dir = (Vector2.up + Vector2.right * rb.transform.localScale.x).normalized ;
        rb.AddForce(dir, ForceMode2D.Impulse);
        isJump = true;
    }

    public void Exit()
    {
        //Debug.Log("stop jump");
        isJump = false;
    }
}
