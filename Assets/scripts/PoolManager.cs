using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public GameObject[] preFabs;
    public List<List<GameObject>> Pools;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            Destroy(this);
        Pools = new List<List<GameObject>>();
    }

    public GameObject pool(int id,Vector2 pos,Transform parent = null) 
    {
        GameObject obj = null;
        if(Pools.Count <= id) 
        {
            List<GameObject> list = new List<GameObject>();
            Pools.Add(list);
        }
        // pool
        if (Pools[id].Count > 0) 
        {
            obj = Pools[id][0];
            obj.transform.position = pos;
            obj.SetActive(true);
            obj.transform.parent = parent;
            Pools[id].Remove(obj);
            return obj;
        }
        //intantiate
        obj = Instantiate(preFabs[id], pos, Quaternion.identity, parent?parent:transform);
        return obj;
    }

    public void destroy(GameObject obj,int id) 
    {
        if (Pools.Count <= id)
        {
            List<GameObject> list = new List<GameObject>();
            Pools.Add(list);
        }
        obj.SetActive(false);
        obj.transform.parent = GameManager.instance.enemyManager.parent_Deactive;
        Pools[id].Add(obj);
    }

}
