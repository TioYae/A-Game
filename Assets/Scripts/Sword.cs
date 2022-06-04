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
        // ��Ҵ򵽵���
        if (this.transform.parent.tag.Equals("Player") && collision.gameObject.tag.Equals("Enemy")) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hurt(atk);
            // �˺�����
            GameObject obj = Instantiate(popupDamage, collision.transform.position, Quaternion.identity);
            obj.GetComponent<DamagePopup>().value = atk;
        }
        // ���˷��𹥻�
        else if (this.transform.parent.tag.Equals("Enemy")) {
            // ���˴򵽶�
            if (collision.gameObject.tag.Equals("Shield")) {
                // ����򵽶�
                if((this.transform.parent.position.x - collision.transform.position.x) * collision.transform.localScale.x < 0) {
                    PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                    player.tag = "Player";
                    player.Hurt(atk); 
                    // �˺�����
                    GameObject obj = Instantiate(popupDamage, collision.transform.position, Quaternion.identity);
                    obj.GetComponent<DamagePopup>().value = atk;
                }
                else {
                    // �˺�����
                    GameObject obj = Instantiate(popupDamage, collision.transform.position, Quaternion.identity);
                    obj.GetComponent<DamagePopup>().value = 0;
                }
            }
            // ���˴����
            else if (collision.gameObject.tag.Equals("Player")) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.tag = "Player";
                player.Hurt(atk);
                // �˺�����
                GameObject obj = Instantiate(popupDamage, collision.transform.position, Quaternion.identity);
                obj.GetComponent<DamagePopup>().value = atk;
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision) {
        // ����������ȡ��ȷ�Ϸ���״̬
        if (collision.gameObject.tag.Equals("Player")) {
            define = false;
        }
    }*/
}
