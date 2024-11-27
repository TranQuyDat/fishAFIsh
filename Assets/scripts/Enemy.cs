using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public abstract void OnGizmos();

}

public class FishEnemy  : Enemy 
{
    Vector3 dir;
    Vector2 dirWall;
    Vector3 dirFlee;
    List<Vector2> dir_walls;
    Vector3 limit;
    Vector2 sizeLimit;
    Vector3 newPos;
    float timeDelay = 0;
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
        if (enemyCtrl.OtherFish == null)
        {
            cur_action = move;
            enemyCtrl.ani.SetBool("isEat", false);
            enemyCtrl.ani.SetBool("isSwim", true);
            return;
        }
    }

   
    public override void move()
    {
        if ((enemyObj.transform.position - newPos).magnitude <= 1f)
        {
            newPos = changePos();
        }
        Debug.DrawLine(enemyObj.transform.position, newPos);
        Vector3 pos = enemyObj.transform.position + _dataFish.speed * dir * Time.deltaTime;
        flip();
        enemyObj.transform.position = pos;
        dir_walls = DetectWallDirection();
        if (enemyCtrl.OtherFish == null) return;

        // dk chuyen sang flee
        timeDelay = 3f;
        cur_action = flee;
        // dk chuyen sang chase
    }

    public override void flee()
    {
        if(enemyCtrl.OtherFish == null) timeDelay -= 1* Time.deltaTime;
        
        // dk chuyen sang move
        if (enemyCtrl.OtherFish == null && timeDelay<=0f)
        {
            cur_action = move;
            newPos = changePos();
            return;
        }
        if (enemyCtrl.OtherFish != null)
        {
            dirFlee = (enemyObj.transform.position - enemyCtrl.OtherFish.transform.position).normalized;
        }
        dir_walls = DetectWallDirection();
        RaycastHit2D hit = Physics2D.Raycast(enemyObj.transform.position, dir,enemyCtrl.radiusScanEnemy,enemyCtrl.layerWall);
        if(hit)
        {
            dirFlee += (Vector3)Vector2.Perpendicular(dirFlee)+(Vector3)dirWall;
        }
        //Debug.Log(dirFlee +" - "+ dirWall +" = "+dir);

        dir = dirFlee;

        flip();
        enemyObj.transform.position += dir.normalized * _dataFish.speed*3f * Time.deltaTime;

    }

    public override void chase()
    {

        float dis = (enemyCtrl.PosCheckEnemy.position - enemyCtrl.OtherFish.transform.position).magnitude;
        // dk chuyen sang eat
        if (enemyCtrl.OtherFish != null && dis <= enemyCtrl.radiusToEat)
        {
            cur_action = eat;
            enemyCtrl.ani.SetBool("isEat", true);
            enemyCtrl.ani.SetBool("isSwim", false);
            return;
        }
        if (enemyCtrl.OtherFish != null) return;
        
        // dk chuyen sang move
        cur_action = move;
        newPos = changePos();
    }

    //sub method 
    #region sub method 
    public void flip()
    {
        Vector3 scale = enemyObj.transform.localScale;

        float dirx = (dir * 2f + enemyObj.transform.position).x;
        if (dirx - enemyObj.transform.position.x < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
        else if(dirx - enemyObj.transform.position.x > 0)
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
    public List<Vector2> DetectWallDirection()
    {
        List<Vector2> dir = new List<Vector2>();
        Vector3 pos = enemyObj.transform.position;
        dirWall = Vector3.zero;
        // Tia kiểm tra từng hướng
        if (Physics2D.Raycast(pos, Vector2.left, enemyCtrl.radiusScanEnemy, enemyCtrl.layerWall))
        {
            dir.Add(Vector2.left);
            dirWall += Vector2.left;
        }
        if (Physics2D.Raycast(pos, Vector2.right, enemyCtrl.radiusScanEnemy, enemyCtrl.layerWall))
        {
            dir.Add(Vector2.right);
            dirWall += Vector2.right;
        }
        if (Physics2D.Raycast(pos, Vector2.down, enemyCtrl.radiusScanEnemy, enemyCtrl.layerWall))
        {
            dir.Add(Vector2.down);
            dirWall += Vector2.down;
        }
        if (Physics2D.Raycast(pos, Vector2.up, enemyCtrl.radiusScanEnemy, enemyCtrl.layerWall))
        {
            dir.Add(Vector2.up);
            dirWall += Vector2.up;
        }
        dirWall = dirWall.normalized;
        
        return dir;
    }

    #endregion

    //GIZMOS 2193
    public override void OnGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Ray(enemyObj.transform.position, dir));
        Gizmos.DrawRay(new Ray(enemyObj.transform.position, dirWall));

        Gizmos.color = Color.red;
        if (enemyCtrl.OtherFish != null)
            Gizmos.DrawLine(enemyObj.transform.position, enemyCtrl.OtherFish.transform.position);
       
        if (enemyCtrl.wall == null) return;
        foreach (Vector2 dir in dir_walls)
        {
            Gizmos.DrawRay(new Ray(enemyObj.transform.position, dir));
        }
    }
}
