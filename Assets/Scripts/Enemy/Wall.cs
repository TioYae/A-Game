using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    private GameObject father;

    // Use this for initialization
    void Start() {
        father = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {
        //this.transform.position = father.transform.position;
    }
}
