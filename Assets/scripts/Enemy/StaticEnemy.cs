using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy , IIdle
{
    float timeDelay = 5;
    float resetTime = 5;
    public StaticEnemy(DataFish dataFish, EnemyController enemyCtrl, int idType)
    {
        base.init(dataFish, enemyCtrl, idType);
        cur_action = idle;
    }

    public void idle()
    {
        if (timeDelay > 0)
        {
            timeDelay -= Time.deltaTime;
            return;
        }
        cur_action = eat;

    }

    public override void eat()
    {
        foreach(Collider2D otherFish in enemyCtrl.getOtherFish())
        {
            if (otherFish.CompareTag("Player"))
            {
                PlayerController playCtrl = otherFish.GetComponent<PlayerController>();
                playCtrl.ondead();
                continue;
            }
            EnemyController otherFishCtrl = otherFish.GetComponent<EnemyController>();
            otherFishCtrl.ondead();
        }
        timeDelay = resetTime;

    }
    public override void OnGizmos()
    {
    }
}
