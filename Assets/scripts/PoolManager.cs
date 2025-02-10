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
        if (Pools[id].Count > 0) 
        {
            obj = Pools[id][0];
            obj.SetActive(true);
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
        Pools[id].Add(obj);
    }

}
