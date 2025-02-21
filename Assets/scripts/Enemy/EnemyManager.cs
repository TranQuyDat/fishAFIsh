using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class NormalEnemy
{
    public Transform postopright;
    public Transform posdownleft;
    public int countEnemy;
    public float timeSpawn;
    public List<EnemyType> enemyTypes;
    public List<LevelType> lvs;

}
[System.Serializable]
public class DangeroutEnemy
{
    public Transform[] posList;//left or right
    public Vector2 targetPos;
    public float rangePosY;
    public float timeSpawn;
    public EnemyType[] dangerousType;
    public LevelType lv;
}
public class EnemyManager : MonoBehaviour
{
    public List<DataFish> allDataEnemy;

    public Transform parent_Active;
    public Transform parent_Deactive;

    public NormalEnemy eNormal;
    public DangeroutEnemy eDangerous;


    public static EnemyFactory enemyFactory;
    private void Awake()
    {
        EnemyManager.enemyFactory = new EnemyFactory();
    }

    private void Start()
    {
        InvokeRepeating("spawnNormalEnemy", 0f, eNormal.timeSpawn);
        InvokeRepeating("spawnDangerousEnemy", 0f, eDangerous.timeSpawn);
    }

    public void spawnNormalEnemy()
    {
        if (!GameManager.instance.statGame.isStart) return;
        int cur_count = parent_Active.childCount;
        if (eNormal.countEnemy == cur_count ) return;
        //random pos
        float posX = Random.Range(eNormal.posdownleft.position.x, 
            eNormal.postopright.position.x);
        float posY = Random.Range(eNormal.posdownleft.position.y, 
            eNormal.postopright.position.y);
        Vector2 pos = new Vector2(posX, posY);

        //random type
        int ranInt = Random.Range(0, 100 * eNormal.enemyTypes.Count) % eNormal.enemyTypes.Count;
        EnemyType type = eNormal.enemyTypes[ranInt];

        //random level
        ranInt = Random.Range(0, 100 * eNormal.lvs.Count) % eNormal.lvs.Count;
        LevelType lv = eNormal.lvs[ranInt];

        //create enemy
        GameObject obj = PoolManager.instance.pool(0, pos, parent_Active);
        infoEnemy info = new infoEnemy(type,EnemyState.NormalEnemy, lv);
        obj.GetComponent<EnemyController>().initData(info);
    }

    public void spawnDangerousEnemy()
    {

        if (!GameManager.instance.statGame.isStart) return;
        //random type
        int ranInt = Random.Range(0, 100 * eDangerous.dangerousType.Length) % eDangerous.dangerousType.Length;
        EnemyType type = eDangerous.dangerousType[ranInt];
        //random pos 
        ranInt = Random.Range(0, 100) % 2;

        float y = Random.Range(eDangerous.posList[ranInt].position.y - eDangerous.rangePosY,
            eDangerous.posList[ranInt].position.y + eDangerous.rangePosY);
        Vector2 pos = new Vector2(eDangerous.posList[ranInt].position.x, y);
        
        eDangerous.targetPos = new Vector2(eDangerous.posList[(ranInt+1)%2].position.x,y);
        //create enemy

        GameObject obj = PoolManager.instance.pool(0, pos, parent_Active);
        infoEnemy info = new infoEnemy(type,EnemyState.DangerousEnemy, eDangerous.lv);
        obj.GetComponent<EnemyController>().initData(info);

    }

    public void addTypeAndLv(EnemyType eType,LevelType eLvs)
    {
        if(!eNormal.enemyTypes.Contains(eType))
            eNormal.enemyTypes.Add(eType);
        if (!eNormal.lvs.Contains(eLvs))
            eNormal.lvs.Add(eLvs);
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
        if (eDangerous.posList == null || eDangerous.posList.Length <= 0) return;
        Gizmos.color = Color.black;
        foreach (Transform tran in eDangerous.posList)
        {
            Vector2 pos = tran.position;
            Gizmos.DrawLine(new Vector2(pos.x, pos.y + eDangerous.rangePosY)
                , new Vector2(pos.x, pos.y - eDangerous.rangePosY));
        }
    }
}
