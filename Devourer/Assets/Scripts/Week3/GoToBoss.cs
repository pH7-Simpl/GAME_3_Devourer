using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBoss : MonoBehaviour
{
    private bool executed;
    private void Awake() {
        executed = false;
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            if (other.transform.childCount > 3)
            {
                for (var i = 4; i < other.transform.childCount; i++)
                {
                    Object.Destroy(other.transform.GetChild(i).gameObject);
                }
            }
            executed = true;
        }
    }
    private void Update() {
        if(executed) {
            transform.parent.GetComponent<LiftMechanism>().GoToBoss();
            Destroy(gameObject, Time.deltaTime);
        }
    }
}
