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

    public EnemyType demoType;
    public LevelType lv;


    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 pos = listPos[Random.Range(0, listPos.Length)].position;
        GameObject obj = PoolManager.instance.pool(0,pos, parent);
        obj.GetComponent<EnemyController>().initData(demoType);
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
