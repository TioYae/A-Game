using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Debug.Log(123);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log(this.name);
    }
    private void OnTriggerStay(Collider collider) {
        Debug.Log("stay:"+this.name);
    }
}
