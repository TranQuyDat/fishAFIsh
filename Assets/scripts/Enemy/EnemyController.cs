using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyType type;
    public LevelType lv;
    public ActionType actionType;
    public Collider2D focusFish;
    public float speed;
    public Transform PosCheckEnemy;
    public float radiusToEat;//check to eat
    public Animator ani;
    public Enemy enemyscript ;
    public LayerMask layerEnemy;
    public bool canInit = false;
    Collider2D[] otherFishs;

    public float radiusScanEnemy; // check to chase or flee
    void Start()
    {
        if (canInit) 
        {
            initData(type);
            canInit = false;
        }
    }

    public void initData(EnemyType type)
    {
        DataFish dataFish = GameManager.instance.enemyManager.getData(type);
        enemyscript = type switch
        {
            EnemyType.whalekiller or EnemyType.shark or EnemyType.anglefish or EnemyType.nemo or EnemyType.whale
              => new FishEnemy(dataFish, gameObject),
            _ => null
        };

        this.lv = GameManager.instance.enemyManager.lv;
        this.type = type;

        this.GetComponent<SpriteRenderer>().sprite = dataFish.sprite;
        ani.runtimeAnimatorController = dataFish.ani;
        transform.localScale = dataFish.scale;

        float scale = Mathf.Abs(transform.localScale.x);// cap nhat scale
        float newRadiusEat = radiusToEat * scale ;

        if (radiusToEat != newRadiusEat)
        {
            radiusToEat = newRadiusEat;
        }

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
        PoolManager.instance.destroy(gameObject, 0);
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
