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

    private Camera cam;
    public Vector2 posFollow;
    Rect rect;
    private void Awake()
    {
        cam = Camera.main;
        player = FindAnyObjectByType<PlayerController>();
        posFollow = new Vector2(player.transform.position.x, player.transform.position.y+
            (player.transform.localScale.y/2));
        rect = new Rect(posFollow, sizefolow);
    }
    private void Update()
    {
        posFollow = new Vector2(player.transform.position.x, player.transform.position.y +
            (player.transform.localScale.y / 2));
        rect.position = posFollow;

        if (rect.Contains(camPos)) return;
        flowTarger(posFollow);
    }

    public void flowTarger(Vector3 targetPos)
    {
        camPos = transform.position;
        Vector3 dir = (targetPos - camPos).normalized;
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
        Gizmos.DrawWireCube(posFollow, sizefolow);
        if (centerLimit == null) return;
        Gizmos.DrawWireCube(centerLimit.position, sizeLimit);
    }
}
