using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SwimState : IState
{
    PlayerController player;
    Rigidbody2D rb;
    float speed;
    Animator ani;
    Transform surFaceSea;

    bool canSwim;
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
        player.actionType = ActionType.swim;
        ani.SetBool("isSwim",true);
    }

    public void Execute()
    {
        //hd swim
        Vector3 inputMove = Input.mousePosition;
        inputMove.z = 10f;

        player.cursorPos = Camera.main.ScreenToWorldPoint(inputMove) ;
        
        Vector2 dir = (player.cursorPos - (Vector2)player.transform.position);
        
        if(dir.magnitude > (0.5f * player.transform.localScale.y)) canSwim = true;
        if(dir.magnitude < (0.3f * player.transform.localScale.y)) canSwim = false;

        if (!player.isUpSurFaceWater() && canSwim )
        {
            Vector2 pos = rb.position+ dir.normalized * (speed) * Time.deltaTime;
            rb.position = pos; 
        }


        //dk chuyen sang jump
        if( !player.isUpSurFaceWater() && player.isDis2SurfaceWater(0.5f,"<") && rb.velocity.y <-1f)
        {
            player.changeState(new JumpState(rb, player));
            return;
        }

        //dk chuyen sang eat
        if (player.food == null) return;
        
        float fScaleY = player.food.transform.localScale.y;
        float pScaleY = player.transform.localScale.y;
        
        if(fScaleY > pScaleY) return;

        player.changeState(new EatState(rb, player));
        return;
        
        


    }
    
    public void Exit()
    {
        ani.SetBool("isSwim", false);
    }
}
