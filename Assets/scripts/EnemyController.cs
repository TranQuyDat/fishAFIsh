using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public  Enemy enemy ;
    public Animator ani;
    public float time;
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
        
        enemy.move();
    }
}
