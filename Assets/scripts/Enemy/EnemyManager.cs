using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public List<DataFish> allDataEnemy;
    public Transform[] listPos;
    public GameObject prefap;
    public Transform parent;

    public Transform limit;
    public Vector2 sizeLimit;
    EnemyFactory enemyFactory;
    public EnemyType demoType;
    public LevelType lv;
    private void Start()
    {
        enemyFactory = new EnemyFactory(allDataEnemy,this);
    }
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 pos = listPos[Random.Range(0, listPos.Length)].position;
        enemyFactory.createEnemy(demoType,lv, prefap, pos, parent);
    }
    public bool isGizmos;
    private void OnDrawGizmos()
    {
        if (!isGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(limit.position, sizeLimit);
    }
}
