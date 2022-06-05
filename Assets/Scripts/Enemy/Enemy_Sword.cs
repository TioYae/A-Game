using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : Enemy {
    public GameObject sword; // ��
    private Animator animSword;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        animSword = sword.GetComponent<Animator>();
        moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
        attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�

        SetATK(); // Ϊ�����蹥����
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
                animSword.SetTrigger("attack");
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
        Sword sw = sword.GetComponent<Sword>();
        sw.SetAttack(atk);
    }

    public override void Hurt(float hurtBlood) {
        if (blood == 0) return;

        // ����ʱȡ�����Ĵ���������
        if (hurtBlood >= blood) animSword.SetBool("death", true);
        base.Hurt(hurtBlood);
    }
}
