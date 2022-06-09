using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunkens : MonoBehaviour
{
    public float atkSunkens;
    public float hurtCD;
    private float tempHurtCD;
    public bool isHurt = false;
    private Collider2D playerCollider = null;

    private void Start() {
        tempHurtCD = hurtCD;
        hurtCD = 0;
    }

    private void Update() {
        if (isHurt) {
            if (hurtCD > 0) {
                hurtCD -= Time.deltaTime;
            }
            else if (hurtCD <= 0) {
                PlayerController player = playerCollider.gameObject.GetComponent<PlayerController>();
                player.Hurt(atkSunkens, true);
                hurtCD = tempHurtCD;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCollider = collision;
            isHurt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            isHurt = false;
            hurtCD = 0;
        }
    }
}
