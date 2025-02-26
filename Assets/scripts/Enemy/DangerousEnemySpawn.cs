using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DangerousEnemySpawn : IEnemySpawn
{
    public Transform[] posList;//left or right
    public Vector2 targetPos;
    public float rangePosY;
    public float timeSpawn;
    public EnemyType[] dangerousType;
    public LevelType lv;
    public EnemyManager enemyManager ;

    System.Random ran;
    float curTime;
    public void init()
    {
        enemyManager = GameManager.instance.enemyManager;
        ran = new System.Random();
        curTime = timeSpawn;
    }
    public void update()
    {
        if(curTime > 0)
        {
            curTime -= Time.deltaTime;
            return;
        }
        spawnDangerousEnemy();
    }

    public void spawnDangerousEnemy()
    {
        //random type
        int ranInt = ran.Next(0, dangerousType.Length);
        EnemyType type = dangerousType[ranInt];

        //random pos 
        ranInt = Random.Range(0, 100) % 2;

        float y = Random.Range(posList[ranInt].position.y - rangePosY,
            posList[ranInt].position.y + rangePosY);
        Vector2 pos = new Vector2(posList[ranInt].position.x, y);

        targetPos = new Vector2(posList[(ranInt + 1) % 2].position.x, y);

        //create enemy

        GameObject obj = PoolManager.instance.pool(0, pos, enemyManager.parent_Active);
        infoEnemy info = new infoEnemy(type, EnemyState.DangerousEnemy, lv);
        obj.GetComponent<EnemyController>().initData(info);
        curTime = timeSpawn;
    }

    public void OnGizmos() 
    {
        Gizmos.color = Color.green;
        if (posList == null || posList.Length <= 0) return;
        Gizmos.color = Color.black;
        foreach (Transform tran in posList)
        {
            Vector2 pos = tran.position;
            Gizmos.DrawLine(new Vector2(pos.x, pos.y + rangePosY)
                , new Vector2(pos.x, pos.y - rangePosY));
        }
    }
}
