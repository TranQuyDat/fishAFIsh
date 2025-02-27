using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

public class EnemySpawnController
{
    List<IEnemySpawn> listSpawn = new List<IEnemySpawn>();

    public void start()
    {

        foreach (IEnemySpawn sp in listSpawn)
        {
            sp.init();
        }
    }
    public void update()
    {
        if (listSpawn == null || listSpawn.Count <= 0) return;
        foreach (IEnemySpawn sp in listSpawn) 
        {
            sp.update();
        }
    }


    public void add2ListSpawn(IEnemySpawn sp)
    {
        listSpawn.Add(sp);
    }
}
    
public class EnemyManager : MonoBehaviour
{
    public List<DataFish> allDataEnemy;

    public Transform parent_Active;
    public Transform parent_Deactive;
    public NormalEnemySpawn normalEnemySpawn;
    public DangerousEnemySpawn dangerousEnemySpawn;

    EnemySpawnController spawnController = new EnemySpawnController();

    public static EnemyFactory enemyFactory;
    private void Awake()
    {
        EnemyManager.enemyFactory = new EnemyFactory();
        spawnController.add2ListSpawn(normalEnemySpawn);
        spawnController.add2ListSpawn(dangerousEnemySpawn);
    }

    private void Start()
    {
        spawnController.start();
    }

    private void Update()
    {

        if (!GameManager.instance.statGame.isStart) return;
        spawnController.update();
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
        dangerousEnemySpawn.OnGizmos();
    }
}
