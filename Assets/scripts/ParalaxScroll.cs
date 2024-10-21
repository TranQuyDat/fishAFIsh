using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxScroll : MonoBehaviour
{
    public Transform[] Bgs;
    public float[] scales;
    public float smooth = 1f;

    Transform cam;
    Vector2 preCamPos;
    private void Awake()
    {
        cam = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        preCamPos = cam.position;
    }

    // Update is called once per frame
    void Update()
    {
    
        float disx = (preCamPos.x - cam.position.x);
        float disy = (cam.position.y - preCamPos.y );
        for (int i = 0; i < Bgs.Length; i++)
        {
             float targetx = Bgs[i].position.x + (disx * scales[i]);
             float targety = Bgs[i].position.y + (disy * scales[i]);
             Vector3 target = new Vector3(targetx, targety, 
                 Bgs[i].position.z);

           // print(this.name+Bgs[i].name +" "+target);
            Bgs[i].position = Vector3.Slerp(Bgs[i].position,target,smooth*Time.deltaTime);
        }
        preCamPos = cam.position;

    }
}
