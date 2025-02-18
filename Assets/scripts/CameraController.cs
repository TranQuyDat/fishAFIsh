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
    Rect rect;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();

        Vector2 pos = (Vector2)player.transform.position;
        //transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        camPos = pos;
        rect = new Rect(player.transform.position, sizefolow);
    }
    private void Update()
    {
        rect.position = player.transform.position;
        if (rect.Contains(camPos)) return;
        flowTarger(player.transform);
    }

    public void flowTarger(Transform target)
    {
        camPos = transform.position;
        Vector3 dir = (target.position - camPos).normalized;
        nextPos = transform.position + dir * speed * Time.deltaTime;
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
