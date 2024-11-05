using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy  
{
    public DataFish _dataFish;
    public GameObject enemyObj;
    public EnemyController enemyScript;
    public EnemyManager enemyMngr;
    public void init(DataFish dataFish ,GameObject obj , EnemyManager enemyMngr)
    {
        _dataFish = dataFish;
        enemyObj = obj;
        enemyScript = obj.GetComponent<EnemyController>();
        this.enemyMngr = enemyMngr;
    }

    public abstract void move();
    public abstract void eat();
    public virtual void escape() { }

}

public class FishEnemy  : Enemy 
{
    Vector2[] dirs ;
    Vector3 dir;
    Vector3 limit;
    Vector2 sizeLimit;
    public float time=0;
    public FishEnemy (DataFish dataFish , GameObject obj, EnemyManager enemyMngr) 
    {
        base.init(dataFish , obj , enemyMngr);
        limit = enemyMngr.limit.position;
        sizeLimit = enemyMngr.sizeLimit;

    }

    public override void eat()
    {
        
    }

   
    public override void move()
    {
        //Debug.Log("move");
        time -= 1 * Time.deltaTime;
        if (time <= 0)
        {
            changeDir();
            time = Random.Range(1, 4);
        }
        
        
        if (isOutLimit())
        {
            Vector3 dir2lmit = (enemyObj.transform.position - limit).normalized;
            dir =  (dir + dir2lmit) ;
        }
        Vector3 newPos = enemyObj.transform.position + dir * _dataFish.speed * Time.deltaTime;
        
        newPos.x = Mathf.Clamp(newPos.x,limit.x - sizeLimit.x , limit.x + sizeLimit.x);
        newPos.y = Mathf.Clamp(newPos.y, limit.y - sizeLimit.y , limit.y + sizeLimit.y);
        flip(newPos);
        enemyObj.transform.position = newPos;
    }
    public override void escape()
    {
        
    }

    public void flip( Vector2 newPos)
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
    public void changeDir()
    {
        dirs = new Vector2[]
        {
            Vector2.up,Vector2.down, Vector2.left , Vector2.right,
            Vector2.up + Vector2.left, Vector2.down + Vector2.left,
            Vector2.up + Vector2.right, Vector2.down + Vector2.right,
        };
        dir = dirs[Random.Range(0, dirs.Length)];
    }

    public bool isOutLimit()
    {
        Vector2 pos = enemyObj.transform.position;
        float disx = Mathf.Abs(pos.x - limit.x);
        float disy = Mathf.Abs(pos.y - limit.x);

        return disx >sizeLimit.x || disy >sizeLimit.y;
    }
}
