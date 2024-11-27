using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public  Enemy enemyscript ;
    public Animator ani;
    public float time;

    public Collider2D OtherFish;
    public Collider2D wall;
    public LayerMask layerEnemy;
    public LayerMask layerWall;
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
        
        wall = Physics2D.OverlapCircle(PosCheckEnemy.position, radiusScanEnemy, layerWall);
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
