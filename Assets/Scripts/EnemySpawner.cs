using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if(col.transform.GetComponent<PlayerController>() != null) {
            //Move both players to that position
            if(GameObject.Find("P1")) {
                GameObject.Find("P1").transform.position = col.transform.position;
            }
            if(GameObject.Find("P2")) {
                GameObject.Find("P2").transform.position = col.transform.position;
            }

            if(transform.GetSiblingIndex() > 0) {
                transform.parent.GetChild(transform.GetSiblingIndex()-1).GetComponent<Collider2D>().isTrigger = false;
            }
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
