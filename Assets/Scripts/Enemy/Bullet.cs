using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float atk;

    public void SetAttack(float atk) {
        this.atk = atk;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // 敌人打到场景障碍物
        if (collision.gameObject.tag.Equals("Ground")) {
            Destroy(this.gameObject);
        }
        // 敌人打到盾
        else if (collision.gameObject.tag.Equals("Shield")) {
            if ((this.transform.position.x - collision.transform.position.x) * collision.transform.localScale.x < 0) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.tag = "Player";
                player.Hurt(atk);
            }
            Destroy(this.gameObject);
        }
        // 敌人打到玩家
        else if (collision.gameObject.tag.Equals("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.tag = "Player";
            player.Hurt(atk);
            // 法术攻击有1/2的概率使玩家进入异常状态
            if (Random.Range(0, 2) == 0) {
                // 如果是火焰弹，玩家烧伤
                if (this.name.Contains("Fire"))
                    player.SetStatus("Fire", atk / 5);
                // 如果是水弹，玩家迟滞
                else if (this.name.Contains("Water"))
                    player.SetStatus("Water", 0);
            }
            Destroy(this.gameObject);
        }
    }
}
