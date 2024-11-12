using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy 
{
    public ActionDelegate cur_action;
    public DataFish _dataFish;
    public GameObject enemyObj;
    public EnemyController enemyCtrl;
    public EnemyManager enemyMngr;
    public void init(DataFish dataFish ,GameObject obj , EnemyManager enemyMngr)
    {
        _dataFish = dataFish;
        enemyObj = obj;
        enemyCtrl = obj.GetComponent<EnemyController>();
        this.enemyMngr = enemyMngr;
        cur_action = move;
    }
    public void starAction()
    {
        cur_action?.Invoke();
    }
    public delegate void ActionDelegate();
    public abstract void move();
    public abstract void eat();
    public abstract void flee();
    public abstract void chase();

  

}

public class FishEnemy  : Enemy 
{
    Vector3 dir;
    Vector3 limit;
    Vector2 sizeLimit;
    Vector3 newPos;
    public FishEnemy (DataFish dataFish , GameObject obj, EnemyManager enemyMngr) 
    {
        base.init(dataFish , obj , enemyMngr);
        limit = enemyMngr.limit.position;
        sizeLimit = enemyMngr.sizeLimit;
        newPos = changePos();
    }

    public override void eat() 
    {
        // dk chuyen sang move
        if (enemyCtrl.food == null)
        {
            cur_action = move;
            enemyCtrl.ani.SetBool("isEat", false);
            enemyCtrl.ani.SetBool("isSwim", true);
            return;
        }
    }

   
    public override void move()
    {
        if((enemyObj.transform.position - newPos).magnitude <=1f)
        {
            newPos = changePos();
        }
        Debug.DrawLine(enemyObj.transform.position,newPos);
        Vector3 pos = enemyObj.transform.position + _dataFish.speed * dir * Time.deltaTime;
        flip();
        enemyObj.transform.position = pos ;

        // dk chuyen sang eat
        if (enemyCtrl.food !=null)
        {
            cur_action = eat;
            enemyCtrl.ani.SetBool("isEat", true);
            enemyCtrl.ani.SetBool("isSwim", false);
            return;
        }
        // dk chuyen sang flee

        // dk chuyen sang chase
    }

    public override void flee()
    {
        // dk chuyen sang move
    }

    public override void chase()
    {
        // dk chuyen sang move
    }


    public void flip()
    {
        Vector3 scale = enemyObj.transform.localScale;
        if (newPos.x - enemyObj.transform.position.x < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
        else if(newPos.x - enemyObj.transform.position.x > 0)
        {
            scale.x = Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
    }
    public Vector2 changePos()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(limit.x - (sizeLimit.x/2), limit.x + (sizeLimit.x/2));
        pos.y = Random.Range(limit.y - (sizeLimit.y/2), limit.y + (sizeLimit.y/2));
        dir = (pos - enemyObj.transform.position).normalized;
        return pos;
    }

}
