using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < transform.childCount; i++) {
            //Killed all enemies
            bool killedAll = true;
            for(int j = 0; j < transform.GetChild(i).childCount; j++) {
                if(transform.GetChild(i).GetChild(j).GetComponent<Enemy_Class>().IsAlive()) {
                    return;
                }
            }
            if(i < transform.childCount - 1) {
                //Next room can be allowed
                transform.GetChild(i+1).GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}
