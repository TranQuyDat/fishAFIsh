using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Enemy 
{
    public ActionDelegate cur_action;
    public DataFish _dataFish;
    public GameObject enemyObj;
    public EnemyController enemyCtrl;
    public float speed;
    public PathFinding pathFinding;
    public bool isfindPath = false;
    public Coroutine finpathCoroutine;
    public int idType;
    public void init(DataFish dataFish ,EnemyController enemyCtrl,int idType)
    {
        _dataFish = dataFish;
        enemyObj = enemyCtrl.gameObject;
        this.enemyCtrl = enemyCtrl;
        enemyCtrl.type = dataFish.type;
        pathFinding = new PathFinding();
        cur_action = move;
        this.idType = idType;
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

    public virtual void OnDead()
    {
        if (finpathCoroutine != null)
        {
            GameManager.instance.StopCoroutine(finpathCoroutine);
            isfindPath = false;
        }
        PoolManager.instance.destroy(enemyObj,idType); // id 0:fish, 
    }

    public abstract void OnGizmos();

}

public class FishEnemy  : Enemy 
{
    float timeDelay = 0;
    Vector3 dir; // huong enemy dang di
    Node enemyNode ;
    Node targetNode;
    List<Node> listNode;
    public bool canFindPath;
    public FishEnemy (DataFish dataFish , EnemyController enemyCtrl, int idType ) 
    {
        base.init(dataFish , enemyCtrl, idType);

        listNode = new List<Node>();
        enemyNode = GridManager.instance.posToNode(enemyObj.transform.position);
        targetNode = changePos();
        speed = _dataFish.speed;
        startFinPath();
    }
   
    public override void move()
    {
        
        updateFinPath(new (() => ( listNode == null || listNode.Count <= 0)), () =>
        {
            targetNode = changePos();
        });

        followPath(listNode);
        flip();

        if (enemyCtrl.focusFish == null) return;

        float scaleFocusFish = enemyCtrl.focusFish.transform.localScale.y;
        float scaleThisFish = enemyObj.transform.localScale.y;
        // ===>dk chuyen sang flee<===
        if (scaleThisFish < scaleFocusFish)
        {
            timeDelay = 0f;
            speed = _dataFish.speed * 4f;
            enemyCtrl.actionType = ActionType.flee;
            cur_action = flee;
        }

        // ===>dk chuyen sang chase<===
        if (scaleThisFish > scaleFocusFish) 
        {
            timeDelay = 1f;
            speed = _dataFish.speed * 3f;
            enemyCtrl.actionType = ActionType.chase;
            cur_action = chase;
        }
    }

    public override void flee()
    {
        // ===>dk chuyen sang move<===
        if ((enemyCtrl.focusFish == null &&  listNode.Count<=0 ) 
            ||
            (enemyCtrl.focusFish == null && enemyNode == targetNode)
            )
        {
            timeDelay = 0f;
            if(listNode != null) listNode.Clear();
            speed = _dataFish.speed;
            targetNode = changePos();
            enemyCtrl.actionType = ActionType.swim;
            cur_action = move;
        }

        //=====>focusFish = null => di chuyen den khi listNode = 0<=====
        if(enemyCtrl.focusFish == null)
        {
            followPath(listNode);
            flip();
            return;
        }
        //=====>hd Flee<=====
        Vector2 dirflee = (enemyObj.transform.position -
            enemyCtrl.focusFish.transform.position).normalized;
        if(listNode == null || listNode.Count<=0 )
            targetNode = findNodeDir(dirflee);
        

        //neu focusfish chan dau (goc 60do) thi update targetNode de chuyen huong
        float angle = Vector2.Angle(dir, dirflee * -1);
        

        //update Find Path
        updateFinPath(new(() => (angle <= 60f || (enemyNode == targetNode))),
            () =>{
                targetNode = findNodeDir(dirflee);
            }
        );

        
        followPath(listNode);
                flip();


    }

    public override async void chase()
    {
        //===>hd chasse<===

        updateFinPath(new(() => (enemyCtrl.focusFish != null)), () =>
        {
            Vector2 dirChase = (enemyCtrl.focusFish.transform.position - enemyObj.transform.position).normalized;
            targetNode = findNodeDir(dirChase);
        });
        followPath(listNode);
        flip();

        // ===>dk chuyen sang move<===
        if (enemyCtrl.focusFish == null && timeDelay <= 0 && enemyNode == targetNode)
        {
            timeDelay = 0;
            if (listNode != null) listNode.Clear();
            speed = _dataFish.speed;
            targetNode = changePos();
            enemyCtrl.actionType = ActionType.swim;
            cur_action = move;
        }

        if (enemyCtrl.focusFish == null) return; 
        
        // get dis to focusFish
        float dis = (enemyCtrl.PosCheckEnemy.position - enemyCtrl.focusFish.transform.position).magnitude;
            
        // ===>dk chuyen sang eat<===
        if (dis <= enemyCtrl.radiusToEat)
        {
            enemyCtrl.actionType = ActionType.eat;

            enemyCtrl.ani.SetBool("isEat", true);
            enemyCtrl.ani.SetBool("isSwim", false);
            cur_action = eat;
            return;
        }

        
    }

    public override async void eat()
    {
        // ===>dk chuyen sang move<===
        bool isToSwim = enemyCtrl.focusFish!=null 
            &&(
            (enemyCtrl.PosCheckEnemy.position - enemyCtrl.focusFish.transform.position).magnitude > enemyCtrl.radiusToEat
            || 
            enemyCtrl.focusFish.transform.localScale.y > enemyObj.transform.localScale.y
            );

        if (enemyCtrl.focusFish == null || isToSwim )
        {
            timeDelay = 0;
            enemyCtrl.actionType = ActionType.swim;
            enemyCtrl.ani.SetBool("isEat", false);
            enemyCtrl.ani.SetBool("isSwim", true);
            cur_action = move;
            return;
        }

        //===>hd eat<===
        
        if (enemyCtrl.focusFish.gameObject.CompareTag("Player")) 
        {
            enemyCtrl.focusFish.GetComponent<PlayerController>().ondead();
            return;
        }

        enemyCtrl.focusFish.GetComponent<EnemyController>().ondead();
            
    }

    //<_________________________SUB_METHOD____________________________> 
    #region sub method 

    public void startFinPath()
    {
        if (isfindPath) return;

        finpathCoroutine = GameManager.instance.StartCoroutine(finpath());
        isfindPath = true;
    }
    public void updateFinPath(Func<bool> funBool, Action update_values = null)
    {
        if (funBool() && update_values !=null)
        {
            //gan 1 vai gia tri
            update_values.Invoke();
        }
        canFindPath = funBool();
    }
    public IEnumerator finpath() 
    {
        while (true)
        {
            if (!canFindPath || targetNode.pos == Vector3.zero)
            {
                yield return null;
                continue;
            }
            this.listNode = pathFinding.findPath(enemyNode, targetNode);
            
            canFindPath=false;
            yield return new WaitForSeconds(timeDelay);
        }
    }
    //flip
    public void flip()
    {
        if (listNode == null || listNode.Count <= 0) return;
        Vector3 scale = enemyObj.transform.localScale;

        float dirx = (listNode[0].pos - enemyObj.transform.position).normalized.x*2f;
        if (dirx < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
        else if(dirx > 0)
        {
            scale.x = Mathf.Abs(scale.x);
            enemyObj.transform.localScale = scale;
        }
    }


    // tim node hop ly nhat theo huong
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

    //follow path
    public void followPath(List<Node> path)
    {
        if (path == null || path.Count <= 0) return;

        // Kiểm tra xem đã gần tới node chưa
        bool isGo = (enemyObj.transform.position - path[0].pos).magnitude <= 0.1f;

        if (isGo )
        {
            enemyNode = path[0];
            path.RemoveAt(0);
            if (listNode.Count <= 0) return;
        }

        Debug.DrawLine(enemyObj.transform.position, path[0].pos);
        // Di chuyển mượt mà tới newPos sử dụng Vector3.MoveTowards
        Vector3 pos = Vector3.MoveTowards(enemyObj.transform.position, path[0].pos, speed * Time.deltaTime);
        enemyObj.transform.position = pos;
    }

    // random Node
    public Node changePos()
    {
        List<Node> nodes = new List<Node>();
        float dis =Vector2.Distance(enemyObj.transform.position, enemyCtrl.PosCheckEnemy.position);
        foreach(Node n in GridManager.grids)
        {
            if (Vector2.Distance(enemyObj.transform.position, n.pos) <= dis 
                || 
                !n.isWalkable
                ||
                n== GridManager.instance.posToNode(enemyObj.transform.position)
                ) 
                continue;
            nodes.Add(n);
        }

        int i = UnityEngine.Random.Range(0,nodes.Count*100)%nodes.Count;
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
