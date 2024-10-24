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
    public Transform headPos;
    public float headRadius;
    public LayerMask layerFood;
    float waterDrag = 10f;
    public Collider2D food;
    // Start is called before the first frame update
    void Start()
    {
        stateManager.speed = speed;
        StateManager.changeState(new SwimState(rb, this));
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        food = Physics2D.OverlapCircle(headPos.position,
            headRadius, layerFood);
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

   public void destroyFood()
    {
        Destroy(food.gameObject);
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

    public float disyJump;
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
