using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WandAndCup : Enemy {
    public GameObject bullet;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
        attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        // ׼���ù������ȴ������ӳ٣�����ԣ�
        if (readyToAttack) {
            // ���ó���
            int tag;
            if (player.transform.position.x < transform.position.x) tag = -1;
            else tag = 1;
            transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // �ӳ�ʱ�䵽��
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�
                anim.SetTrigger("attack");
                anim.SetBool("attacking", true);
                // ����prefab�������ӵ�
                SetATK();
            }
            // �ӳ�ʱ��û��
            else attackDealyTime -= Time.deltaTime;
        }
        else {
            // �ڲ�׷����ҵ�ʱ��ȴ��ӳ�ʱ�䴥���ƶ�
            if (!follow) {
                // �ӳ�ʱ�䵽��
                if (moveDealyTime <= 0) {
                    if (!moving) destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                    MoveToDestination();
                }
                // �ӳ�ʱ��û��
                else {
                    moveDealyTime -= Time.deltaTime;
                }
            }
            // ׷�����ʱ�ж��Ƿ��뿪̫Զ
            else {
                LeaveFarOrNot();
            }
        }

        if (found) FollowPlayer();
        // ��Ҳ������з�Χ�ڣ�ȡ��׷��
        else if (follow) {
            follow = false; // ����׷��
            moving = false; // �����ƶ�
            // �ƶ��ٶ�����
            SetVelocity(0);
            // ����뿪��Ѳ�����򣬷���
            if (transform.position.x > right_x || transform.position.x < left_x) {
                moveDealyTime = 0;
                destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                MoveToDestination();
            }
            // û�뿪��ԭ�ش���
            else
                anim.SetBool("running", false);
        }
    }

    // ���蹥��������д���෽��
    public override void SetATK() {
        base.SetATK();
        GameObject obj = Instantiate(bullet);
        obj.SetActive(true);
        obj.GetComponent<Bullet>().SetAttack(atk);
        //obj.transform.parent = this.transform;
        obj.transform.position = this.transform.position;
        Rigidbody2D bulletRb = obj.GetComponent<Rigidbody2D>();

        float speed = 4f;
        float x1 = player.transform.position.x, y1 = player.transform.position.y + 1f; // ���λ�ò���
        float x2 = this.transform.position.x, y2 = this.transform.position.y;
        float s = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        float x = speed * (x1 - x2) / s, y = speed * (y1 - y2) / s;
        // ��������
        if (transform.localScale.x < 0) {
            obj.transform.localScale = new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            //bulletRb.velocity = new Vector2(-4, 0);
            bulletRb.velocity = new Vector2(x, y);
        }
        else {
            //bulletRb.velocity = new Vector2(4, 0);
            bulletRb.velocity = new Vector2(x, y);
        }

    }
}
