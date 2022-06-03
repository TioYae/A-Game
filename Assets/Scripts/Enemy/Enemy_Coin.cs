using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Coin : Enemy {
    public GameObject enemy;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
        attackDealyTime = Random.Range(5f, 10f); // ����5s��10s�Ĺ����ӳ�
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
                //anim.SetBool("attacking", true);
                // todo 
                // Ѱ��ս��״̬�����е�Ѫ����͵ģ����лظ�
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
                    moving = true;
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

    // Ϊ���ѻ�Ѫ����д���෽��
    public override void SetATK() {
        int n = enemy.transform.childCount;
        float min = Mathf.Infinity;
        Enemy e = this.GetComponent<Enemy>();
        for(int i = 0; i < n; i++) {
            float t = enemy.transform.GetChild(i).GetComponent<Enemy>().GetBlood();
            if (t != -1 && t < min) {
                e = enemy.transform.GetChild(i).GetComponent<Enemy>();
                min = t;
            }
        }
        e.BloodUp(atk);
    }
}
