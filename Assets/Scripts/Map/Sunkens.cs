using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunkens : MonoBehaviour
{
    public float atkSunkens;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Hurt(atkSunkens);
        }
    }
}
