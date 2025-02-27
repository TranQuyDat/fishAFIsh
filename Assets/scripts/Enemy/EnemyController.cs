using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class infoEnemy
{
    public EnemyType type;
    public EnemyState state;
    public LevelType lv;
    public float speed;
    public bool canInit = false;
    public infoEnemy(EnemyType type,EnemyState state, LevelType lv, float speed=0)
    {
        this.type = type;
        this.state = state;
        this.lv = lv;
        this.speed = speed;
    }

}
public class EnemyController : MonoBehaviour
{
    
    public infoEnemy info;
    public ActionType actionType;
    public Collider2D focusFish;
    public float radiusToEat;
    public LayerMask layerEnemy;
    public float radiusScanEnemy;
    public Animator ani;
    public Enemy enemyscript;

    public SpriteRenderer sr;
    public CircleCollider2D cirCollider;
    Collider2D[] otherFishs;

    void Start()
    {
        if (info.canInit) 
        {
            initData(info);
            info.canInit = false;
        }

    }
    private void OnEnable()
    {


        float maxSize = Mathf.Max(sr.bounds.size.x, sr.bounds.size.y);
        cirCollider.radius = maxSize/2;
    }
    public void initData(infoEnemy info)
    {
        DataFish dataFish = GameManager.instance.enemyManager.getData(info.type);
        enemyscript = EnemyManager.enemyFactory.create(info.state, info.type, dataFish, this, 0);


        //set 
        this.info =info;

        this.GetComponent<SpriteRenderer>().sprite = dataFish.sprite;
        ani.runtimeAnimatorController = dataFish.ani;

        float mapScale = GameManager.instance.setting.mapScale;

        transform.localScale = dataFish.scaleLV[(int)this.info.lv] * mapScale;

        float scale = transform.localScale.y  ;// cap nhat scale


        this.radiusScanEnemy = 2.13f * scale ;
        radiusToEat = dataFish.radiusHead * scale;


    }

    // Update is called once per frame
    void Update()
    {
        if (enemyscript == null) return;
        otherFishs = Physics2D.OverlapCircleAll(transform.position, radiusScanEnemy, layerEnemy);
        checkFocusFish();
        enemyscript.starAction();
        info.speed = enemyscript.speed;
    }


    public void checkFocusFish()
    {
        if (otherFishs.Length <= 0) return;
        if(focusFish!=null && !otherFishs.Contains(focusFish))
        {
            focusFish = null;
        }
        if (focusFish != null && otherFishs.Length==1) return;
        float dis = radiusScanEnemy+1;
        Collider2D col = null;
        foreach (Collider2D c in otherFishs)
        {
            if (dis < Vector2.Distance(c.transform.position, transform.position)) 
                continue;
            col = c;
        }
        if (col == null) return;
        if (col.CompareTag("Player"))
        {
            focusFish = col;
            return;
        }
                
        EnemyController eCtrl = col.GetComponent<EnemyController>();
        if (eCtrl.info.type == this.info.type) return ;
            focusFish = col;
            return;
    }

    public Collider2D[] getOtherFish()
    {
        return otherFishs; 
    }

    public bool isAniTimeSame(float time )
    {
        AnimatorStateInfo curAni = ani.GetCurrentAnimatorStateInfo(0);
        //print(curAni.normalizedTime%1);
        return (curAni.normalizedTime % 1) >= time;
    }

    public void ondead()
    {
        enemyscript.OnDead();
    }

    public bool isGizmos;
    private void OnDrawGizmos()
    {
        if (!isGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusToEat);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusScanEnemy);
        enemyscript.OnGizmos();
    }
}
