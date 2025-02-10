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
        if ((int)lv == newlv) return;
        lv = (LevelType)(newlv);
        obj.localScale = (Vector2)obj.localScale * newlv;

        PlayerController playerCtrl = obj.GetComponent<PlayerController>();

        //neu lv up thi update head radius
        playerCtrl.headRadius *= obj.localScale.y;

    }
}
public class PlayerController : MonoBehaviour
{
    [Header("Info settings")]
    public float speed;
    public Level lv;
    [Header("Interaction settings")]
    public Rigidbody2D rb;
    public Transform surFaceSea;
    public Vector2 cursorPos;
    public Transform headPos;
    public float headRadius;
    public LayerMask layerFood;
    public Collider2D food;
    
    [Header("Animation settings")]
    public Animator ani;
    public ActionType actionType;


    float waterDrag = 10f;
    StateManager stateManager;
    void Start()
    {
        stateManager = new StateManager();
        stateManager.changeState(new SwimState(rb, this));
    }

    // Update is called once per frame
    void Update()
    {
        stateManager.excute();
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        food = Physics2D.OverlapCircle(headPos.position,
            headRadius, layerFood);
        //tren mat nuoc
        if (transform.position.y > (surFaceSea.position.y))
        {
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }
        //duoi mat nuoc
        else
        {
            rb.drag = waterDrag;
            rb.angularDrag = waterDrag;
        }

        flip(transform.position);
    }
    public void changeState(IState state)
    {
        stateManager.changeState(state);
    }
    public void addExp(float exp)
    {
        lv.addExp(exp, this.transform);
    }
    public void flip(Vector2 playerPos)
    {
        float diffx = (cursorPos.x - playerPos.x);
        
        if (diffx == 0 || diffx * transform.localScale.x > 0) return;
        
        Vector3 scale = this.transform.localScale;

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
    }

    float disyJump;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(headPos.position, headRadius);
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
