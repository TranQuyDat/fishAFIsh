using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpPower;
    [SerializeField] private GameObject GroundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private GameObject dialoguebox;
    [SerializeField] private GameObject bagobj;
    [SerializeField] private GameObject Shopobj;


    private Vector3 offset;

    private int m_idlefox;
    private int m_runfox;
    private int m_jumpfox;
    private int m_crouchfox;
    private bool m_isrightface = true;
    
    private float m_horizon;
    private NpcController npcController;

    private bool m_iscrouch =false;
    private bool m_isjump = false;
    private bool m_isrun = false;
    private bool isclose = false;
    private bool setopenshop = false;
    // Start is called before the first frame update
    void Start()
    {
        m_idlefox = Animator.StringToHash("isidle");
        m_jumpfox = Animator.StringToHash("isjump");
        m_runfox = Animator.StringToHash("isrun");
        m_crouchfox = Animator.StringToHash("iscrouch");
        
    }

    private void Update()
    {

        Dialogue();
    }

    private void FixedUpdate()
    {
        
        m_rb.velocity = new Vector2(m_horizon * m_speed, m_rb.velocity.y);
        if (!m_isrightface && m_horizon > 0)
        {
            Flip();
        }
        else if (m_isrightface && m_horizon < 0)
        {
            Flip();
        }

        m_animator.SetFloat("yVelocity", m_rb.velocity.y);

        activeAnim();
    }
    public void click_btn_Shop()
    {
        setopenshop = true;
        Shopobj.SetActive(setopenshop);
    }
    public void click_close_Shop()
    {
        setopenshop = false;
        Shopobj.SetActive(setopenshop);
    }



    private void Dialogue ()
    {
        if (Input.GetKeyDown(KeyCode.E) && isclose)
        {
            if (!dialoguebox.active && npcController != null)
            {
                npcController.Emptydialogue();
                dialoguebox.SetActive(true);
                npcController.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC") )
        {
            npcController = other.GetComponent<NpcController>();
            if(npcController != null )
                isclose = true;
            else isclose = false;

        }
    }

    [Obsolete]
    private void OnTriggerExit2D(Collider2D collision)
    {
        isclose = false;
        if (dialoguebox != null && dialoguebox.active)
        {
            dialoguebox.SetActive(false);
        }  
        if (!collision.CompareTag("NPC") && npcController !=null)
        {
            npcController.Emptydialogue();    
            
        }
    }

    private void activeAnim()
    {
        if (m_isrun && m_rb.velocity.y == 0f) playrunanim();
        else if (m_isjump && m_rb.velocity.y != 0) playjumpanim();
        else if (m_iscrouch) playcrouchanim();
        else
        {
            playidleanim();
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.transform.position,
                                       0.05f , groundLayer) ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.transform.position,0.05f);
    }

    private void Flip()
    {
        m_isrightface = !m_isrightface;
        transform.localScale = new Vector2 (transform.localScale.x * -1f, transform.localScale.y);
    }

    public void Move(InputAction.CallbackContext context) 
    {
        m_horizon = context.ReadValue<Vector2>().x;
        if (m_horizon!=0)
        {  m_isrun = true; }
        else { m_isrun = false; }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded() && context.ReadValue<Vector2>().y !=0)
        {
            m_isjump = true;
            m_rb.velocity = new Vector2(m_rb.velocity.x,m_jumpPower);
        }
    }
    public void Crounch(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded())
        {
            m_iscrouch = true;
        }
        if (context.canceled)
        {
            m_iscrouch = false;
        }
    }

    [ContextMenu("play idle fox")]
    private void playidleanim()
    {
        m_animator.SetBool(m_idlefox, true);
        m_animator.SetBool(m_jumpfox, false);
        m_animator.SetBool(m_runfox, false);
        m_animator.SetBool(m_crouchfox, false);
    }

    [ContextMenu("play run fox")]
    private void playrunanim()
    {
        m_animator.SetBool(m_idlefox, false);
        m_animator.SetBool(m_jumpfox, false);
        m_animator.SetBool(m_runfox, true);
        m_animator.SetBool(m_crouchfox, false);
    }

    [ContextMenu("play jumple fox")]
    private void playjumpanim()
    {
        m_animator.SetBool(m_idlefox, false);
        m_animator.SetBool(m_jumpfox, true);
        m_animator.SetBool(m_runfox, false);
        m_animator.SetBool(m_crouchfox, false);
    }

    private void playcrouchanim()
    {
        m_animator.SetBool(m_idlefox, false);
        m_animator.SetBool(m_jumpfox, false);
        m_animator.SetBool(m_runfox, false);
        m_animator.SetBool(m_crouchfox, true);
    }

}
