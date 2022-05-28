using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private float atk;
    private bool define = false;

    public void SetAttack(float atk) {
        this.atk = atk;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // 玩家打到敌人
        if (this.transform.parent.tag.Equals("Player") && collision.gameObject.tag.Equals("Enemy")) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hurt(atk);
        }
        // 敌人发起攻击
        else if (this.transform.parent.tag.Equals("Enemy")) {
            // 敌人打到盾
            if (collision.gameObject.tag.Equals("Shield")) {
                define = true;
            }
            // 敌人打到玩家
            else if (collision.gameObject.tag.Equals("Player")) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                // 没防御，受伤
                if (!define) player.Hurt(atk);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // 攻击结束，取消确认防御状态
        if (collision.gameObject.tag.Equals("Player")) {
            define = false;
        }
    }
}
