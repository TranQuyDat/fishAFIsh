using System.Collections;
using System.Collections.Generic;
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
        //dk chuyen sang swim
        if (player.food == null || 
            (player != null && player.transform.localScale.y < player.food.transform.localScale.y))
        {
            player.changeState(new SwimState(rb,player));
        }
        // hd eat
        else
        {
           EnemyController enemyCtrl = player.food.GetComponent<EnemyController>();
            enemyCtrl.ondead();

            SoundManager.Instance.PlaySFX(SFXType.Eat,player.transform.position);
            float exp = enemyCtrl.enemyscript._dataFish.expReward * 
                (enemyCtrl.transform.localScale.y / player.transform.localScale.y);
            player.addExp(exp);
            player.addScore(enemyCtrl);
        }

    }

    public void Exit()
    {
        ani.SetBool("isEat", false);
    }
}
