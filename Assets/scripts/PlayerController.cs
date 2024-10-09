using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public StateManager stateManager;
    public Transform surFaceSea;
    public Animator ani;
    public Rigidbody2D rb;
    public Vector2 cursorPos;
    float waterDrag = 10f;
    // Start is called before the first frame update
    void Start()
    {
        stateManager.speed = speed;
        StateManager.changeState(new IdleState(rb, this));
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dis = ((Vector2)this.transform.position - cursorPos).magnitude;

        if (transform.position.y > (surFaceSea.position.y))
        {
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }
        else
        {
            rb.drag = waterDrag;
            rb.angularDrag = waterDrag;
        }

        flip(transform.position);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, cursorPos);
    }
}
