using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject p1, p2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p1 == null) { p1 = GameObject.Find("P1"); }
        if(p2 == null) { p2 = GameObject.Find("P2"); }

        if(p1) {
            transform.position = p1.transform.position;
        }

        if(p2) {
            transform.position *= 0.5f;
            transform.position += p2.transform.position * 0.5f;
        }

        if(p1) {
            transform.position += Vector3.forward * -10;
        }
    }
}
