using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float atk;

    public void SetAttack(float atk) {
        this.atk = atk;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // ���˴򵽶ܻ򳡾��ϰ���
        if (collision.gameObject.tag.Equals("Shield")|| collision.gameObject.tag.Equals("Ground")) {
            Destroy(this.gameObject);
        }
        // ���˴����
        else if (collision.gameObject.tag.Equals("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            // û����������
            player.Hurt(atk);
            // ����������1/2�ĸ���ʹ��ҽ����쳣״̬
            if (Random.Range(0, 2) == 0) {
                // ����ǻ��浯���������
                if (this.name.Equals("Fire"))
                    player.SetStatus("Fire", atk / 5);
                // �����ˮ������ҳ���
                else if (this.name.Equals("Water"))
                    player.SetStatus("Water", 0);
            }
            Destroy(this.gameObject);
        }
    }
}
