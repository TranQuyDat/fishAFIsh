using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory 
{
    private readonly Dictionary<(EnemyState, EnemyType),
        Func<DataFish, EnemyController, int,Enemy >> dicEnemy = new ();
    public EnemyFactory() 
    {
        //normal enemy
        add2Dic(EnemyState.NormalEnemy, EnemyType.whale, (data, eCtrl, id) => new FishEnemy(data,eCtrl,id));
        add2Dic(EnemyState.NormalEnemy, EnemyType.whalekiller, (data, eCtrl, id) => new FishEnemy(data,eCtrl,id));
        add2Dic(EnemyState.NormalEnemy, EnemyType.nemo, (data, eCtrl, id) => new FishEnemy(data,eCtrl,id));
        add2Dic(EnemyState.NormalEnemy, EnemyType.anglefish, (data, eCtrl, id) => new FishEnemy(data,eCtrl,id));
        add2Dic(EnemyState.NormalEnemy, EnemyType.shark, (data, eCtrl, id) => new FishEnemy(data,eCtrl,id));

        //dangerous enemy
        add2Dic(EnemyState.DangerousEnemy, EnemyType.whale, (data, eCtrl, id) => new DangerFishEnemy(data, eCtrl, id));
        add2Dic(EnemyState.DangerousEnemy, EnemyType.whalekiller, (data, eCtrl, id) => new DangerFishEnemy(data, eCtrl, id));
        add2Dic(EnemyState.DangerousEnemy, EnemyType.nemo, (data, eCtrl, id) => new DangerFishEnemy(data, eCtrl, id));
        add2Dic(EnemyState.DangerousEnemy, EnemyType.anglefish, (data, eCtrl, id) => new DangerFishEnemy(data, eCtrl, id));
        add2Dic(EnemyState.DangerousEnemy, EnemyType.shark, (data, eCtrl, id) => new DangerFishEnemy(data, eCtrl, id));
    }

    public void add2Dic(EnemyState state,EnemyType type , Func<DataFish, EnemyController, int, Enemy> func)
    {
        dicEnemy.Add((state, type), func);
    }

    public Enemy create(EnemyState state, EnemyType type,DataFish dataFish,EnemyController eCtrl,int idPrefab)
    {
        if(dicEnemy.TryGetValue((state, type), out Func<DataFish, EnemyController, int, Enemy> func))
        {
            return func(dataFish,eCtrl,idPrefab);
        }
        return null;
    }
}
