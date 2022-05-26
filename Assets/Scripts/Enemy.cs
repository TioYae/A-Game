using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float blood;

    public void Hurt(float hurtBlood) {
        if (hurtBlood > blood) {
            blood = 0;
            Dead();
        }
        else {
            blood -= hurtBlood;
        }
    }

    private void Dead() {
        // todo
        Debug.Log(this.name + "Dead");
    }
}
