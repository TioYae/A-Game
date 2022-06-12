using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject popupDamage;
    private float atk;

    public void SetAttack(float atk) {
        this.atk = atk;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // ���˴򵽳����ϰ���
        if (collision.gameObject.tag.Equals("Ground")) {
            Destroy(this.gameObject);
        }
        // ���˴򵽶�
        else if (collision.gameObject.tag.Equals("Shield")) {
            if ((this.transform.position.x - collision.transform.position.x) * collision.transform.localScale.x < 0) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.tag = "Player";
                player.Hurt(atk, true);
            }
            else {
                // ����˺�����λ�ò���
                Vector2 position = new Vector2(collision.transform.position.x, collision.transform.position.y + 1f);
                // �˺�����
                GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
                obj.GetComponent<DamagePopup>().value = 0;
            }
            Destroy(this.gameObject);
        }
        // ���˴����
        else if (collision.gameObject.tag.Equals("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.tag = "Player";
            player.Hurt(atk, true);

            // ����������1/2�ĸ���ʹ��ҽ����쳣״̬
            if (Random.Range(0, 2) == 0) {
                // ����ǻ��浯���������
                if (this.name.Contains("Fire")) 
                    player.SetStatus("Fire", atk / 5);
                // �����ˮ������ҳ���
                else if (this.name.Contains("Water"))
                    player.SetStatus("Water", 0);
            }
            Destroy(this.gameObject);
        }
    }
}
