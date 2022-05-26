using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collider) {
        // 玩家打到敌人
        if (this.transform.parent.tag.Equals("Player") && collider.gameObject.tag.Equals("Enemy")) {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            enemy.Hurt(10);
        }
        // 敌人打到玩家
        else if(this.transform.parent.tag.Equals("Enemy") && collider.gameObject.tag.Equals("Player")) {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
        }
    }
}
