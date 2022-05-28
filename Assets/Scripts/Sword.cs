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
        // ��Ҵ򵽵���
        if (this.transform.parent.tag.Equals("Player") && collision.gameObject.tag.Equals("Enemy")) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hurt(atk);
        }
        // ���˷��𹥻�
        else if (this.transform.parent.tag.Equals("Enemy")) {
            // ���˴򵽶�
            if (collision.gameObject.tag.Equals("Shield")) {
                define = true;
            }
            // ���˴����
            else if (collision.gameObject.tag.Equals("Player")) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                // û����������
                if (!define) player.Hurt(atk);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // ����������ȡ��ȷ�Ϸ���״̬
        if (collision.gameObject.tag.Equals("Player")) {
            define = false;
        }
    }
}
