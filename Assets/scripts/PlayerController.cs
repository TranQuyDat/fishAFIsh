using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public StateManager stateManager;
    public Animator ani;
    public Rigidbody2D rb;
    public Vector2 cursorPos;
    // Start is called before the first frame update
    void Start()
    {
        stateManager.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dis = ((Vector2)this.transform.position - cursorPos).magnitude;
        movement(dis);
        flip();
    }

    public void movement(float dis)
    {
        if (dis <= 0.3f && StateManager.stateTyle != StateTyle.swim) 
            return;
        if (dis <= 0.3f && StateManager.stateTyle == StateTyle.swim)
        { 
            stateManager.stopState();
            return;
        }
        if(StateManager.stateTyle != StateTyle.swim)
        {
            stateManager.changeState(new SwimState(rb, speed, ani) ) ;
            return;
        }
        stateManager.RepeatState();
    }

    public void flip()
    {
        if (cursorPos.x == this.transform.position.x) return;
        Vector3 scale = this.transform.localScale;
        if(cursorPos.x > this.transform.position.x)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, cursorPos);
    }
}
