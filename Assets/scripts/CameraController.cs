using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public Transform centerLimit;
    public Vector2 sizefolow;
    public Vector2 sizeLimit;
    public float speed;
    public Vector3 camPos;
    Vector3 nextPos;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
    }
    private void Update()
    {
        camPos = transform.position;
        Rect rect = new Rect(player.transform.position, sizefolow);
        if (rect.Contains(camPos)) return;
        Vector3 dir = (player.transform.position - camPos).normalized;
        nextPos += dir * speed * Time.deltaTime ;
        nextPos.z = transform.position.z;
        transform.position = nextPos; 
        
    }
    public bool enGizmos = true;
    private void OnDrawGizmos()
    {
        if (!enGizmos) return;
        Gizmos.color = Color.red;
        if (camPos == null) return;
        Gizmos.DrawWireCube(player.transform.position, sizefolow);
        if (centerLimit == null) return;
        Gizmos.DrawWireCube(centerLimit.position, sizeLimit);
    }
}
