using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DangerFishEnemy : Enemy
{
    Vector2? tarGetPos =null;
    public DangerFishEnemy(DataFish dataFish, EnemyController enemyCtrl, int idType )
    {
        base.init(dataFish, enemyCtrl, idType);
        tarGetPos = GameManager.instance.enemyManager.eDangerous.targetPos;
    }

    public override void move()
    {
        if (tarGetPos == null) return;
        enemyObj.transform.position = Vector2.MoveTowards(enemyObj.transform.position, (Vector2)tarGetPos, speed *10* Time.deltaTime);
        flip();
        if (Vector2.Distance(enemyObj.transform.position, (Vector2)tarGetPos) <= 0.1f)
        {
            OnDead();
        }

        //===>dk chuyen sang eat<===
        if (enemyCtrl.focusFish == null) return;

        // get dis to focusFish
        float dis = (enemyCtrl.PosCheckEnemy.position - enemyCtrl.focusFish.transform.position).magnitude;

        if (dis <= enemyCtrl.radiusToEat)
        {
            enemyCtrl.actionType = ActionType.eat;

            enemyCtrl.ani.SetBool("isEat", true);
            enemyCtrl.ani.SetBool("isSwim", false);
            cur_action = eat;
            return;
        }
    }

    public override void eat()
    {
        //===>dh eat<===
        base.eat(); 

        // ===>dk chuyen sang move<===
        bool isToSwim = enemyCtrl.focusFish != null
            && (
            (enemyCtrl.PosCheckEnemy.position - enemyCtrl.focusFish.transform.position).magnitude > enemyCtrl.radiusToEat
            ||
            enemyCtrl.focusFish.transform.localScale.y > enemyObj.transform.localScale.y
            );

        if (enemyCtrl.focusFish == null || isToSwim)
        {
            enemyCtrl.actionType = ActionType.swim;
            enemyCtrl.ani.SetBool("isEat", false);
            enemyCtrl.ani.SetBool("isSwim", true);
            cur_action = move;
            return;
        }


        
    }

    public void flip()
    {
        Vector3 scale = enemyObj.transform.localScale;

        float dirx = ((Vector3)tarGetPos - enemyObj.transform.position).normalized.x * 2f;
        if (dirx < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
        else if (dirx > 0)
        {
            scale.x = Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
    }

    public override void OnGizmos()
    {
        
    }
}
