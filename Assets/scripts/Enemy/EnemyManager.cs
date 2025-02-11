using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public List<DataFish> allDataEnemy;
    public Transform[] listPos;
    public GameObject prefap;

    public Transform parent_Active;
    public Transform parent_Deactive;

    public int countEnemy;
    public float timeSpawn;
    public EnemyType[] enemyTypes;
    public LevelType[] lvs;

    public float cur_time;
    private void Start()
    {
        cur_time = timeSpawn;
    }
    private void Update()
    {
        if(cur_time > 0) cur_time -= Time.deltaTime;
        
        int cur_count = parent_Active.childCount;
        
        if (countEnemy == cur_count || cur_time > 0) return;
        cur_time = timeSpawn;
        spawnEnemy();

    }
    public void spawnEnemy() 
    {
        //random pos
        float posX = Random.Range(listPos[0].position.x, listPos[1].position.x);
        float posY = Random.Range(listPos[0].position.y, listPos[1].position.y);
        Vector2 pos = new Vector2(posX, posY);

        //random type
        int ranInt = Random.Range(0, 10 * enemyTypes.Length) / 10;
        EnemyType type = enemyTypes[ranInt];

        //random level
        ranInt = Random.Range(0, 10 * lvs.Length) / 10;
        LevelType lv = lvs[ranInt];

        //create enemy
        GameObject obj = PoolManager.instance.pool(0, pos, parent_Active);
        obj.GetComponent<EnemyController>().initData(type, lv);
    }
    public DataFish getData(EnemyType type)
    {
        return allDataEnemy.
            FirstOrDefault(data => data.type == type);
    }

    public bool isGizmos;
    private void OnDrawGizmos()
    {
        if (!isGizmos) return;
        Gizmos.color = Color.green;
    }
}
