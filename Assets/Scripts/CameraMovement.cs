using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    PlayerController p1, p2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p1 == null) { p1 = GameObject.Find("P1").GetComponent<PlayerController>(); }
        if(p2 == null) { p2 = GameObject.Find("P2").GetComponent<PlayerController>(); }

        if(p1 && p1.IsAlive() && p2 && p2.IsAlive()) {
            transform.position = (p1.transform.position + p2.transform.position) * 0.5f;
        } else if(p1 && p1.IsAlive()) {
            transform.position = p1.transform.position;
        } else if (p2 && p2.IsAlive()) {
            transform.position = p2.transform.position;
        }
    }
}
