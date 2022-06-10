using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public GameObject popupDamage;
    private float atk;
    //private bool define = false;

    public void SetAttack(float atk) {
        this.atk = atk;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // 玩家打到敌人
        if (this.transform.parent.tag.Equals("Player") && collision.gameObject.tag.Equals("Enemy")) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hurt(atk);
            // 伤害数字
            GameObject obj = Instantiate(popupDamage, collision.transform.position, Quaternion.identity);
            obj.GetComponent<DamagePopup>().value = atk;
        }
        // 敌人发起攻击
        else if (this.transform.parent.tag.Equals("Enemy")) {
            // 敌人打到盾
            if (collision.gameObject.tag.Equals("Shield")) {
                // 背面打到盾
                if((this.transform.parent.position.x - collision.transform.position.x) * collision.transform.localScale.x < 0) {
                    PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                    player.tag = "Player";
                    player.Hurt(atk, true);
                    // 玩家伤害数字位置补偿
                    Vector2 position = new Vector2(collision.transform.position.x, collision.transform.position.y + 1f);
                    // 伤害数字
                    GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
                    obj.GetComponent<DamagePopup>().value = atk;
                }
                else {
                    // 玩家伤害数字位置补偿
                    Vector2 position = new Vector2(collision.transform.position.x, collision.transform.position.y + 1f);
                    // 伤害数字
                    GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
                    obj.GetComponent<DamagePopup>().value = 0;
                }
            }
            // 敌人打到玩家
            else if (collision.gameObject.tag.Equals("Player")) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.tag = "Player";
                player.Hurt(atk, true);
                // 玩家伤害数字位置补偿
                Vector2 position = new Vector2(collision.transform.position.x, collision.transform.position.y + 1f);
                // 伤害数字
                GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
                obj.GetComponent<DamagePopup>().value = atk;
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision) {
        // 攻击结束，取消确认防御状态
        if (collision.gameObject.tag.Equals("Player")) {
            define = false;
        }
    }*/
}
