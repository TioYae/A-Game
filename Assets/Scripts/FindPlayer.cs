using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            Enemy enemy = this.transform.parent.GetComponent<Enemy>();
            enemy.FoundPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            Enemy enemy = this.transform.parent.GetComponent<Enemy>();
            enemy.LostPlayer();
        }
    }
}
