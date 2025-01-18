using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        player.actionType = ActionType.swim;
        ani.SetBool("isSwim",true);
    }

    public void Execute()
    {
        //hd swim
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;

        Vector2 dir = (cursorPos - rb.transform.position).normalized;
        rb.position +=  dir * (5+speed) * Time.deltaTime;


        //dk chuyen sang jump
        if(rb.transform.position.y > (surFaceSea.position.y +0.35f ))
        {
            StateManager.changeState(new JumpState(rb, player));
            return;
        }

        //dk chuyen sang eat
        if (player.food == null) return;
        
        float fScaleY = player.food.transform.localScale.y;
        float pScaleY = player.transform.localScale.y;
        
        if(fScaleY > pScaleY) return;

        StateManager.changeState(new EatState(rb, player));
        return;
        
        


    }
    
    public void Exit()
    {
        ani.SetBool("isSwim", false);
    }
}
