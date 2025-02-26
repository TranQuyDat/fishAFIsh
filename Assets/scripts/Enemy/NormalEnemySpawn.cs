using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[System.Serializable]
public class EnemyValue
{
    public EnemyType type;
    public LevelType lvType;
    public int percent;
}
[System.Serializable]
public class NormalEnemySpawn : IEnemySpawn
{
    public Transform postopright;
    public Transform posdownleft;
    public int countEnemy;
    public float timeSpawn;
    public List<EnemyValue> enemys;

    EnemyManager enemyManager ;
    List<EnemyValue> validEnemys;
    Dictionary<(EnemyType, LevelType), int> countEnemyValue =
        new Dictionary<(EnemyType, LevelType), int>();
    int maxPercent;
    System.Random ran;
    float curTime;
    public void init()
    {
        enemyManager = GameManager.instance.enemyManager;
        ran = new System.Random();
        validEnemys = new List<EnemyValue>();
        foreach (EnemyValue e in enemys)
        {
            countEnemyValue.Add((e.type, e.lvType), 0);
        }

        checkActiveEnemy();

        curTime = timeSpawn;
    }
    public void update()
    {
        if(curTime > 0)
        {
            curTime -= Time.deltaTime;
            return;
        }
        spawnNormalEnemy();
    }

    public void spawnNormalEnemy()
    {
        int cur_count = enemyManager.parent_Active.childCount;
        if (countEnemy <= cur_count) return;

        add2ValidEnemys();
        if (validEnemys.Count <= 0) return;
        //random pos
        float posX = UnityEngine.Random.Range(posdownleft.position.x,
            postopright.position.x);
        float posY = UnityEngine.Random.Range(posdownleft.position.y,
            postopright.position.y);
        Vector2 pos = new Vector2(posX, posY);

        //random enemyValues
        int ranId = ran.Next(0,maxPercent-1);
        EnemyValue enemyValue = randomEnemyValid(ranId);

        //create enemy
        GameObject obj = PoolManager.instance.pool(0, pos, enemyManager.parent_Active);
        infoEnemy info = new infoEnemy(enemyValue.type, EnemyState.NormalEnemy, enemyValue.lvType);
        obj.GetComponent<EnemyController>().initData(info);
        curTime = timeSpawn;
        addCountEnemyValue((enemyValue.type,enemyValue.lvType),1);
    }


    public EnemyValue randomEnemyValid(int ranId)
    {
        int w = 0;
        foreach (EnemyValue enemy in validEnemys)
        {
            w = (w + enemy.percent);
                Debug.Log(ranId);
            if (ranId < w)
            {
                return enemy;
            }
        }
        return null;
    }

    public void add2ValidEnemys()
    {
        maxPercent = 0;
        validEnemys.Clear();
        foreach (EnemyValue enemy in enemys)
        {
            if (countEnemyValue[(enemy.type, enemy.lvType)] >= (int)((enemy.percent / 100f) * countEnemy)) continue;
            validEnemys.Add(enemy);
            maxPercent += enemy.percent;
        }
    }

    public void checkActiveEnemy()
    {
        EnemyController[] enemyActives = enemyManager.parent_Active.GetComponentsInChildren<EnemyController>();
        if (enemyActives.Length <= 0) return;
        foreach (EnemyController e in enemyActives)
        {
            if (!countEnemyValue.ContainsKey((e.info.type, e.info.lv))) return;
            addCountEnemyValue((e.info.type, e.info.lv), 1);
        }
    }

    public void addCountEnemyValue((EnemyType, LevelType) key, int c)
    {
        if(!countEnemyValue.ContainsKey(key)) return;
        countEnemyValue[key] += c;
    }

}
