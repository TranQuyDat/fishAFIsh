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

    public PathFinding pathFinding;
    public void init(DataFish dataFish ,GameObject obj , EnemyManager enemyMngr)
    {
        _dataFish = dataFish;
        enemyObj = obj;
        enemyCtrl = obj.GetComponent<EnemyController>();
        pathFinding = new PathFinding();
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
    Vector3 limit;
    Vector2 sizeLimit;
    Vector3 newPos;
    float timeDelay = 0;
    Vector3 dir;

    Node enemyNode ;
    Node targetNode;
    List<Node> listNode;
    int index = 0;
    public FishEnemy (DataFish dataFish , GameObject obj, EnemyManager enemyMngr) 
    {
        base.init(dataFish , obj , enemyMngr);
        limit = enemyMngr.limit.position;
        sizeLimit = enemyMngr.sizeLimit;

        enemyNode = GridManager.instance.posToNode(enemyObj.transform.position);
        targetNode = changePos();
        listNode = pathFinding.findPath(enemyNode, targetNode);
        newPos = listNode[index].pos;
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
        bool isGo = (enemyObj.transform.position - newPos).magnitude <= 0.01f;
        if ( isGo && index < listNode.Count -1)
        {
            index += 1;
            enemyNode = listNode[index];
            float j = Random.Range(-0.25f, 0.25f);
            newPos = enemyNode.pos +new Vector3(j,j,0);
        }

        if(isGo &&  index >= listNode.Count - 1)
        {
            index = 0;
            targetNode = changePos();
            listNode = pathFinding.findPath(enemyNode, targetNode);
        }

        Debug.DrawLine(enemyObj.transform.position, newPos);

        dir = (newPos - enemyObj.transform.position).normalized;
        Vector3 pos = enemyObj.transform.position + dir * _dataFish.speed * Time.deltaTime;
        flip();
        enemyObj.transform.position = pos;
        if (enemyCtrl.OtherFish == null) return;

        // dk chuyen sang flee
        timeDelay = 3f;
        //cur_action = flee;
        // dk chuyen sang chase
    }

    public override void flee()
    {
        if(enemyCtrl.OtherFish == null) timeDelay -= 1* Time.deltaTime;
        
        // dk chuyen sang move
        if (enemyCtrl.OtherFish == null && timeDelay<=0f)
        {
            cur_action = move;
            //newPos = changePos();
            return;
        }
        

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
        //newPos = changePos();
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
    public Node changePos()
    {
        List<Node> nodes = new List<Node>();

        foreach(Node n in GridManager.grids)
        {
            if(n.isWalkable)
                nodes.Add(n);
        }

        int i = Random.Range(0,nodes.Count);
        return nodes[i];
    }
    
    #endregion

    //GIZMOS 2193
    public override void OnGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Ray(enemyObj.transform.position, dir));

        Gizmos.color = Color.red;
        if (enemyCtrl.OtherFish != null)
            Gizmos.DrawLine(enemyObj.transform.position, enemyCtrl.OtherFish.transform.position);
       
    }
}
