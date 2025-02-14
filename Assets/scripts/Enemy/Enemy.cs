using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
public abstract class Enemy 
{
    public ActionDelegate cur_action;
    public DataFish _dataFish;
    public GameObject enemyObj;
    public EnemyController enemyCtrl;
    public float speed;
    public PathFinding pathFinding;

    public void init(DataFish dataFish ,GameObject obj  )
    {
        _dataFish = dataFish;
        enemyObj = obj;
        enemyCtrl = obj.GetComponent<EnemyController>();
        enemyCtrl.type = dataFish.type;
        pathFinding = new PathFinding();
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

    public async Task delayWithSeconds(float seconds)
    {
        await Task.Delay((int)(seconds * 1000));
    }
    public async Task delayUntil(bool b)
    {
        while (!b)
        {
            await Task.Yield();
        }
    }
    public abstract void OnGizmos();

}

public class FishEnemy  : Enemy 
{
    Vector3 newPos;
    float timeDelay = 0;
    Vector3 dir;
    Node enemyNode ;
    Node targetNode;
    List<Node> listNode;
    public FishEnemy (DataFish dataFish , GameObject obj ) 
    {
        base.init(dataFish , obj );

        enemyNode = GridManager.instance.posToNode(enemyObj.transform.position);
        targetNode = changePos();
        listNode = pathFinding.findPath(enemyNode, targetNode);
        newPos = listNode[0].pos;
        speed = _dataFish.speed;
    }
   
    public override void move()
    {
        if (listNode == null) return;
        
        // dk: khi đã đến targetNode => random targetNode mới
        if( listNode.Count <=0  || listNode == null ) 
        {
            targetNode = changePos();
            listNode = pathFinding.findPath(enemyNode, targetNode);
        }

        followPath(listNode);
        flip();

        if (enemyCtrl.focusFish == null) return;

        float scaleFocusFish = enemyCtrl.focusFish.transform.localScale.y;
        float scaleThisFish = enemyObj.transform.localScale.y;
        if (scaleThisFish < scaleFocusFish)
        {
            // dk chuyen sang flee
            timeDelay = 3f;
            listNode.Clear();
            speed = _dataFish.speed * 3f;
            enemyCtrl.actionType = ActionType.flee;
            cur_action = flee;
        }

        if (scaleThisFish > scaleFocusFish) 
        {
            // dk chuyen sang chase
            timeDelay = 0;
            listNode.Clear();
            speed = _dataFish.speed * 4f;
            enemyCtrl.actionType = ActionType.chase;
            cur_action = chase;
        }
    }

    public override void flee()
    {

        if (enemyCtrl.focusFish == null) timeDelay -= 1 * Time.deltaTime;



        logicFlee();

        followPath(listNode);
        flip();


        // dk chuyen sang move
        if ((enemyCtrl.focusFish == null && timeDelay <= 0f) ||
            (enemyCtrl.focusFish == null && enemyNode == targetNode)
            )
        {
            if(listNode != null) listNode.Clear();
            speed = _dataFish.speed;
            targetNode = changePos();
            listNode = pathFinding.findPath(enemyNode, targetNode);
            enemyCtrl.actionType = ActionType.swim;
            cur_action = move;
        }

    }

    public override async void chase()
    {
        if(timeDelay >0)
            timeDelay -=  Time.deltaTime;

        float dis = enemyCtrl.radiusToEat + 1f ;
        if (enemyCtrl.focusFish != null)
        {
            Vector2 posOtherFish = enemyCtrl.focusFish.transform.position;
            Vector2 posEnemy = enemyObj.transform.position;
            Vector2 dirChase = (posOtherFish - posEnemy).normalized;
            targetNode = findNodeDir(dirChase);
            dis = (enemyCtrl.PosCheckEnemy.position - enemyCtrl.focusFish.transform.position).magnitude;
            
        }
        if(timeDelay <= 0f && enemyNode != targetNode)
        {
            timeDelay = 2f;
            listNode = pathFinding.findPath(enemyNode, targetNode);

        }
        followPath(listNode);
        flip();
        
        
        // dk chuyen sang eat
        if (enemyCtrl.focusFish != null && dis <= enemyCtrl.radiusToEat)
        {
            enemyCtrl.actionType = ActionType.eat;

            enemyCtrl.ani.SetBool("isEat", true);
            enemyCtrl.ani.SetBool("isSwim", false);
            cur_action = eat;
            return;
        }
        if (enemyCtrl.focusFish != null || timeDelay <= 0 || enemyNode != targetNode) return;
        await delayWithSeconds(0.8f);
        // dk chuyen sang move
        if (listNode != null) listNode.Clear();
        speed = _dataFish.speed;
        targetNode = changePos();
        listNode = pathFinding.findPath(enemyNode, targetNode);
        enemyCtrl.actionType = ActionType.swim;
        cur_action = move;
    }

    public override async void eat()
    {
        // dk chuyen sang move
        bool isFar = enemyCtrl.focusFish!=null &&
            (enemyCtrl.PosCheckEnemy.position - 
            enemyCtrl.focusFish.transform.position).magnitude > enemyCtrl.radiusToEat;

        if (enemyCtrl.focusFish == null || isFar )
        {
            enemyCtrl.actionType = ActionType.swim;
            enemyCtrl.ani.SetBool("isEat", false);
            enemyCtrl.ani.SetBool("isSwim", true);
            cur_action = move;
            return;
        }

        //eat
        
        if (enemyCtrl.focusFish.gameObject.CompareTag("Player")) 
        {
            enemyCtrl.focusFish.GetComponent<PlayerController>().ondead();
            return;
        }

        enemyCtrl.focusFish.GetComponent<EnemyController>().ondead();
            
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
    public void logicFlee()
    {
        if (enemyCtrl.focusFish == null) return;
        Vector2 posEnemy = enemyObj.transform.position;
        Vector2 posPlayer = enemyCtrl.focusFish.transform.position;
        Vector2 dirflee = (posEnemy - posPlayer).normalized;
        if (listNode == null || listNode.Count <= 0)
        {
            targetNode = findNodeDir(dirflee);
            listNode = pathFinding.findPath(enemyNode, targetNode);
        }

        float angle = Vector2.Angle(dir, dirflee * -1);

        //Debug.Log("targetnode = enemynode :"+ (enemyNode == targetNode));
        if (angle <= 60f || (enemyNode == targetNode))
        {
            //Debug.Log("angle");
            targetNode = findNodeDir(dirflee);
            listNode = pathFinding.findPath(enemyNode, targetNode);
        }

     
    }
    public Node findNodeDir(Vector2 dir)
    {
        Node bestNode = enemyNode;
        if(enemyCtrl.focusFish == null) return bestNode;
        Vector2 posEnemy = enemyObj.transform.position;
        float maxAlignment = float.MinValue;
        foreach(Node n in GridManager.grids)
        {
            if (!n.isWalkable || enemyNode == n) continue;
            Vector2 dirNode = ((Vector2)n.pos - posEnemy).normalized;
            float alignment = Vector2.Dot(dir, dirNode);
            if (alignment > maxAlignment)
            {
                maxAlignment = alignment;
                bestNode = n;
            }
        }
        return bestNode;
    }
    public void followPath(List<Node> path)
    {
        if (path == null) return;
        bool isGo = (enemyObj.transform.position - newPos).magnitude <= 0.01f;
        if (path.Count <= 0) return;
        //dk : chuyển sang Node kế tiếp 
        if (isGo && path.Count>0)
        {
            float j = Random.Range(-0.25f, 0.25f);
            newPos = listNode[0].pos + new Vector3(j, j, 0);
            enemyNode = listNode[0];
            path.RemoveAt(0);
        }

        //dk : 1 obj chắn ngang đường => findpath lại từ Node hiện tại đến targetNode
        if(listNode.Count>0 && !listNode[0].isWalkable) 
            listNode = pathFinding.findPath(enemyNode, targetNode);

        Debug.DrawLine(enemyObj.transform.position, newPos);
        dir = (newPos - enemyObj.transform.position).normalized;
        Vector3 pos = enemyObj.transform.position + dir * speed * Time.deltaTime;

        enemyObj.transform.position = pos; // move
    }

    public Node changePos()
    {
        List<Node> nodes = new List<Node>();

        foreach(Node n in GridManager.grids)
        {
            if(n.isWalkable && n != GridManager.instance.posToNode(enemyObj.transform.position))
                nodes.Add(n);
        }

        int i = Random.Range(0,nodes.Count);
        return nodes[i];
    }
    
    #endregion

    //GIZMOS 
    public override void OnGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Ray(enemyObj.transform.position, dir));

        Gizmos.color = Color.red;
        if (enemyCtrl.focusFish != null)
            Gizmos.DrawLine(enemyObj.transform.position, enemyCtrl.focusFish.transform.position);
       
    }
}
