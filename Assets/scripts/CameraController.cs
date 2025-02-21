using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public Transform centerLimit;
    public Vector2 sizefolow;
    public Vector2 sizeLimit;
    public float speed;
    public Vector3 camPos;
    Vector3 nextPos;

    private Camera cam;
    Rect rect;
    private void Awake()
    {
        cam = Camera.main;
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

        // --- Giới hạn Camera ---
        float halfWidth = cam.orthographicSize * cam.aspect;
        float halfHeight = cam.orthographicSize;

        float limitxMin = centerLimit.position.x - sizeLimit.x / 2 + halfWidth;
        float limitxMax = centerLimit.position.x + sizeLimit.x / 2 - halfWidth;
        float limityMin = centerLimit.position.y - sizeLimit.y / 2 + halfHeight;
        float limityMax = centerLimit.position.y + sizeLimit.y / 2 - halfHeight;

        // Giữ camera trong giới hạn
        nextPos.x = Mathf.Clamp(nextPos.x, limitxMin, limitxMax);
        nextPos.y = Mathf.Clamp(nextPos.y, limityMin, limityMax);

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
