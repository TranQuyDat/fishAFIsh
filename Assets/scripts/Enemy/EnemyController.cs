using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyType type;
    public Level lv;
    public ActionType actionType;
    public Collider2D focusFish;
    public float speed;
    public Transform PosCheckEnemy;
    public float radiusToEat;//check to eat
    public Animator ani;
    public Enemy enemyscript ;
    public LayerMask layerEnemy;

    Collider2D[] otherFishs;

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

        float scale = Mathf.Abs(transform.localScale.x);
        float newRadiusEat = radiusToEat * scale ;
        if (radiusToEat != newRadiusEat)
        {
            radiusToEat = newRadiusEat;
            print(radiusToEat);
        }

        CancelInvoke("initData");
    }

    // Update is called once per frame
    void Update()
    {
        otherFishs = Physics2D.OverlapCircleAll(PosCheckEnemy.position, radiusScanEnemy, layerEnemy);
        checkFocusFish();
        enemyscript.starAction();
        speed = enemyscript.speed;
    }


    public void checkFocusFish()
    {
        if(focusFish!=null && !otherFishs.Contains(focusFish))
        {
            focusFish = null;
        }
        if (focusFish != null) return;
        foreach (Collider2D c in otherFishs)
        {
            if (c.CompareTag("Player"))
            {
                focusFish = c;
                return;
            }
                
            EnemyController eCtrl = c.GetComponent<EnemyController>();
            if (eCtrl.type == this.type) continue;
            focusFish = c;
            return;
        }
    }

    public void ondead()
    {
        Destroy(gameObject);
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
