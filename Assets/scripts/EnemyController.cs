using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyActionType { swim, flee, chase, eat }
public class EnemyController : MonoBehaviour
{
    public EnemyActionType actionType;
    public  Enemy enemyscript ;
    public Animator ani;
    public float time;

    public Collider2D OtherFish;
    public LayerMask layerEnemy;
    public Transform PosCheckEnemy;
    public float radiusToEat;//check to eat

    public float radiusScanEnemy; // check to chase or flee
    void Start()
    {
        InvokeRepeating("initData",0,1f);
    }

    public void initData()
    {
        if (enemyscript == null) return;
        this.GetComponent<SpriteRenderer>().sprite = enemyscript._dataFish.sprite;
        ani.runtimeAnimatorController = enemyscript._dataFish.ani;
        CancelInvoke("initData");
    }

    // Update is called once per frame
    void Update()
    {

        OtherFish = Physics2D.OverlapCircle(PosCheckEnemy.position, radiusScanEnemy, layerEnemy);
        
        enemyscript.starAction();
    }
    public bool isGizmos;
    private void OnDrawGizmos()
    {
        if (!isGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PosCheckEnemy.position, radiusToEat);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusScanEnemy);
        enemyscript.OnGizmos();
    }
}
