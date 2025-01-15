using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]


public class EnemyFactory : MonoBehaviour
{
    public List<DataFish> allDataEnemy;
    EnemyManager enemyMngr;

    public EnemyFactory(List<DataFish> allDataEnemy, EnemyManager enemyMngr)
    {
        this.allDataEnemy = allDataEnemy;
        this.enemyMngr = enemyMngr;
    }



    public Enemy createEnemy(EnemyType type,Level lv, GameObject prefap, Vector2 pos, Transform parent)
    {
        DataFish dataFish = getDataEnemy(type);

        GameObject obj = Instantiate(prefap, pos, Quaternion.identity, parent);
        EnemyController objCtrl = obj.GetComponent<EnemyController>();

        obj.transform.localScale = dataFish.scale * (float)lv;

        Enemy e = type  switch 
        {
            EnemyType.whalekiller or EnemyType.shark or EnemyType.anglefish or EnemyType.nemo or EnemyType.whale
              => new FishEnemy(dataFish,obj,enemyMngr),
            _ => null
        };
        objCtrl.enemyscript = e;
        objCtrl.lv = lv;
        return  e;
    }


    

    public DataFish getDataEnemy(EnemyType type)
    {
        return this.allDataEnemy.
            FirstOrDefault(data => data.type == type);
    }
}
