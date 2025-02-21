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
        enemyCtrl.info.type = dataFish.type;
        pathFinding = new PathFinding();
        cur_action = move;
        this.idType = idType;
        this.speed = dataFish.speed;
    }
    public void starAction()
    {
        if (!GameManager.instance.statGame.isStart) return;
        cur_action?.Invoke();
    }
    public delegate void ActionDelegate();
    public abstract void move();

    public virtual void eat() 
    {
        //===>hd eat<===
        if (enemyCtrl.focusFish == null) return;
        if (enemyCtrl.focusFish.gameObject.CompareTag("Player"))
        {
            enemyCtrl.focusFish.GetComponent<PlayerController>().ondead();
            return;
        }

        enemyCtrl.focusFish.GetComponent<EnemyController>().ondead();
    }

    public virtual void flee() 
    {

    }
    
    public virtual void chase() 
    {

    }

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
