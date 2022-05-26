using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : Enemy {
    private bool move = false;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = this.transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (!move) {
            rb.gravityScale = 100000000000000;
        }
        else {
            rb.gravityScale = 1;
        }
    }
}
