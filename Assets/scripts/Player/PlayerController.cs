using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class Level
{
    public LevelType lv = LevelType.child;
    public Vector2 exp_MaxExp;
    public void addExp(float exp ,Transform player)
    {
        exp_MaxExp.x = Mathf.Min(exp_MaxExp.x + exp, exp_MaxExp.y);
        if (exp_MaxExp.x < exp_MaxExp.y) return;
        exp_MaxExp.x = 0;
        exp_MaxExp.y += (exp_MaxExp.y * 0.3f);
        levelUp(player);
    }
    public void levelUp(Transform obj)
    {
        int newlv = math.min((int)LevelType.old, (int)lv + 1);

        if ((int)lv == newlv)
        {
            GameManager.instance.statGame.isWin = true;
            return;
        }
        lv = (LevelType)(newlv);

        PlayerController playerCtrl = obj.GetComponent<PlayerController>();
        obj.localScale = playerCtrl.data.scaleLV[newlv]*GameManager.instance.setting.mapScale;

        //neu lv up thi update head radius
        playerCtrl.headRadius = playerCtrl.data.radiusHead * obj.localScale.y;
        playerCtrl.speed += newlv;

    }
}
public class PlayerController : MonoBehaviour
{
    
    [Header("FILL")]
    public Rigidbody2D rb;
    public Animator ani;
    public SpriteRenderer spriteRenderer;
    public Transform surFaceSea;
    public DataFish data;
    [Header("Info settings")]
    public float speed;
    public Level lv;
    public ActionType actionType;
    [Header("Interaction settings")]
    public Vector2 cursorPos;
    public float headRadius;
    public LayerMask layerFood;
    public Collider2D food;
    public float score { get;private set;}


    float waterDrag = 10f;
    StateManager stateManager;
    int comboCount = 0;
    float comboTime = 2f;
    float resetComboTime = 2f;
    bool isUnderSurfacewater = false;
    void Start()
    {
        init();
        stateManager = new StateManager();
        stateManager.changeState(new SwimState(rb, this));
        GameManager.instance.uiGame.img_Avt.sprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.statGame.isStart) return;
        food = Physics2D.OverlapCircle(transform.position,headRadius, layerFood);

        stateManager.excute();
        if(comboTime > 0)
        {
            comboTime--;
            if (comboTime <= 0) comboCount = 0;
        }

        //tren mat nuoc
        if (isUpSurFaceWater() && isUnderSurfacewater)
        {
            rb.drag = 0f;
            rb.angularDrag = 0f;

            SoundManager.Instance.PlayBGM(BGMType.UpSurFaceWater);
            isUnderSurfacewater = false;
        }
        //duoi mat nuoc
        else if(!isUpSurFaceWater() && !isUnderSurfacewater) 
        {
            rb.drag = waterDrag;
            rb.angularDrag = waterDrag;
            isUnderSurfacewater = true;

            SoundManager.Instance.PlayBGM(BGMType.UnderSurFaceWater);
        }

        flip(spriteRenderer.bounds.center);
    }

    public void init()
    {
        lv.lv = LevelType.child;
        spriteRenderer.sprite = data.sprite;
        ani.runtimeAnimatorController = data.ani;
        transform.localScale = data.scaleLV[0]*GameManager.instance.setting.mapScale;
        headRadius = data.radiusHead * transform.localScale.y;
    }

    public void changeState(IState state)
    {
        stateManager.changeState(state);
    }

    public void addExp(float exp)
    {
        float max_value = GameManager.instance.uiGame.slider_Evolution.maxValue;

        GameManager.instance.uiGame.slider_Evolution.value += 
            ((exp / lv.exp_MaxExp.y) * (max_value/3));
        
        // reward exp
        lv.addExp(exp, this.transform);
        
    }

    public void addScore(EnemyController enemy)
    {
        float baseScore = 5 * (enemy.transform.localScale.y/transform.localScale.y) 
            * enemy.enemyscript._dataFish.expReward;

        comboCount++;
        comboTime = resetComboTime;
        score += (comboCount - 1) * 2 + baseScore;
    }

    public void flip(Vector2 playerPos)
    {
        float diffx = (cursorPos.x - playerPos.x);
        
        if (diffx == 0 || diffx * transform.localScale.x > 0) return;
        
        Vector3 scale = this.transform.localScale;
        Vector3 oldCentre = spriteRenderer.bounds.center;
        if(cursorPos.x > playerPos.x)
        {
            // sang phai
            scale.x = math.abs(scale.x);
            this.transform.localScale = scale;
        }
        else
        {
            // sang trai
            scale.x = -1* math.abs(scale.x);
            this.transform.localScale = scale;
        }

        Vector3 newCentre = spriteRenderer.bounds.center;

        transform.position += oldCentre - newCentre;
    }

    public bool isUpSurFaceWater()
    {
        return transform.position.y > (surFaceSea.position.y + 0.35f);
    }



    public bool isDis2SurfaceWater(float e, string key)
    {
        if(key == "<")
            return math.abs(transform.position.y - (surFaceSea.position.y + 0.35f)) 
            <= e;
        else if(key == ">")
            return math.abs(transform.position.y - (surFaceSea.position.y + 0.35f))
            >= e;

        return false;

    }

    public void ondead() 
    {
        this.gameObject.SetActive(false);
        GameManager.instance.statGame.isLose = true;
    }

    float disyJump;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, headRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, cursorPos);

        Gizmos.color = Color.red;
        Vector2 v = surFaceSea.position;
        v.y = surFaceSea.position.y+ disyJump;
        Gizmos.DrawRay(new Ray(v, Vector2.right));
        Gizmos.DrawRay(new Ray(v, Vector2.left)); 
        
        v.y = surFaceSea.position.y-disyJump;
        Gizmos.DrawRay(new Ray(v, Vector2.right));
        Gizmos.DrawRay(new Ray(v, Vector2.left));

    }
}
