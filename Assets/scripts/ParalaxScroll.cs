using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParalaxScroll : MonoBehaviour
{
    public Transform[] Bgs;
    public float[] scales;
    public float smooth = 1f;
    public Transform center;

    Transform cam;
    public Vector2 prePos;
    public Vector2 dis;
    private void Awake()
    {
        cam = Camera.main.transform;
    }
    private void Start()
    {
        prePos = cam.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (prePos == (Vector2)cam.position) return;
        dis.x = math.clamp(prePos.x - cam.position.x, -1f, 1f);
        dis.y = math.clamp(cam.position.y - prePos.y, -1f, 1f);
        for (int i = 0; i < Bgs.Length; i++)
        {
             float targetx = Bgs[i].position.x + (dis.x * scales[i]);
             float targety = Bgs[i].position.y + (dis.y * scales[i]);
             Vector3 target = new Vector3(targetx, targety, 
                 Bgs[i].position.z);

            Bgs[i].position = Vector3.Slerp(Bgs[i].position,target,smooth*Time.deltaTime);

        }
    }

    private void LateUpdate()
    {
        prePos = cam.position;
    }

}
