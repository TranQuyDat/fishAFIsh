using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public  Enemy enemy ;
    public Animator ani;
    public float time;

    public Collider2D food;
    public LayerMask layerFood;
    public Transform PosCheckFood;
    public float radiusCheck;
    void Start()
    {
        InvokeRepeating("initData",0,1f);
    }

    public void initData()
    {
        if (enemy == null) return;
        this.GetComponent<SpriteRenderer>().sprite = enemy._dataFish.sprite;
        ani.runtimeAnimatorController = enemy._dataFish.ani;
        CancelInvoke("initData");
    }

    // Update is called once per frame
    void Update()
    {
        food = Physics2D.OverlapCircle(PosCheckFood.position, radiusCheck, layerFood);
        enemy.starAction();
    }
    public bool isGizmos;
    private void OnDrawGizmosSelected()
    {
        if (!isGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PosCheckFood.position, radiusCheck);
    }
}
