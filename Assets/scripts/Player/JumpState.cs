using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    PlayerController player;
    Rigidbody2D rb;
    public JumpState(Rigidbody2D rb , PlayerController player )
    {
        this.player = player;
        this.rb = rb;
    }

    public void Enter()
    {
        player.actionType = ActionType.jump;
        //hd jump
        Vector2 dir = (Vector2.up + Vector2.right * rb.transform.localScale.x).normalized ;
        rb.AddForce(dir*2, ForceMode2D.Impulse);
    }

    public void Execute()
    {
        //dk de chuyen sang swim
        if (!player.isUpSurFaceWater() && player.isDis2SurfaceWater(0.5f,">") && rb.velocity.y>-1f)
        {
            player.changeState(new SwimState(rb,player));
            return;
        }

        
    }

    public void Exit()
    {
        //Debug.Log("stop jump");
    }
}
