using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;

public class EatState : IState
{
    Rigidbody2D rb;
    PlayerController player;
    Animator ani;

    public EatState(Rigidbody2D rb, PlayerController player) 
    {
        this.rb = rb;
        this.player = player;
        this.ani = player.ani;
    }
    public void Enter()
    {
        player.actionType = ActionType.eat;
        ani.SetBool("isEat", true);
    }

    public void Execute()
    {
        if(player.food != null)
        {
           EnemyController enemyCtrl = player.food.GetComponent<EnemyController>();
            enemyCtrl.ondead();
            float exp = enemyCtrl.enemyscript._dataFish.expReward * (float)enemyCtrl.lv;
            player.addExp(exp);
        }
        if (player.food == null)
        {
            player.changeState(new SwimState(rb,player));
        }

    }

    public void Exit()
    {
        ani.SetBool("isEat", false);
    }
}
